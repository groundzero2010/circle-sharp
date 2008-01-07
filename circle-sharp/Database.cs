using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
    public partial class SharpCore
    {
		private Dictionary<int, RoomData> _rooms = new Dictionary<int, RoomData> ();
		private int _topOfRoomTable = 0;
		private static int _loadRoomNumber = 0, _loadRoomZoneNumber = 0;

		private Dictionary<int, ZoneData> _zones = new Dictionary<int, ZoneData> ();
		private int _topOfZoneTable = 0;
		private static int _loadZoneNumber = 0;
		
        private List<CharacterData> _characters = new List<CharacterData>();
        private List<CharacterData> _combatters = new List<CharacterData>();

        private DateTime _bootTime;
        private bool _autoPWipe = false;
        private bool _noSpecials = false;
        private bool _scanFile = false;
        private bool _noMail = false;
        private bool _noRentCheck = false;
        private bool _miniMud = false;

        private TimeInfoData _timeInfo;
        private WeatherData _weatherInfo = new WeatherData ();

		private string _baseDirectory = String.Empty;

        private void BootDatabase()
        {
            GlobalUtilities.Log("Boot db -- BEGIN.");

            GlobalUtilities.Log("Resetting the game time.");
            ResetTime();

            GlobalUtilities.Log("Reading news, credits, help, background, info & motds.");
            // Load all the files that give us data.

            GlobalUtilities.Log("Loading spell defenitions.");
            //MagicAssignSpells();

            BootWorld();

            GlobalUtilities.Log("Loading help entries.");
            IndexBoot(GlobalConstants.DB_BOOT_HELP);

            GlobalUtilities.Log("Generating player index.");
            //BuildPlayerIndex();

            if (_autoPWipe)
            {
                GlobalUtilities.Log("Cleaning out the pfiles.");
                //CleanPFiles();
            }

            GlobalUtilities.Log("Loading fight messages.");
            //LoadMessages();

            GlobalUtilities.Log("Loading social messages.");
            //BootSocialMessages();

            if (!_noSpecials)
            {
                GlobalUtilities.Log("  Mobiles.");
                //AssignMobiles();
                GlobalUtilities.Log("  Shopkeepers.");
                //AssignShopkeepers();
                GlobalUtilities.Log("  Objects.");
                //AssignObjects();
                GlobalUtilities.Log("  Rooms.");
                //AssignRooms();
            }

            GlobalUtilities.Log("Assigning spell and skill levels.");
            //InitSpellLevels();

            GlobalUtilities.Log("Sorting command list and spells.");
            //SortCommands();
            //SortSpells();

            GlobalUtilities.Log("Booting mail system.");
            if (!_scanFile)
            {
                GlobalUtilities.Log("  Mail boot failed! Mail system disabled!");
                _noMail = true;
            }

            GlobalUtilities.Log("Reading banned site and invalid-name list.");
            //LoadBanned();
            //ReadInvalidList();

            if (!_noRentCheck)
            {
                GlobalUtilities.Log("Deleting timed-out crash and rent files.");
                //UpdateObjectFile();
                GlobalUtilities.Log("  Done.");
            }

            if (!_miniMud)
            {
                GlobalUtilities.Log("Booting houses.");
                //HouseBoot();
            }

            // Reset all zones.

            // Reset Queue setup

            _bootTime = DateTime.Now;

            GlobalUtilities.Log("Boot db -- DONE.");
        }

        private void ResetTime()
        {
            DateTime beginningOfTime = DateTime.MinValue;

            // FIXME: Load game time from file. or something.

            if (beginningOfTime == DateTime.MinValue)
                beginningOfTime = DateTime.Now;

            _timeInfo = MudTimePassed(DateTime.Now, beginningOfTime);

            if (_timeInfo.Hours <= 4)
                _weatherInfo.Sun = SunState.Dark;
            else if (_timeInfo.Hours == 5)
                _weatherInfo.Sun = SunState.Rise;
            else if (_timeInfo.Hours <= 20)
                _weatherInfo.Sun = SunState.Light;
            else if (_timeInfo.Hours == 21)
                _weatherInfo.Sun = SunState.Set;
            else
                _weatherInfo.Sun = SunState.Dark;

            GlobalUtilities.Log("  Current Game Time: " + _timeInfo.Hours + "H " + _timeInfo.Day + "D " + _timeInfo.Month + "M " + _timeInfo.Year + "Y.");

            _weatherInfo.Pressure = 960;

            if ((_timeInfo.Month >= 7) && (_timeInfo.Month <= 12))
                _weatherInfo.Pressure += GlobalUtilities.Dice(1, 50);
            else
                _weatherInfo.Pressure += GlobalUtilities.Dice(1, 80);

            _weatherInfo.Change = 0;

            if (_weatherInfo.Pressure <= 980)
                _weatherInfo.Sky = SkyState.Lightning;
            else if (_weatherInfo.Pressure <= 1000)
                _weatherInfo.Sky = SkyState.Raining;
            else if (_weatherInfo.Pressure <= 1020)
                _weatherInfo.Sky = SkyState.Cloudy;
            else
                _weatherInfo.Sky = SkyState.Cloudless;
        }

        private TimeInfoData MudTimePassed(DateTime t2, DateTime t1)
        {
            TimeSpan span = t2 - t1;
            long seconds = span.Seconds + (span.Minutes * 60) + (span.Hours * 3600) + (span.Days * 3600 * 24);

            long mudhours = (seconds / GlobalConstants.SECS_PER_MUD_HOUR) % 24;
            long muddays = (seconds / GlobalConstants.SECS_PER_MUD_DAY) % 35;
            long mudmonths = (seconds / GlobalConstants.SECS_PER_MUD_MONTH) % 17;
            long mudyears = (seconds / GlobalConstants.SECS_PER_MUD_YEAR);

            TimeInfoData now = new TimeInfoData(mudhours, muddays, mudmonths, mudyears);

            return (now);
        }

        private void BootWorld()
        {
			_baseDirectory = Path.Combine (Environment.CurrentDirectory, GlobalConstants.LIB_DIR);

			GlobalUtilities.Log ("Using library directory: " + _baseDirectory);

            GlobalUtilities.Log("Loading zone table.");
            IndexBoot(GlobalConstants.DB_BOOT_ZONE);

            GlobalUtilities.Log("Loading rooms.");
            IndexBoot(GlobalConstants.DB_BOOT_ROOM);

            GlobalUtilities.Log("Renumbering rooms.");
            RenumWorld();

            GlobalUtilities.Log("Checking start rooms.");
            CheckStartRooms();

            GlobalUtilities.Log("Loading mobiles and generating index.");
            IndexBoot(GlobalConstants.DB_BOOT_MOBILE);

            GlobalUtilities.Log("Loading objects and generating index.");
            IndexBoot(GlobalConstants.DB_BOOT_OBJECT);

            GlobalUtilities.Log("Renumbering zone table.");
            RenumZoneTable();

            if (!_noSpecials)
            {
                GlobalUtilities.Log("Loading shops.");
                IndexBoot(GlobalConstants.DB_BOOT_SHOP);
            }
        }

        private bool IndexBoot(int mode)
        {
            string indexFilename = String.Empty;
            string prefix = String.Empty;

            switch (mode)
            {
                case GlobalConstants.DB_BOOT_ROOM:
					prefix = Path.Combine(GlobalConstants.LIB_WORLD, GlobalConstants.ROOM_PREFIX);
                    break;

                case GlobalConstants.DB_BOOT_MOBILE:
					prefix = Path.Combine (GlobalConstants.LIB_WORLD, GlobalConstants.MOBILE_PREFIX);
                    break;
                
                case GlobalConstants.DB_BOOT_OBJECT:
					prefix = Path.Combine (GlobalConstants.LIB_WORLD, GlobalConstants.OBJECT_PREFIX);
                    break;

                case GlobalConstants.DB_BOOT_ZONE:
					prefix = Path.Combine (GlobalConstants.LIB_WORLD, GlobalConstants.ZONE_PREFIX);
                    break;

                case GlobalConstants.DB_BOOT_SHOP:
					prefix = Path.Combine (GlobalConstants.LIB_WORLD, GlobalConstants.SHOP_PREFIX);
                    break;

                case GlobalConstants.DB_BOOT_HELP:
					prefix = Path.Combine (GlobalConstants.LIB_WORLD, GlobalConstants.HELP_PREFIX);
                    break;

                case GlobalConstants.DB_BOOT_TRIGGER:
					prefix = Path.Combine (GlobalConstants.LIB_WORLD, GlobalConstants.TRIGGER_PREFIX);
                    break;

                default:
                    GlobalUtilities.Log("SYSERR: Unknown subcommand "+mode+" to IndexBoot!");
                    throw new Exception();
            }

			prefix = Path.Combine (_baseDirectory, prefix);

            if (_miniMud)
                indexFilename = GlobalConstants.MINDEX_FILE;
            else
                indexFilename = GlobalConstants.INDEX_FILE;

			FileStream indexFile = null;

			try
			{
				indexFile = File.OpenRead (Path.Combine(prefix, indexFilename));
			}
			catch
			{
				GlobalUtilities.Log ("SYSERR: Unable to open index file: " + Path.Combine (prefix, indexFilename));
				return false;
			}

			TextReader indexReader = new StreamReader (indexFile);
			string line = String.Empty;

			line = indexReader.ReadLine();

			while (line != "$")
			{
				string filename = line;
				XmlDocument xmlFile = null;

				try
				{
					Console.WriteLine ("Opening xml file: " + Path.Combine (prefix, filename));
					xmlFile = new XmlDocument ();
					xmlFile.Load(Path.Combine(prefix, filename));
				}
				catch (Exception e)
				{
					GlobalUtilities.Log ("SYSERR: Unable to open database file mentioned in index: " + filename);
					GlobalUtilities.Log("SYSERR: Exception: " + e.Message);
					return false;
				}

				switch (mode)
				{
					case GlobalConstants.DB_BOOT_ROOM:
						LoadRooms (xmlFile, filename);
						break;

					case GlobalConstants.DB_BOOT_OBJECT:
						//LoadObjects (xmlFile, filename);
						break;

					case GlobalConstants.DB_BOOT_MOBILE:
						//LoadMobiles( xmlFile, filename);
						break;

					case GlobalConstants.DB_BOOT_TRIGGER:
						//LoadTriggers (xmlFile, filename);
						break;

					case GlobalConstants.DB_BOOT_ZONE:
						LoadZone (xmlFile, filename);
						break;

					case GlobalConstants.DB_BOOT_SHOP:
						//LoadShop (dbFile, filename);
						break;

					case GlobalConstants.DB_BOOT_HELP:
						//LoadHelp (dbFile, filename);
						break;
				}
				
				line = indexReader.ReadLine ();
			}
			
			indexReader.Close();
			indexFile.Close();

			return true;
        }

		private bool LoadZone (XmlDocument file, string filename)
		{
			// Note: There should only be one zone per file.

			XmlNodeList list = file.GetElementsByTagName("ZoneData");

			foreach (XmlNode node in list)
			{
				ZoneData zone = new ZoneData();

				try
				{
					int commandCount = 0;

					zone.Number = Int32.Parse(node.Attributes["Number"].Value);

					foreach (XmlNode child in node.ChildNodes)
					{
						switch (child.Name)
						{
							case "Name":
								zone.Name = child.InnerText;
								break;

							case "Bottom":
								zone.Bottom = Int32.Parse(child.InnerText);
								break;

							case "Top":
								zone.Top = Int32.Parse(child.InnerText);
								break;

							case "Lifespan":
								zone.Lifespan = Int32.Parse(child.InnerText);
								break;

							case "ResetMode":
								zone.ResetMode = Int32.Parse(child.InnerText);
								break;

							case "Command":
								ResetCommand command = new ResetCommand();
								command.Command = child.Attributes["Type"].Value[0];
								command.Argument1 = Int32.Parse(child["Arg1"].InnerText);
								command.Argument2 = Int32.Parse(child["Arg2"].InnerText);
								command.Argument3 = Int32.Parse(child["Arg3"].InnerText);
								//command.Argument4 = Int32.Parse(child["Arg4"].Value);
								zone.Commands.Add(commandCount++, command);
								break;
						}
					}
				}
				catch
				{
					GlobalUtilities.Log("SYSERR: Format error in XML for zone in file: " + filename);
					return false;
				}

				_zones.Add(_topOfZoneTable++, zone);
			}

			return true;
		}

		private bool LoadRooms(XmlDocument file, string filename)
		{
			XmlNodeList list = file.GetElementsByTagName("RoomData");

			foreach (XmlNode node in list)
			{
				RoomData room = new RoomData();

				try
				{
					room.Number = Int32.Parse(node.Attributes["Number"].Value);

					foreach (XmlNode child in node.ChildNodes)
					{
						switch (child.Name)
						{
							case "Name":
								room.Name = child.InnerText;
								break;

							case "Description":
								room.Description = child.InnerText;
								break;

							case "Zone":
								room.Zone = Int32.Parse(child.InnerText);
								break;

							case "Flags":
								room.Flags = (RoomFlags)long.Parse(child.InnerText);
								break;

							case "SectorType":
								room.SectorType = (SectorTypes)int.Parse(child.InnerText);
								break;

							case "Direction":
								int direction = Int32.Parse(child.Attributes["Direction"].Value);

								if (room.DirectionOptions[direction] != null)
								{
									GlobalUtilities.Log("SYSERR: Direction [" + direction + "] already defined in XML file: " + filename);
									continue;
								}

								room.DirectionOptions[direction] = new RoomDirectionData();
								room.DirectionOptions[direction].Description = child["Description"].InnerText;
								room.DirectionOptions[direction].Keyword = child["Keyword"].InnerText;
								room.DirectionOptions[direction].Key = Int32.Parse(child["Key"].InnerText);
								room.DirectionOptions[direction].ToRoom = Int32.Parse(child["ToRoom"].InnerText);

								int exitinfo = Convert.ToInt32(child["Flags"].InnerText);

								if (exitinfo == 1)
									room.DirectionOptions[direction].ExitInfo = DirectionOptionFlags.IsDoor;
								else if (exitinfo == 2)
									room.DirectionOptions[direction].ExitInfo = DirectionOptionFlags.IsDoor | DirectionOptionFlags.PickProof;
								else
									room.DirectionOptions[direction].ExitInfo = DirectionOptionFlags.None;
								break;

							case "ExtraDescription":
								ExtraDescriptionData description = new ExtraDescriptionData();
								description.Keyword = child["Keyword"].InnerText;
								description.Description = child["Description"].InnerText;
								room.ExtraDescriptions.Add(description);
								break;

							case "Trigger":
								// TODO: Add trigger stuff here.
								break;
						}
					}
				}
				catch
				{
					GlobalUtilities.Log("SYSERR: Format error in XML for room in file: " + filename);
					return false;
				}

				_rooms.Add(_topOfRoomTable++, room);
			}

			return true;
		}

        private void CheckStartRooms()
        {

        }

        private void RenumWorld()
        {

        }

        private void RenumZoneTable()
        {

        }
    }
}
