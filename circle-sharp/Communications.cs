using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
    public partial class CircleCore
    {
        private static int MinutesSinceCrashSave = 0;
        private static long Pulse = 0;

        private List<DescriptorData> _descriptors = new List<DescriptorData>();
        private Queue<TcpClient> _newClients = new Queue<TcpClient>();

		private TcpListener _listener;

        private int _maxPlayers = 0;
		private int _actCheck;

        private int scriptActTrigger;

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
                {
                    if (ProcessInput(descriptor) < 0)
                    {
                        descriptor.ConnectState = ConnectState.Close;
                        descriptor.Connection.Close();
                    }
                }

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
                        //descriptor.Character.CharacterSpecials.Timer = 0;

                        if (descriptor.ConnectState == ConnectState.Playing && descriptor.Character.WasInRoom != GlobalConstants.NOWHERE)
                        {
                            //if (descriptor.Character.InRoom != NOWHERE)
                                //CharacterFromRoom(descriptor.Character);

                            //CharacterToRoom(descriptor.Character, descriptor.Character.WasInRoom);
                            descriptor.Character.WasInRoom = GlobalConstants.NOWHERE;
                            //act("$n has returned.", TRUE, d->character, 0, 0, TO_ROOM);
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

                for (int i = 0; i < _descriptors.Count; i++)
                {
                    if (_descriptors[i].ConnectState == ConnectState.Close || _descriptors[i].ConnectState == ConnectState.Disconnect)
                        CloseSocket(_descriptors[i]);
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

            TcpClient tcpClient = descriptor.Connection;

            try
            {
                // Test the socket
                descriptor.Connection.Client.Send(Encoding.UTF8.GetBytes("\0"));
            }
            catch (SocketException ex)
            {
                return -1;
            }

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
                    else if (commandbuffer.Length == 0 && indexNL != -1)
                    {
                        descriptor.InputQueue.Enqueue("");
                    }
                }
            }

            return (buffer.Length);
        }

        private void ProcessOutput(DescriptorData descriptor)
        {
            byte[] output = new byte[0];
            string end = "\r\n";

            //output += descriptor.Output;

            // TODO: Deal with overflow?

            //if (descriptor.ConnectState == ConnectState.Playing && descriptor.Character != null && !descriptor.Character.IsNPC && !descriptor.Character.PreferenceFlagged (PRF_COMPACT))
            //  output += "\r\n";

            end += MakePrompt(descriptor);

            if (descriptor.HasPrompt)
            {
                descriptor.HasPrompt = false;
                WriteToDescriptor(descriptor, end);
            }
            else
            {
                WriteToDescriptor(descriptor, descriptor.Output);

                // TODO: Output to Snooper

                descriptor.Output = new byte[0];
            }
        }

        private void EchoOff(DescriptorData descriptor)
        {
            byte[] bytes = new byte[4] { (byte)GlobalConstants.IAC, (byte)GlobalConstants.WILL, (byte)GlobalConstants.TELOPT_ECHO, (byte)0 };
            WriteToOutput(descriptor, bytes);
        }

        private void EchoOn(DescriptorData descriptor)
        {
            byte[] bytes = new byte[4] { (byte)GlobalConstants.IAC, (byte)GlobalConstants.WONT, (byte)GlobalConstants.TELOPT_ECHO, (byte)0 };
            WriteToOutput(descriptor, bytes);
        }

        private string ProcessColors(string text)
        {
            if (text.IndexOf("@") < 0)
                return text;
            else
            {
                string output = String.Empty;
                int position = 0;

                for (position = 0; position < text.Length; position++)
                {
                    if (text[position] == '@')
                    {
                        string code = "\x1B[";

                        switch (text[position + 1])
                        {
                            // @ndbgcrmywDBGCRMYW01234567luoex!
                            case '@':
                                output += "@";
                                break;

                            case 'n':
                                output += code + "0m";
                                break;

                            case 'd':
                                output += code + "0;30m";
                                break;

                            case 'b':
                                output += code + "0;34m";
                                break;

                            case 'g':
                                output += code + "0;32m";
                                break;

                            case 'c':
                                output += code + "0;36m";
                                break;

                            case 'r':
                                output += code + "0;31m";
                                break;

                            case 'm':
                                output += code + "0;35m";
                                break;

                            case 'y':
                                output += code + "0;33m";
                                break;

                            case 'w':
                                output += code + "0;37m";
                                break;

                            case 'D':
                                output += code + "1;30m";
                                break;

                            case 'B':
                                output += code + "1;34m";
                                break;

                            case 'G':
                                output += code + "1;32m";
                                break;

                            case 'C':
                                output += code + "1;36m";
                                break;

                            case 'R':
                                output += code + "1;31m";
                                break;

                            case 'M':
                                output += code + "1;35m";
                                break;

                            case 'Y':
                                output += code + "1;33m";
                                break;

                            case 'W':
                                output += code + "1;37m";
                                break;

                            case '0':
                                output += code + "40m";
                                break;

                            case '1':
                                output += code + "44m";
                                break;

                            case '2':
                                output += code + "42m";
                                break;

                            case '3':
                                output += code + "46m";
                                break;

                            case '4':
                                output += code + "41m";
                                break;

                            case '5':
                                output += code + "45m";
                                break;

                            case '6':
                                output += code + "43m";
                                break;

                            case '7':
                                output += code + "47m";
                                break;

                            case 'l':
                                output += code + "5m";
                                break;

                            case 'u':
                                output += code + "4m";
                                break;

                            case 'o':
                                output += code + "1m";
                                break;

                            case 'e':
                                output += code + "7m";
                                break;

                            case 'x':
                                output += code + "7m";
                                break;

                            case '!':
                                output += code + "!";
                                break;

                        }

                        // Skip the next position, since its been consumed
                        position++;

                    }
                    else
                    {
                        // Not a color code
                        output += text[position];
                    }
                }

                return output;
            }
        }

        private void WriteToOutput(DescriptorData descriptor, string text)
        {
            WriteToOutput(descriptor, Encoding.UTF8.GetBytes(ProcessColors(text)));
        }

        private void WriteToOutput(DescriptorData descriptor, byte[] bytes)
        {
            // this is to add data to the output buffer (queue) to be sent out.
            // Add the bytes to a new byte array with both.
            byte[] totalBytes = new byte[bytes.Length + descriptor.Output.Length];

            Array.Copy(descriptor.Output, 0, totalBytes, 0, descriptor.Output.Length);
            Array.Copy(bytes, 0, totalBytes, descriptor.Output.Length, bytes.Length);

            descriptor.Output = totalBytes;
        }

        private int WriteToDescriptor(DescriptorData descriptor, byte[] bytes)
        {
            int bytesWritten = descriptor.Connection.Client.Send(bytes);

            return (bytesWritten);
        }

        private int WriteToDescriptor(DescriptorData descriptor, string text)
        {
            int bytesWritten = descriptor.Connection.Client.Send(Encoding.UTF8.GetBytes(text));

            return (bytesWritten);
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

        private void Act(string text, bool hideInvisible, CharacterData character, ObjectData obj, object victim, int type)
        {
            int toSleeping;

            if (String.IsNullOrEmpty(text))
                return;

            if ((toSleeping = (type & GlobalConstants.TO_SLEEP)) != 0)
                type &= ~GlobalConstants.TO_SLEEP;

            /* this is a hack as well - DG_NO_TRIG is 256 -- Welcor */
            if ((_actCheck = (type & GlobalConstants.DG_NO_TRIG)) != 0)
                type &= ~GlobalConstants.DG_NO_TRIG;

            if (type == GlobalConstants.TO_CHAR)
            {
                if (character != null && SendOK(character, toSleeping))
                    PerformAct(text, character, obj, victim, character);

                return;
            }

            if (type == GlobalConstants.TO_VICT)
            {
                CharacterData to = victim as CharacterData;

                if (to != null && SendOK(to, toSleeping))
                {
                    PerformAct(text, character, obj, victim, to);
                }

                return;
            }

            if (type == GlobalConstants.TO_GMOTE)
            {
                DescriptorData i;

                foreach (DescriptorData data in _descriptors)
                {
                    if (data.Character != null && data.Character.PreferenceFlagged(PreferenceFlags.NoGossip) && data.Character.PlayerFlagged(PlayerFlags.Writing) &&
                        _rooms[data.Character.InRoom].RoomFlagged(RoomFlags.Soundproof))
                    {
                        SendToCharacter(data.Character, "%s", CharacterData.ColorYellow(data.Character, GlobalConstants.C_NRM));
                        PerformAct(text, character, obj, victim, data.Character);
                        SendToCharacter(data.Character, "%s", CharacterData.ColorNormal(data.Character, GlobalConstants.C_NRM));
                    }
                }

                return;
            }

            List<CharacterData> people = null;

            if (character != null && character.InRoom != GlobalConstants.NOWHERE)
                people = _rooms[character.InRoom].People;
            else if (obj != null && obj.InRoom != GlobalConstants.NOWHERE)
                people = _rooms[obj.InRoom].People;
            else
            {
                Log("SYSERR: no valid target to Act()!");
                return;
            }

            foreach (CharacterData person in people)
            {
                if (!SendOK(person, toSleeping) || person == character)
                    continue;
                if (hideInvisible && character != null && !CanSee(person, character))
                    continue;
                if (type != GlobalConstants.TO_ROOM && person == victim)
                    continue;
                PerformAct(text, character, obj, victim, person);
            }
        }

        void PerformAct (string text, CharacterData character, ObjectData obj, object victimObject, CharacterData to)
        {
            string str = "";
            CharacterData scriptVictim = null;
            ObjectData scriptTarget = null;

            string buffer = String.Empty;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '$')
                {
                    switch (text[++i])
                    {
                        case 'n':
                            str = PersonString(character, to);
                            break;

                        case 'N':
                            scriptVictim = victimObject as CharacterData;

                            if (scriptVictim == null)
                                str = "<NULL>";
                            else
                                str = PersonString(scriptVictim, to);
                            break;

                        case 'm':
                            str = HimHer(character);
                            break;

                        case 'M':
                            scriptVictim = victimObject as CharacterData;

                            if (scriptVictim == null)
                                str = "<NULL>";
                            else
                                str = HimHer(scriptVictim);
                            break;

                        case 's':
                            str = HisHer(character);
                            break;

                        case 'S':
                            scriptVictim = victimObject as CharacterData;

                            if (scriptVictim == null)
                                str = "<NULL>";
                            else
                                str = HisHer(scriptVictim);
                            break;

                        case 'e':
                            str = HeShe(character);
                            break;

                        case 'E':
                            scriptVictim = victimObject as CharacterData;

                            if (scriptVictim == null)
                                str = "<NULL>";
                            else
                                str = HeShe(scriptVictim);
                            break;

                        case 'o':
                            if (obj == null)
                                str = "<NULL>";
                            else
                                str = ObjectName(obj, to);
                            break;

                        case 'O':
                            scriptTarget = victimObject as ObjectData;

                            if (scriptTarget == null)
                                str = "<NULL>";
                            else
                                str = ObjectName(scriptTarget, to);
                            break;

                        case 'p':
                            if (obj == null)
                                str = "<NULL>";
                            else
                                str = ObjectString(obj, to);
                            break;

                        case 'P':
                            scriptTarget = victimObject as ObjectData;

                            if (scriptTarget == null)
                                str = "<NULL>";
                            else
                                str = ObjectString(scriptTarget, to);
                            break;

                        case 'a':
                            if (obj == null)
                                str = "<NULL>";
                            else
                                str = SAnA(obj);
                            break;

                        case 'A':
                            scriptTarget = victimObject as ObjectData;

                            if (scriptTarget == null)
                                str = "<NULL>";
                            else
                                str = SAnA(scriptTarget);
                            break;

                        case 't':
                            if (obj == null)
                                str = "<NULL>";
                            else
                                str = obj.Name;
                            break;

                        case 'T':
                            scriptTarget = victimObject as ObjectData;

                            if (scriptTarget == null)
                                str = "<NULL>";
                            else
                                str = scriptTarget.Name;
                            break;

                        case 'F':
                            if (obj == null)
                                str = "<NULL>";
                            else
                                str = GlobalUtilities.FirstName(obj.Name);
                            break;

                        case 'u':
                            //for (j=buf; j > lbuf && !isspace((int) *(j-1)); j--);
                            //if (j != buf)
                            //  *j = UPPER(*j);
                            //i = "";
                            break;

                        case 'U':
                            //uppercasenext = TRUE;
                            //i = "";
                            break;

                        case '$':
                            str = "$";
                            break;

                        default:
                            Log("SYSERR: Illegal $-code to act(): " + text[i]);
                            Log("SYSERR: " + text);
                            str = "";
                            break;
                    }

                    buffer += str;
                }
                else
                {
                    buffer += text[i];
                }
                //else if (!(*(buf++) = *(orig++)))
                //{
                //    break;
                //}
                //else if (uppercasenext && !isspace((int) *(buf-1)))
                //{
                //    *(buf-1) = UPPER(*(buf-1));
                    //uppercasenext = FALSE;
                //}
            }

            buffer += "r\n\0";

            if (to.Descriptor != null)
                WriteToOutput(to.Descriptor, buffer);

            //if ((to.IsNPC && scriptActTrigger > 0) && (to != character))
              //act_mtrigger(to, lbuf, ch, dg_victim, obj, dg_target, dg_arg);
        }
    }
}
