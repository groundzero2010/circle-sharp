using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
	public partial class CircleCore
	{
		private Dictionary<int, HelpData> _help = new Dictionary<int, HelpData> ();
		private int _topOfHelpTable = 0;

		private Dictionary<int, RoomData> _rooms = new Dictionary<int, RoomData> ();
		private int _topOfRoomTable = 0;

		private Dictionary<int, ShopData> _shops = new Dictionary<int, ShopData> ();
		private int _topOfShopTable = 0;

		private Dictionary<int, ZoneData> _zones = new Dictionary<int, ZoneData> ();
		private int _topOfZoneTable = 0;

		private Dictionary<int, ObjectData> _objects = new Dictionary<int, ObjectData> ();
		private Dictionary<int, IndexData> _objectIndex = new Dictionary<int, IndexData> ();
		private int _topOfObjectTable = 0;

		private Dictionary<int, CharacterData> _mobiles = new Dictionary<int, CharacterData> ();
		private Dictionary<int, IndexData> _mobileIndex = new Dictionary<int, IndexData> ();
		private int _topOfMobileTable = 0;

		private Dictionary<int, TriggerData> _triggers = new Dictionary<int, TriggerData> ();
		private Dictionary<int, IndexData> _triggerIndex = new Dictionary<int, IndexData> ();
		private int _topOfTriggerTable = 0;

		private List<CharacterData> _characters = new List<CharacterData> ();
		private List<CharacterData> _combatters = new List<CharacterData> ();

		private DateTime _bootTime;
		private bool _noSpecials = false;
		private bool _noMail = false;
		private bool _noRentCheck = false;
		private bool _miniMud = false;

		private int _realMortalStartRoom = GlobalConstants.NOWHERE;
		private int _realImmortalStartRoom = GlobalConstants.NOWHERE;
		private int _realFrozenStartRoom = GlobalConstants.NOWHERE;

		private TimeInfoData _timeInfo;
		private WeatherData _weatherInfo = new WeatherData ();

		private string _textGreetings;
		private string _textCredits;
		private string _textMOTD;
		private string _textIMOTD;
		private string _textHelp;
		private string _textInfo;
		private string _textWizList;
		private string _textImmList;
		private string _textPolicies;
		private string _textHandbook;
		private string _textBackground;
		private string _textMenu;

		private void BootDatabase ()
		{
			Log ("Boot db -- BEGIN.");

			Log ("Resetting the game time.");
			ResetTime ();

			Log ("Reading news, credits, help, background, info & motds.");
			LoadTextFiles ();

			Log ("Loading spell definitions.");
			MagicAssignSpells ();

			BootWorld ();

			Log ("Loading help entries.");
			IndexBoot (GlobalConstants.DB_BOOT_HELP);

			Log ("Generating player index.");
			BuildPlayerIndex ();

			if (GlobalSettings.AutoPlayerWipe)
			{
				Log ("Cleaning up the player index.");
				CleanPlayerIndex ();
			}

			Log ("Loading fight messages.");
			LoadFightMessages ();

			Log ("Loading social messages.");
			BootSocialMessages ();

			Log ("Assigning function pointers:");

			if (!_noSpecials)
			{
				Log ("  Mobiles.");
				AssignMobiles ();
				Log ("  Shopkeepers.");
				AssignShopkeepers ();
				Log ("  Objects.");
				AssignObjects ();
				Log ("  Rooms.");
				AssignRooms ();
			}

			Log ("Assigning spell and skill levels.");
			InitializeSpellLevels ();

			Log ("Sorting command list and spells.");
			SortCommands ();
			//SortSpells ();

			Log ("Booting mail system.");
			if (!LoadMail())
			{
				Log ("  Mail boot failed! Mail system disabled!");
				_noMail = true;
			}

			Log ("Reading banned site and invalid-name list.");
			LoadBanned ();
			ReadInvalidList ();

			if (!_noRentCheck)
			{
				Log ("Deleting timed-out crash and rent files.");
				//UpdateObjectFile ();
				Log ("  Done.");
			}

			if (!_miniMud)
			{
				Log ("Booting houses.");
				//HouseBoot ();
			}

			// Reset all zones.

			// Reset Queue setup

			_bootTime = DateTime.Now;

			Log ("Boot db -- DONE.");
		}

		private void ResetTime ()
		{
			DateTime beginningOfTime = DateTime.MinValue;

			// FIXME: Load game time from file. or something.

			if (beginningOfTime == DateTime.MinValue)
				beginningOfTime = DateTime.Now;

			_timeInfo = MudTimePassed (DateTime.Now, beginningOfTime);

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

			Log ("  Current Game Time: " + _timeInfo.Hours + "H " + _timeInfo.Day + "D " + _timeInfo.Month + "M " + _timeInfo.Year + "Y.");

			_weatherInfo.Pressure = 960;

			if ((_timeInfo.Month >= 7) && (_timeInfo.Month <= 12))
				_weatherInfo.Pressure += GlobalUtilities.Dice (1, 50);
			else
				_weatherInfo.Pressure += GlobalUtilities.Dice (1, 80);

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

		private string LoadText (string filename)
		{
			XmlDocument file = new XmlDocument ();
			file.Load (filename);

			try
			{
				XmlNodeList list = file.GetElementsByTagName ("TextData");

				foreach (XmlNode node in list)
				{
					return (node.InnerText);
				}
			}
			catch (Exception e)
			{
				Log ("SYSERR: Format error in XML for text file: " + filename);
				Log ("Exception: " + e.Message);

				return String.Empty;
			}

			return String.Empty;
		}

		private void LoadTextFiles ()
		{
			string path = Path.Combine (_baseDirectory, GlobalConstants.LIB_TEXT);

			_textGreetings = LoadText (Path.Combine (path, "Greetings.xml"));
			_textCredits = LoadText (Path.Combine (path, "Credits.xml"));
			_textMOTD = LoadText (Path.Combine (path, "MOTD.xml"));
			_textIMOTD = LoadText (Path.Combine (path, "IMOTD.xml"));
			_textHelp = LoadText (Path.Combine (path, "Help.xml"));
			_textInfo = LoadText (Path.Combine (path, "Info.xml"));
			_textWizList = LoadText (Path.Combine (path, "Wizlist.xml"));
			_textImmList = LoadText (Path.Combine (path, "Immlist.xml"));
			_textPolicies = LoadText (Path.Combine (path, "Policies.xml"));
			_textHandbook = LoadText (Path.Combine (path, "Handbook.xml"));
			_textBackground = LoadText (Path.Combine (path, "Background.xml"));
			_textMenu = LoadText (Path.Combine (path, "Menu.xml"));
		}

		private TimeInfoData MudTimePassed (DateTime t2, DateTime t1)
		{
			TimeSpan span = t2 - t1;
			long seconds = span.Seconds + (span.Minutes * 60) + (span.Hours * 3600) + (span.Days * 3600 * 24);

			long mudhours = (seconds / GlobalConstants.SECS_PER_MUD_HOUR) % 24;
			long muddays = (seconds / GlobalConstants.SECS_PER_MUD_DAY) % 35;
			long mudmonths = (seconds / GlobalConstants.SECS_PER_MUD_MONTH) % 17;
			long mudyears = (seconds / GlobalConstants.SECS_PER_MUD_YEAR);

			TimeInfoData now = new TimeInfoData (mudhours, muddays, mudmonths, mudyears);

			return (now);
		}

		private void BootWorld ()
		{
			Log ("Using library directory: " + _baseDirectory);

			Log ("Loading zone table.");
			IndexBoot (GlobalConstants.DB_BOOT_ZONE);

			Log ("Loading triggers and generating index.");
			IndexBoot (GlobalConstants.DB_BOOT_TRIGGER);

			Log ("Loading rooms.");
			IndexBoot (GlobalConstants.DB_BOOT_ROOM);

			Log ("Renumbering rooms.");
			RenumWorld ();

			Log ("Checking start rooms.");
			CheckStartRooms ();

			Log ("Loading mobiles and generating index.");
			IndexBoot (GlobalConstants.DB_BOOT_MOBILE);

			Log ("Loading objects and generating index.");
			IndexBoot (GlobalConstants.DB_BOOT_OBJECT);

			Log ("Renumbering zone table.");
			RenumZoneTable ();

			if (!_noSpecials)
			{
				Log ("Loading shops.");
				IndexBoot (GlobalConstants.DB_BOOT_SHOP);
			}
		}

		private bool IndexBoot (int mode)
		{
			string indexFilename = String.Empty;
			string prefix = String.Empty;

			switch (mode)
			{
				case GlobalConstants.DB_BOOT_ROOM:
					prefix = Path.Combine (GlobalConstants.LIB_WORLD, GlobalConstants.ROOM_PREFIX);
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
					prefix = GlobalConstants.HELP_PREFIX;
					break;

				case GlobalConstants.DB_BOOT_TRIGGER:
					prefix = Path.Combine (GlobalConstants.LIB_WORLD, GlobalConstants.TRIGGER_PREFIX);
					break;

				default:
					Log ("SYSERR: Unknown subcommand " + mode + " to IndexBoot!");
					throw new Exception ();
			}

			prefix = Path.Combine (_baseDirectory, prefix);

			if (_miniMud)
				indexFilename = GlobalConstants.MINDEX_FILE;
			else
				indexFilename = GlobalConstants.INDEX_FILE;

			FileStream indexFile = null;

			try
			{
				indexFile = File.OpenRead (Path.Combine (prefix, indexFilename));
			}
			catch
			{
				Log ("SYSERR: Unable to open index file: " + Path.Combine (prefix, indexFilename));
				return false;
			}

			TextReader indexReader = new StreamReader (indexFile);
			string line = String.Empty;

			line = indexReader.ReadLine ();

			while (line != "$")
			{
				string filename = line;
				XmlDocument xmlFile = null;

				try
				{
					xmlFile = new XmlDocument ();
					xmlFile.Load (Path.Combine (prefix, filename));
				}
				catch (Exception e)
				{
					Log ("SYSERR: Unable to open database file mentioned in index: " + filename);
					Log ("SYSERR: Exception: " + e.Message);
					return false;
				}

				switch (mode)
				{
					case GlobalConstants.DB_BOOT_ROOM:
						LoadRooms (xmlFile, filename);
						break;

					case GlobalConstants.DB_BOOT_OBJECT:
						LoadObjects (xmlFile, filename);
						break;

					case GlobalConstants.DB_BOOT_MOBILE:
						LoadMobiles (xmlFile, filename);
						break;

					case GlobalConstants.DB_BOOT_TRIGGER:
						LoadTriggers (xmlFile, filename);
						break;

					case GlobalConstants.DB_BOOT_ZONE:
						LoadZone (xmlFile, filename);
						break;

					case GlobalConstants.DB_BOOT_SHOP:
						LoadShops (xmlFile, filename);
						break;

					case GlobalConstants.DB_BOOT_HELP:
						LoadHelp (xmlFile, filename);
						break;
				}

				line = indexReader.ReadLine ();
			}

			indexReader.Close ();
			indexFile.Close ();

			switch (mode)
			{
				case GlobalConstants.DB_BOOT_ROOM:
					Log ("  " + _rooms.Count + " room record(s) loaded.");
					break;

				case GlobalConstants.DB_BOOT_MOBILE:
					Log ("  " + _mobileIndex.Count + " mobile record(s) loaded.");
					break;

				case GlobalConstants.DB_BOOT_OBJECT:
					Log ("  " + _objectIndex.Count + " object record(s) loaded.");
					break;

				case GlobalConstants.DB_BOOT_ZONE:
					Log ("  " + _zones.Count + " zone record(s) loaded.");
					break;

				case GlobalConstants.DB_BOOT_SHOP:
					Log ("  " + _rooms.Count + " shop record(s) loaded.");
					break;

				case GlobalConstants.DB_BOOT_HELP:
					Log ("  " + _help.Count + " help record(s) loaded.");
					break;

				case GlobalConstants.DB_BOOT_TRIGGER:
					Log ("  " + _triggerIndex.Count + " trigger record(s) loaded.");
					break;
			}

			return true;
		}

		private bool LoadZone (XmlDocument file, string filename)
		{
			// Note: There should only be one zone per file.

			XmlNodeList list = file.GetElementsByTagName ("ZoneData");

			foreach (XmlNode node in list)
			{
				ZoneData zone = new ZoneData ();
				zone.Number = -1;

				try
				{
					int commandCount = 0;

					zone.Number = Int32.Parse (node.Attributes["Number"].Value);

					foreach (XmlNode child in node.ChildNodes)
					{
						switch (child.Name)
						{
							case "Name":
								zone.Name = child.InnerText;
								break;

							case "Bottom":
								zone.Bottom = Int32.Parse (child.InnerText);
								break;

							case "Top":
								zone.Top = Int32.Parse (child.InnerText);
								break;

							case "Lifespan":
								zone.Lifespan = Int32.Parse (child.InnerText);
								break;

							case "ResetMode":
								zone.ResetMode = Int32.Parse (child.InnerText);
								break;

							case "Command":
								ResetCommand command = new ResetCommand ();
								command.Command = child.Attributes["Type"].Value[0];
								command.Argument1 = Int32.Parse (child["Arg1"].InnerText);
								command.Argument2 = Int32.Parse (child["Arg2"].InnerText);
								command.Argument3 = Int32.Parse (child["Arg3"].InnerText);
								//command.Argument4 = Int32.Parse(child["Arg4"].Value);
								zone.Commands.Add (commandCount++, command);
								break;
						}
					}
				}
				catch
				{
					Log ("SYSERR: Error pasing XML for zone [" + zone.Number + "] in file: " + filename);
					return false;
				}

				_zones.Add (_topOfZoneTable++, zone);
			}

			return true;
		}

		private bool LoadShops (XmlDocument file, string filename)
		{
			XmlNodeList list = file.GetElementsByTagName ("ShopData");

			foreach (XmlNode node in list)
			{
				ShopData shop = new ShopData ();

				try
				{
					shop.Number = Int32.Parse (node.Attributes["Number"].Value);

					foreach (XmlNode child in node.ChildNodes)
					{
						switch (child.Name)
						{
							case "Product":
								shop.Producing.Add (Int32.Parse (child.InnerText));
								break;

							case "ProfitBuy":
								shop.ProfitBuy = float.Parse (child.InnerText);
								break;

							case "ProfitSell":
								shop.ProfitSell = float.Parse (child.InnerText);
								break;

							case "Type":
								ShopBuyData data = new ShopBuyData ();

								shop.Type.Add (data);
								break;

							case "NoSuchItem1":
								shop.NoSuchItem1 = child.InnerText;
								break;

							case "NoSuchItem2":
								shop.NoSuchItem2 = child.InnerText;
								break;

							case "DoNotBuy":
								shop.DoNotBuy = child.InnerText;
								break;

							case "MissingCash1":
								shop.MissingCash1 = child.InnerText;
								break;

							case "MissingCash2":
								shop.MissingCash2 = child.InnerText;
								break;

							case "MessageBuy":
								shop.MessageBuy = child.InnerText;
								break;

							case "MessageSell":
								shop.MessageSell = child.InnerText;
								break;

							case "Temper":
								shop.Temper = Int32.Parse (child.InnerText);
								break;

							case "Bitvector":
								shop.Bitvector = long.Parse (child.InnerText);
								break;

							case "Keeper":
								shop.Keeper = Int32.Parse (child.InnerText);
								break;

							case "WithWho":
								shop.WithWho = Int32.Parse (child.InnerText);
								break;

							case "InRoom":
								shop.InRoom.Add (Int32.Parse (child.InnerText));
								break;

							case "Open1":
								shop.Open1 = Int32.Parse (child.InnerText);
								break;

							case "Close1":
								shop.Close1 = Int32.Parse (child.InnerText);
								break;

							case "Open2":
								shop.Open2 = Int32.Parse (child.InnerText);
								break;

							case "Close2":
								shop.Close2 = Int32.Parse (child.InnerText);
								break;
						}
					}
				}
				catch
				{
					Log ("SYSERR: Error pasing XML for shop [" + shop.Number + "] in file: " + filename);
					return false;
				}

				_shops.Add (_topOfShopTable++, shop);
			}

			return true;
		}

		private bool LoadRooms (XmlDocument file, string filename)
		{
			XmlNodeList list = file.GetElementsByTagName ("RoomData");

			foreach (XmlNode node in list)
			{
				RoomData room = new RoomData ();
				room.Number = -1;

				try
				{
					room.Number = Int32.Parse (node.Attributes["Number"].Value);

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
								room.Zone = Int32.Parse (child.InnerText);
								break;

							case "Flags":
								room.Flags = long.Parse (child.InnerText);
								break;

							case "SectorType":
								room.SectorType = (SectorTypes)int.Parse (child.InnerText);
								break;

							case "Direction":
								int direction = Int32.Parse (child.Attributes["Direction"].Value);

								if (room.DirectionOptions[direction] != null)
								{
									Log ("SYSERR: Direction [" + direction + "] already defined in XML file: " + filename);
									continue;
								}

								room.DirectionOptions[direction] = new RoomDirectionData ();
								room.DirectionOptions[direction].Description = child["Description"].InnerText;
								room.DirectionOptions[direction].Keyword = child["Keyword"].InnerText;
								room.DirectionOptions[direction].Key = Int32.Parse (child["Key"].InnerText);
								room.DirectionOptions[direction].ToRoom = Int32.Parse (child["ToRoom"].InnerText);

								int exitinfo = Convert.ToInt32 (child["Flags"].InnerText);

								if (exitinfo == 1)
									room.DirectionOptions[direction].ExitInfo = (long)DirectionOptionFlags.IsDoor;
								else if (exitinfo == 2)
									room.DirectionOptions[direction].ExitInfo = (long)DirectionOptionFlags.IsDoor | (long)DirectionOptionFlags.PickProof;
								else
									room.DirectionOptions[direction].ExitInfo = (long)DirectionOptionFlags.None;
								break;

							case "ExtraDescription":
								ExtraDescriptionData description = new ExtraDescriptionData ();
								description.Keyword = child["Keyword"].InnerText;
								description.Description = child["Description"].InnerText;
								room.ExtraDescriptions.Add (description);
								break;

							case "Trigger":
								// TODO: Add trigger stuff here.
								break;
						}
					}
				}
				catch
				{
					Log ("SYSERR: Error pasing XML for room [" + room.Number + "] in file: " + filename);
					return false;
				}

				_rooms.Add (_topOfRoomTable++, room);
			}

			return true;
		}

		private bool LoadObjects (XmlDocument file, string filename)
		{
			XmlNodeList list = file.GetElementsByTagName ("ObjectData");

			foreach (XmlNode node in list)
			{
				ObjectData obj = new ObjectData ();
				int virtualNumber = -1;

				try
				{
					virtualNumber = Int32.Parse (node.Attributes["Number"].Value);
					obj.Number = _topOfObjectTable;

					foreach (XmlNode child in node.ChildNodes)
					{
						switch (child.Name)
						{
							case "Name":
								obj.Name = child.InnerText;
								break;

							case "ShortDescription":
								obj.ShortDescription = child.InnerText;
								break;

							case "Description":
								obj.Description = child.InnerText;
								break;

							case "ActionDescription":
								obj.ActionDescription = child.InnerText;
								break;

							case "ObjectType":
								obj.Type = (ObjectTypes)int.Parse (child.InnerText);
								break;

							case "ObjectFlags":
								obj.Flags.ObjectFlags = long.Parse (child.InnerText);
								break;

							case "WearFlags":
								obj.Flags.WearFlags = long.Parse (child.InnerText);
								break;

							case "Bitvector":
								obj.Flags.Bitvector = long.Parse (child.InnerText);
								break;

							case "Values":
								obj.Flags.Values[0] = Int32.Parse (child.Attributes["One"].Value);
								obj.Flags.Values[1] = Int32.Parse (child.Attributes["Two"].Value);
								obj.Flags.Values[2] = Int32.Parse (child.Attributes["Three"].Value);
								obj.Flags.Values[3] = Int32.Parse (child.Attributes["Four"].Value);
								break;

							case "Weight":
								obj.Flags.Weight = Int32.Parse (child.InnerText);
								break;

							case "Cost":
								obj.Flags.Cost = Int32.Parse (child.InnerText);
								break;

							case "CostPerDay":
								obj.Flags.CostPerDay = Int32.Parse (child.InnerText);
								break;

							case "MinimumLevel":
								obj.Flags.MinimumLevel = Int32.Parse (child.InnerText);
								break;

							case "Affect":
								ObjectAffectData affect = new ObjectAffectData ();
								affect.Location = (ApplyTypes)Int32.Parse (child["Location"].InnerText);
								affect.Modifier = Int32.Parse (child["Modifier"].InnerText);
								obj.Affects.Add (affect);
								break;

							case "ExtraDescription":
								ExtraDescriptionData description = new ExtraDescriptionData ();
								description.Keyword = child["Keyword"].InnerText;
								description.Description = child["Description"].InnerText;
								obj.ExtraDescriptions.Add (description);
								break;

							case "Trigger":
								// TODO: Add trigger stuff here.
								break;
						}
					}
				}
				catch
				{
					Log ("SYSERR: Error pasing XML for object [" + virtualNumber + "] in file: " + filename);
					return false;
				}

				IndexData index = new IndexData ();
				index.VirtualNumber = virtualNumber;
				index.Count = 0;
				_objectIndex.Add (_topOfObjectTable, index);

				_objects.Add (_topOfObjectTable++, obj);
			}

			return true;
		}

		private bool LoadMobiles (XmlDocument file, string filename)
		{
			XmlNodeList list = file.GetElementsByTagName ("MobileData");

			foreach (XmlNode node in list)
			{
				CharacterData mobile = new CharacterData ();
				int virtualNumber = -1;

				try
				{
					virtualNumber = Int32.Parse (node.Attributes["Number"].Value);
					mobile.Number = _topOfMobileTable;

					foreach (XmlNode child in node.ChildNodes)
					{
						switch (child.Name)
						{
							case "Name":
								mobile.Player.Name = child.InnerText;
								break;

							case "ShortDescription":
								mobile.Player.ShortDescription = child.InnerText;
								break;

							case "LongDescription":
								mobile.Player.LongDescription = child.InnerText;
								break;

							case "Description":
								mobile.Player.Description = child.InnerText;
								break;

							case "MobileFlags":
								mobile.CharacterSpecials.Saved.Flags = long.Parse (child.InnerText);
								break;

							case "Alignment":
								mobile.CharacterSpecials.Saved.Alignment = int.Parse (child.InnerText);
								break;

							case "Level":
								mobile.Player.Level = int.Parse (child.InnerText);
								break;

							case "HitRoll":
								mobile.Points.HitRoll = int.Parse (child.InnerText);
								break;

							case "ArmorClass":
								mobile.Points.ArmorClass = int.Parse (child.InnerText);
								break;

							case "Hit":
								mobile.Points.Hit = int.Parse (child.InnerText);
								break;

							case "Mana":
								mobile.Points.Mana = int.Parse (child.InnerText);
								break;

							case "Move":
								mobile.Points.Move = int.Parse (child.InnerText);
								break;

							case "DamageNoDice":
								mobile.MobileSpecials.DamageNoDice = int.Parse (child.InnerText);
								break;

							case "DamageSizeDice":
								mobile.MobileSpecials.DamageSizeDice = int.Parse (child.InnerText);
								break;

							case "DamageRoll":
								mobile.Points.DamageRoll = int.Parse (child.InnerText);
								break;

							case "Gold":
								mobile.Points.Gold = int.Parse (child.InnerText);
								break;

							case "Experience":
								mobile.Points.Experience = int.Parse (child.InnerText);
								break;

							case "Position":
								mobile.CharacterSpecials.Position = (PositionTypes)int.Parse (child.InnerText);
								break;

							case "DefaultPosition":
								mobile.MobileSpecials.DefaultPosition = (PositionTypes)int.Parse (child.InnerText);
								break;

							case "Sex":
								mobile.Player.Sex = (SexTypes)int.Parse (child.InnerText);
								break;

							case "Class":
								mobile.Player.Class = (ClassTypes)int.Parse (child.InnerText);
								break;

							case "Height":
								mobile.Player.Height = int.Parse (child.InnerText);
								break;

							case "Weight":
								mobile.Player.Weight = int.Parse (child.InnerText);
								break;

							case "SavingThrows":
								break;

							case "BareHandAttack":
								mobile.MobileSpecials.AttackType = int.Parse (child.InnerText);
								break;

							case "Strength":
								mobile.RealAbilities.Strength = int.Parse (child.InnerText);
								break;

							case "StrengthAdd":
								mobile.RealAbilities.StrengthAdd = int.Parse (child.InnerText);
								break;

							case "Intelligence":
								mobile.RealAbilities.Intelligence = int.Parse (child.InnerText);
								break;

							case "Wisdom":
								mobile.RealAbilities.Wisdom = int.Parse (child.InnerText);
								break;

							case "Dexterity":
								mobile.RealAbilities.Dexterity = int.Parse (child.InnerText);
								break;

							case "Constitution":
								mobile.RealAbilities.Constitution = int.Parse (child.InnerText);
								break;

							case "Charisma":
								mobile.RealAbilities.Charisma = int.Parse (child.InnerText);
								break;
						}

					}
				}
				catch
				{
					Log ("SYSERR: Error pasing XML for mobile [" + virtualNumber + "] in file: " + filename);
					return false;
				}

				IndexData index = new IndexData ();
				index.VirtualNumber = virtualNumber;
				index.Count = 0;
				_mobileIndex.Add (_topOfMobileTable, index);

				_mobiles.Add (_topOfMobileTable++, mobile);
			}

			return true;
		}

		private bool LoadTriggers (XmlDocument file, string filename)
		{
			XmlNodeList list = file.GetElementsByTagName ("TriggerData");

			foreach (XmlNode node in list)
			{
				TriggerData trigger = new TriggerData ();
				int virtualNumber = -1;

				try
				{
					virtualNumber = Int32.Parse (node.Attributes["Number"].Value);
					trigger.Number = _topOfMobileTable;

					foreach (XmlNode child in node.ChildNodes)
					{
						switch (child.Name)
						{
							case "Name":
								trigger.Name = child.InnerText;
								break;

							case "AttachType":
								trigger.AttachType = byte.Parse (child.InnerText);
								break;

							case "TriggerType":
								trigger.Type = long.Parse (child.InnerText);
								break;

							case "NumericalArgument":
								trigger.NumericalArgument = Int32.Parse (child.InnerText);
								break;

							case "ArgumentList":
								trigger.ArgumentList = child.InnerText;
								break;

							case "Command":
								CommandListElement command = new CommandListElement ();
								command.Command = child.InnerText;
								trigger.Commands.Add (command);
								break;
						}
					}
				}
				catch
				{
					Log ("SYSERR: Error pasing XML for trigger [" + virtualNumber + "] in file: " + filename);
					return false;
				}

				IndexData index = new IndexData ();
				index.VirtualNumber = virtualNumber;
				index.Count = 0;
				index.Trigger = trigger;
				_triggerIndex.Add (_topOfMobileTable, index);
			}

			return true;
		}

		private bool LoadHelp (XmlDocument file, string filename)
		{
			XmlNodeList list = file.GetElementsByTagName ("HelpData");

			foreach (XmlNode node in list)
			{
				HelpData help = new HelpData ();
				help.Keyword = "UNKNOWN";

				try
				{
					help.Keyword = node.Attributes["Keyword"].Value;
					help.Entry = node.InnerText;
				}
				catch
				{
					Log ("SYSERR: Error pasing XML for help [" + help.Keyword + "] in file: " + filename);
					return false;
				}

				_help.Add (_topOfHelpTable++, help);
			}

			return true;
		}

		private bool CheckStartRooms ()
		{
			if ((_realMortalStartRoom = RealRoom (GlobalSettings.MortalStartRoom)) == GlobalConstants.NOWHERE)
			{
				Log ("SYSERR: Mortal start room does not exist! Change in GlobalSettings.cs.");
				return false;
			}

			if ((_realImmortalStartRoom = RealRoom (GlobalSettings.ImmortalStartRoom)) == GlobalConstants.NOWHERE)
			{
				if (!_miniMud)
					Log ("SYSERR: Immortal start room does not exist! Change in GlobalSettings.cs.");

				_realImmortalStartRoom = _realMortalStartRoom;
			}

			if ((_realFrozenStartRoom = RealRoom (GlobalSettings.FrozenStartRoom)) == GlobalConstants.NOWHERE)
			{
				if (!_miniMud)
					Log ("SYSERR: Frozen start room does not exist! Change in GlobalSettings.cs.");

				_realFrozenStartRoom = _realMortalStartRoom;
			}

			return true;
		}

		private void RenumWorld ()
		{
			for (int room = 0; room < _topOfRoomTable; room++)
				for (int door = 0; door < (int)DirectionTypes.Index; door++)
					if (_rooms[room].DirectionOptions[door] != null)
						if (_rooms[room].DirectionOptions[door].ToRoom != GlobalConstants.NOWHERE)
							_rooms[room].DirectionOptions[door].ToRoom = RealRoom (_rooms[room].DirectionOptions[door].ToRoom, _rooms[room].Number);
		}

		private void RenumZoneTable ()
		{
			int a, b, c;
			int olda, oldb, oldc;

			for (int zone = 0; zone < _topOfZoneTable; zone++)
			{
				foreach (ResetCommand command in _zones[zone].Commands.Values)
				{
					a = b = c = 0;

					olda = command.Argument1;
					oldb = command.Argument2;
					oldc = command.Argument3;

					switch (command.Command)
					{
						case 'M':
							a = command.Argument1 = RealMobile (command.Argument1);
							c = command.Argument3 = RealRoom (command.Argument3);
							break;

						case 'O':
							a = command.Argument1 = RealObject (command.Argument1);
							if (command.Argument3 != GlobalConstants.NOWHERE)
								c = command.Argument3 = RealRoom (command.Argument3);
							break;

						case 'G':
							a = command.Argument1 = RealObject (command.Argument1);
							break;

						case 'E':
							a = command.Argument1 = RealObject (command.Argument1);
							break;

						case 'P':
							a = command.Argument1 = RealObject (command.Argument1);
							c = command.Argument3 = RealObject (command.Argument3);
							break;

						case 'D':
							a = command.Argument1 = RealRoom (command.Argument1);
							break;

						case 'R':
							a = command.Argument1 = RealRoom (command.Argument1);
							b = command.Argument2 = RealObject (command.Argument2);
							break;

						case 'T':
							b = command.Argument2 = RealTrigger (command.Argument2);
							c = command.Argument3 = RealRoom (command.Argument3);
							break;

						case 'V':
							b = command.Argument3 = RealRoom (command.Argument3);
							break;
					}

					if (a == GlobalConstants.NOWHERE || b == GlobalConstants.NOWHERE || c == GlobalConstants.NOWHERE)
					{
						if (!_miniMud)
						{
							Log ("Invalid virtual #" + (a == GlobalConstants.NOWHERE ? olda : b == GlobalConstants.NOWHERE ? oldb : oldc) + ", command disabled.");
							//log_zone_error (zone, cmd_no, buf);
						}

						command.Command = '*';
					}
				}
			}
		}

		private int RealRoom (int virtualNumber, int reference)
		{
			int room = RealRoom (virtualNumber);

			if (room == GlobalConstants.NOWHERE)
				Log ("Room #" + virtualNumber + " does not exist in database! (References in room #" + reference + ")");

			return room;
		}

		private int RealRoom (int virtualNumber)
		{
			// TODO: This could be binary search to make it faster.
			for (int room = 0; room < _topOfRoomTable; room++)
				if (_rooms[room].Number == virtualNumber)
					return room;

			return GlobalConstants.NOWHERE;
		}

		private int RealObject (int virtualNumber)
		{
			// TODO: This could be binary search to make it faster.
			for (int obj = 0; obj < _topOfObjectTable; obj++)
				if (_objects[obj].Number == virtualNumber)
					return obj;

			return GlobalConstants.NOWHERE;
		}

		private int RealMobile (int virtualNumber)
		{
			// TODO: This could be binary search to make it faster.
			for (int mobile = 0; mobile < _topOfMobileTable; mobile++)
				if (_mobiles[mobile].Number == virtualNumber)
					return mobile;

			return GlobalConstants.NOWHERE;
		}

		private int RealTrigger (int virtualNumber)
		{
			// TODO: This could be binary search to make it faster.
			for (int trigger = 0; trigger < _topOfTriggerTable; trigger++)
				if (_triggerIndex[trigger].VirtualNumber == virtualNumber)
					return trigger;

			return GlobalConstants.NOWHERE;
		}

		private int RealZone (int virtualNumber)
		{
			// TODO: This could be binary search to make it faster.
			for (int zone = 0; zone < _topOfZoneTable; zone++)
				if (_zones[zone].Number == virtualNumber)
					return zone;

			return GlobalConstants.NOWHERE;
		}
	}
}
