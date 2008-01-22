using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
    public partial class SharpCore
    {
        private static int MinutesSinceCrashSave = 0;
        private static long Pulse = 0;

        private List<DescriptorData> _descriptors = new List<DescriptorData>();
        private Queue<TcpClient> _newClients = new Queue<TcpClient>();

		private TcpListener _listener;

        private int _maxPlayers = 0;
		private int _actCheck;

        private void InitializeGame()
        {
            _running = true;

			Log ("Listening on port " + GlobalSettings.ListenPort + ".");

            Log ("Finding player limit.");
            _maxPlayers = GetMaxPlayers();

            BootDatabase();

            GameLoop();

            if (Stopped != null)
                Stopped(this, new EventArgs());
        }

        private void GameLoop()
        {
            DateTime lastTime, beforeSleepTime;
            TimeSpan timeoutTime;
            TimeSpan nullTime = new TimeSpan(0, 0, 0, 0, 0);
            TimeSpan optTime = new TimeSpan(0, 0, 0, 0, GlobalConstants.OPT_USEC);
            string command = String.Empty;

            int missedPulses = 0;

            // Setup the tcp socket listener
			_listener = new TcpListener (IPAddress.Any, GlobalSettings.ListenPort);
			
            _listener.Start ();
			
            // Initialize the time values
            lastTime = DateTime.Now;

			Log ("Entering game loop.");

            while (_running)
            {
                if (_descriptors.Count < 1)
                {
                    Log ("No connections. Going to sleep.");

					try
					{
						_newClients.Enqueue (_listener.AcceptTcpClient ());
					}
					catch (SocketException e)
					{
						if (e.SocketErrorCode != SocketError.Interrupted)
							Log ("SYSERR: Socket Exception [" + e.SocketErrorCode + "]: " + e.Message);

						break;
					}
					
                    Log ("New connection. Waking up.");

                    lastTime = DateTime.Now;
                }

                beforeSleepTime = DateTime.Now;
                TimeSpan processTime = beforeSleepTime.Subtract(lastTime);

                if (processTime.Seconds == 0 && processTime.Milliseconds < GlobalConstants.OPT_USEC)
                {
                    missedPulses = 0;
                }
                else
                {
                    missedPulses = processTime.Seconds * GlobalConstants.PASSES_PER_SEC;
                    missedPulses += processTime.Milliseconds / GlobalConstants.OPT_USEC;

                    processTime = new TimeSpan(0, 0, 0, 0, processTime.Milliseconds % GlobalConstants.OPT_USEC);
                }

                TimeSpan tempTime = optTime.Subtract(processTime);
                lastTime = beforeSleepTime.Add(tempTime);

                timeoutTime = lastTime.Subtract(DateTime.Now);

                do
                {
                    System.Threading.Thread.Sleep(timeoutTime.Seconds * 1000 + timeoutTime.Milliseconds);
                    timeoutTime = lastTime.Subtract(DateTime.Now);
                }
                while (timeoutTime.Milliseconds > 0 || timeoutTime.Seconds > 0);

				try
				{
					if (!_running)
						break;

					if (_listener.Pending ())
						_newClients.Enqueue (_listener.AcceptTcpClient ());
				}
				catch (SocketException e)
				{
					if (e.SocketErrorCode != SocketError.Interrupted)
						Log ("SYSERR: Socket Exception [" + e.SocketErrorCode + "]: " + e.Message);

					break;
				}

                if (_newClients.Count > 0)
                    NewDescriptor(_newClients.Dequeue());

                // kick out old/bad connections

                foreach (DescriptorData descriptor in _descriptors)
                    if (ProcessInput(descriptor) < 0)
                        descriptor.Connection.Close();

                foreach (DescriptorData descriptor in _descriptors)
                {
                    if (descriptor.Character != null)
                    {
                        if (descriptor.Character.Wait > 0)
                            descriptor.Character.Wait--;

                        if (descriptor.Character.Wait > 0)
                            continue;
                    }

                    if (descriptor.InputQueue.Count < 1)
                        continue;
                    else
                        command = descriptor.InputQueue.Dequeue();

                    if (descriptor.Character != null)
                    {
                        descriptor.Character.CharacterSpecials.Timer = 0;

                        if (descriptor.ConnectState == ConnectState.Playing && descriptor.Character.WasInRoom != GlobalConstants.NOWHERE)
                        {
                            if (descriptor.Character.InRoom != GlobalConstants.NOWHERE)
								CharacterFromRoom(descriptor.Character);

                            CharacterToRoom(descriptor.Character, descriptor.Character.WasInRoom);
                            descriptor.Character.WasInRoom = GlobalConstants.NOWHERE;
                            Act("$n has returned.", true, descriptor.Character, null, null, GlobalConstants.TO_ROOM);
                        }

                        descriptor.Character.Wait = 1;
                    }

                    descriptor.HasPrompt = false;

                    // TODO: Work with pagers, boards, mail, etc.

                    if (descriptor.ConnectState != ConnectState.Playing)
                        Nanny(descriptor, command);
                    else
                    {
                        // TODO: Prevent recursive aliases, when we actually do aliases
                        CommandInterpreter(descriptor.Character, command);
                    }
                }

                foreach (DescriptorData descriptor in _descriptors)
                {
                    if (descriptor.Output != null && descriptor.Output.Length > 0)
                    {
                        ProcessOutput(descriptor);

                        if (descriptor.Output.Length == 0)
                            descriptor.HasPrompt = true;
                    }
                }

                foreach (DescriptorData descriptor in _descriptors)
                {
                    if (!descriptor.HasPrompt && descriptor.Output.Length == 0)
                    {
                        WriteToDescriptor(descriptor, MakePrompt(descriptor));
                        descriptor.HasPrompt = true;
                    }
                }

                foreach (DescriptorData descriptor in _descriptors)
                {
                    if (descriptor.ConnectState == ConnectState.Close || descriptor.ConnectState == ConnectState.Disconnect)
                        CloseSocket(descriptor);
                }

                missedPulses++;

                if (missedPulses <= 0)
                {
                    // Missed pulses non-positive, time is going backwards.
                    missedPulses = 1;
                }

                while (missedPulses-- > 0)
                    Heartbeat(++Pulse);

                // Check for signals
            }

			Log ("Game loop ended.");

			if (Stopped != null)
				Stopped (this, new EventArgs ());

			_listener.Stop ();
        }

        private void Heartbeat(long heartPulse)
        {
            //EventProcess();

            //if ((heartPulse % GlobalConstants.PULSE_SCRIPT) == 0)
            //    ScriptTriggerCheck();

            //if ((heartPulse % GlobalConstants.PULSE_ZONE) == 0)
            //    ZoneUpdate();

            //if ((heartPulse % GlobalConstants.PULSE_IDLEPWD) == 0)
            //    CheckIdlePasswords();

            //if ((heartPulse % GlobalConstants.PULSE_MOBILE) == 0)
            //    MobileActivity();

            //if ((heartPulse % GlobalConstants.PULSE_VIOLENCE) == 0)
            //    PerformViolence();

            /*if ((heartPulse % (GlobalConstants.SECS_PER_MUD_HOUR * GlobalConstants.PASSES_PER_SEC)) == 0)
            {
                WeatherAndTime();
                AffectUpdate();
                PointUpdate();
            }*/

            /*if (_autoSave && (heartPulse % GlobalConstants.PULSE_AUTOSAVE) == 0)
            {
                if (++MinutesSinceCrashSave >= _autoSaveTime)
                {
                    CashSaveAll();
                    HouseSaveAll();
                }
            }*/

            //if ((heartPulse % GlobalConstants.PULSE_USAGE) == 0)
            //    RecordUsage();

            //if ((heartPulse % GlobalConstants.PULSE_TIMESAVE) == 0)
            //    SaveMudTime();

            //ExtractPendingCharacters();
        }

        private void CloseSocket(DescriptorData descriptor)
        {
            _descriptors.Remove(descriptor);

            descriptor.Connection.Close();

            // Forget Snooping

            // Reset the switched mobiles/snooping/etc

            // Clear command history

            // Clear any OLC stuff
        }

        private int GetMaxPlayers()
        {
            return GlobalConstants.MAX_PLAYERS;
        }

        private void NewDescriptor(TcpClient client)
        {
            DescriptorData descriptor = new DescriptorData(client);

            // FIXME: Fix the max user count so its global and changable by the admin.

            if (_descriptors.Count >= 150)
            {
                WriteToDescriptor(descriptor, "Sorry, the system is full right now... please try again later!\r\n");
                client.Close();
                return;
            }

            descriptor.Hostname = Dns.GetHostEntry(client.Client.RemoteEndPoint.ToString().Split(':')[0]).HostName;

            // TODO: Determine if the site is banned.

            // TODO: Notify of the new connection in the Mud Log.
            //mudlog(CMP, LVL_GOD, FALSE, "New connection from [%s]", newd->host);

            descriptor.IdleTicks = 0;
            descriptor.LoginTime = DateTime.Now;
            descriptor.HasPrompt = true;
            descriptor.ConnectState = ConnectState.GetName;

            _descriptors.Add(descriptor);

            WriteToOutput(descriptor, _textGreetings);
        }

        private int ProcessInput(DescriptorData descriptor)
        {
            byte[] buffer = new byte[descriptor.Connection.Available];

            if (descriptor.Connection.Available > 0)
            {
                int position = descriptor.RawInputBuffer.Length;

                // Receive available data from the client connection.
                descriptor.Connection.Client.Receive(buffer);

                char[] destination = descriptor.RawInputBuffer;
                char[] charbuffer = Encoding.UTF8.GetChars(buffer);

                Array.Resize(ref destination, destination.Length + charbuffer.Length);
                Array.Copy(charbuffer, 0, destination, position, charbuffer.Length);

                descriptor.RawInputBuffer = destination;

                // Now we have put the raw buffer together with the new data.

                int indexNL = -1;

                // Look for a new line within the data.
                for (int i = 0; i < descriptor.RawInputBuffer.Length; i++)
                {
                    if (descriptor.RawInputBuffer[i] == '\r' || descriptor.RawInputBuffer[i] == '\n')
                    {
                        indexNL = i;
                        break;
                    }
                }

                if (indexNL < 0)
                    return 0;

                while (indexNL >= 0)
                {
                    char[] commandbuffer = new char[indexNL];
                    string command = String.Empty;

                    Array.Copy(descriptor.RawInputBuffer, 0, commandbuffer, 0, indexNL);

                    char[] remainingbuffer = new char[descriptor.RawInputBuffer.Length - indexNL - 1];

                    Array.Copy(descriptor.RawInputBuffer, indexNL + 1, remainingbuffer, 0, descriptor.RawInputBuffer.Length - indexNL - 1);

                    descriptor.RawInputBuffer = remainingbuffer;

                    indexNL = -1;

                    for (int i = 0; i < descriptor.RawInputBuffer.Length; i++)
                    {
                        if (descriptor.RawInputBuffer[i] == '\r' || descriptor.RawInputBuffer[i] == '\n')
                        {
                            indexNL = i;
                            break;
                        }
                    }

                    if (commandbuffer.Length > 0)
                    {
                        foreach (char c in commandbuffer)
                        {
                            if (c == '\b' || c == 127)
                                command = command.Substring(0, command.Length - 1);
                            else
                                command += c;
                        }

                        // TODO: Worry about who to show this command to (snooping).

                        // TODO: Worry about substitution and repetition (^ and !)

                        descriptor.LastInput = command;

                        // TODO: Work with history.

                        descriptor.InputQueue.Enqueue(command);
                    }
                }
            }

            return (buffer.Length);
        }

        private void ProcessOutput(DescriptorData descriptor)
        {
            string output = String.Empty;
            string end = "\r\n";

            output += descriptor.Output;

            // TODO: Deal with overflow?

            //if (descriptor.ConnectState == ConnectState.Playing && descriptor.Character != null && !descriptor.Character.IsNPC && !descriptor.Character.PreferenceFlagged (PRF_COMPACT))
            //  output += "\r\n";

            end += MakePrompt(descriptor);

            if (!descriptor.HasPrompt)
            {
                descriptor.HasPrompt = true;
                WriteToDescriptor(descriptor, end);
            }
            else
            {
                WriteToDescriptor(descriptor, output);

                // TODO: Output to Snooper

                descriptor.Output = String.Empty;
            }
        }

        private int WriteToDescriptor(DescriptorData descriptor, string text)
        {
            int bytesWritten = descriptor.Connection.Client.Send(Encoding.UTF8.GetBytes(text));

            return (bytesWritten);
        }

        private void WriteToOutput(DescriptorData descriptor, string text)
        {
            // this is to add data to the output buffer (queue) to be sent out.
            descriptor.Output += text;
        }

		private bool SendToCharacter (CharacterData character, string message, params object[] vars)
		{
			if (character.Descriptor != null && !String.IsNullOrEmpty(message))
			{
				string text = String.Format(message, vars);

				WriteToOutput(character.Descriptor, text);

				return true;
			}

			return false;
		}

        private string MakePrompt(DescriptorData descriptor)
        {
            return ">";
        }
		
		private void PerformAct (string original, CharacterData character, ObjectData obj, object victim, CharacterData to)
		{
			CharacterData triggerVictim = null;
			ObjectData triggerTarget = null;
			string triggerArg = String.Empty;

			//const char *i = NULL;
			//char lbuf[MAX_STRING_LENGTH], *buf, *j;
			//bool uppercasenext = FALSE;
			//buf = lbuf;

			if (original.IndexOf ("$n") >= 0)
			{
				original = original.Replace ("$n", PersonName (character, to));
			}

			if (original.IndexOf ("$N") >= 0)
			{
				if (victim == null)
					original = original.Replace ("$N", "<NULL>");
				else
					original = original.Replace ("$N", PersonName (victim as CharacterData, to));

				triggerVictim = victim as CharacterData;
			}

			if (original.IndexOf ("$m") >= 0)
			{
				original = original.Replace ("$m", HimHer (character));
			}

			if (original.IndexOf ("$M") >= 0)
			{
				if (victim == null)
					original = original.Replace ("$M", "<NULL>");
				else
					original = original.Replace ("$M", HimHer (victim as CharacterData));

				triggerVictim = victim as CharacterData;
			}

			if (original.IndexOf ("$s") >= 0)
			{
				original = original.Replace ("$s", HisHer (character));
			}

			if (original.IndexOf ("$S") >= 0)
			{
				if (victim == null)
					original = original.Replace ("$M", "<NULL>");
				else
					original = original.Replace ("$M", HisHer (victim as CharacterData));

				triggerVictim = victim as CharacterData;
			}

			if (original.IndexOf ("$e") >= 0)
			{
				original = original.Replace ("$e", HeShe(character));
			}

			if (original.IndexOf ("$E") >= 0)
			{
				if (victim == null)
					original = original.Replace ("$M", "<NULL>");
				else
					original = original.Replace ("$M", HeShe (victim as CharacterData));

				triggerVictim = victim as CharacterData;
			}

			if (original.IndexOf ("$o") >= 0)
			{
				if (obj == null)
					original = original.Replace ("$o", "<NULL>");
				else
					original = original.Replace ("$o", ObjectName (obj, to));
			}

			if (original.IndexOf ("$O") >= 0)
			{
				if (victim == null)
					original = original.Replace ("$O", "<NULL>");
				else
					original = original.Replace ("$O", ObjectName (victim as ObjectData, to));

				triggerTarget = victim as ObjectData;
			}

			if (original.IndexOf ("$p") >= 0)
			{
				if (obj == null)
					original = original.Replace ("$p", "<NULL>");
				else
					original = original.Replace ("$p", ObjectDescription (obj, to));
			}

			if (original.IndexOf ("$P") >= 0)
			{
				if (victim == null)
					original = original.Replace ("$P", "<NULL>");
				else
					original = original.Replace ("$P", ObjectDescription (victim as ObjectData, to));

				triggerTarget = victim as ObjectData;
			}

			if (original.IndexOf ("$a") >= 0)
			{
				if (obj == null)
					original = original.Replace ("$a", "<NULL>");
				else
					original = original.Replace ("$a", SAnA(obj));
			}

			if (original.IndexOf ("$A") >= 0)
			{
				if (victim == null)
					original = original.Replace ("$A", "<NULL>");
				else
					original = original.Replace ("$A", SAnA(victim as ObjectData));

				triggerTarget = victim as ObjectData;
			}

			if (original.IndexOf ("$T") >= 0)
			{
				if (victim == null)
					original = original.Replace ("$T", "<NULL>");
				else
					original = original.Replace ("$T", victim as string);

				triggerArg = victim as string;
			}

			if (original.IndexOf ("$F") >= 0)
			{
				if (victim == null)
					original = original.Replace ("$F", "<NULL>");
				else
					original = original.Replace ("$F", GlobalUtilities.FirstName (victim as string));
			}

			if (original.IndexOf ("$$") >= 0)
				original = original.Replace ("$$", "$");

			if (to.Descriptor != null)
				WriteToOutput (to.Descriptor, original);

			//if ((to.IsNPC && triggerActCheck) && (to != character))
			//	ActMobileTrigger (to, original, character, triggerVictim, obj, triggerTarget, triggerArg);
		}

		private void Act (string text, bool hideInvisible, CharacterData character, ObjectData obj, object victim, int type)
		{
			CharacterData to;
			int toSleeping;

			if (String.IsNullOrEmpty (text))
				return;

			if ((toSleeping = (type & GlobalConstants.TO_SLEEP)) > 0)
				type &= ~GlobalConstants.TO_SLEEP;

			if ((_actCheck = (type & GlobalConstants.DG_NO_TRIG)) > 0)
				type &= ~GlobalConstants.DG_NO_TRIG;

			if (type == GlobalConstants.TO_CHAR)
			{
				if (character != null && SendOK (character, toSleeping))
					PerformAct (text, character, obj, victim, character);

				return;
			}

			if (type == GlobalConstants.TO_VICT)
			{
				to = victim as CharacterData;

				if (to != null && SendOK (to, toSleeping))
					PerformAct (text, character, obj, victim, to);

				return;
			}

			List<CharacterData> people;

			if (character != null && character.InRoom != GlobalConstants.NOWHERE)
				people = _rooms[character.InRoom].People;
			else if (obj != null && obj.InRoom != GlobalConstants.NOWHERE)
				people = _rooms[obj.InRoom].People;
			else
			{
				Log ("SYSERR: No valid target to Act()!");
				return;
			}

			foreach (CharacterData person in people)
			{
				if (!SendOK (person, toSleeping) || (person == character))
					continue;

				if (hideInvisible && character != null && !CanSee (person, character))
					continue;

				if (type != GlobalConstants.TO_ROOM && person == victim)
					continue;

				PerformAct (text, character, obj, victim, person);
			}
		}
    }
}
