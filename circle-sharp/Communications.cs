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

        private int _maxPlayers = 0;

        private void InitializeGame()
        {
            _running = true;

            //LogUtility.Info("Finding player limit.");
            _maxPlayers = GetMaxPlayers();

            BootDatabase();

            //LogUtility.Info ("Entering game loop.");
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
            TcpListener listener = new TcpListener(IPAddress.Loopback, 5000);
            listener.Start();

            // Initialize the time values
            lastTime = DateTime.Now;

            while (_running)
            {
                if (_descriptors.Count < 1)
                {
                    Console.WriteLine("No connections. Going to sleep.");

                    _newClients.Enqueue(listener.AcceptTcpClient());

                    Console.WriteLine("New connection. Waking up.");

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

                if (listener.Pending())
                    _newClients.Enqueue(listener.AcceptTcpClient());

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

                // execute heartbeat

                // Check for signals
            }

            listener.Stop();
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

            WriteToOutput(descriptor, GlobalSettings.GreetingText);
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

        private string MakePrompt(DescriptorData descriptor)
        {
            return ">";
        }
    }
}
