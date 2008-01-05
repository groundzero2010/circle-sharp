using System;
using System.Collections.Generic;
using System.Text;

using SharpMUD.Structures;
using SharpMUD.Enumerations;

namespace SharpMUD
{
    public partial class SharpCore
    {
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
            GlobalUtilities.Log("Loading zone table.");
            IndexBoot(GlobalConstants.DB_BOOT_ZONE);

            GlobalUtilities.Log("Loading rooms.");
            IndexBoot(GlobalConstants.DB_BOOT_TRIGGER);

            GlobalUtilities.Log("Renumbering rooms.");
            RenumWorld();

            GlobalUtilities.Log("Checking start rooms.");
            CheckStartRooms();

            GlobalUtilities.Log("Loading mobs and generating index.");
            IndexBoot(GlobalConstants.DB_BOOT_MOBILE);

            GlobalUtilities.Log("Loading objs and generating index.");
            IndexBoot(GlobalConstants.DB_BOOT_OBJECT);

            GlobalUtilities.Log("Renumbering zone table.");
            RenumZoneTable();

            if (!_noSpecials)
            {
                GlobalUtilities.Log("Loading shops.");
                IndexBoot(GlobalConstants.DB_BOOT_SHOP);
            }
        }

        private void IndexBoot(int mode)
        {
            string indexFilename = String.Empty;
            string prefix = String.Empty;

            switch (mode)
            {
                case GlobalConstants.DB_BOOT_WORLD:
                    
                    break;

                case GlobalConstants.DB_BOOT_MOBILE:
                    prefix = GlobalConstants.MOBILE_PREFIX;
                    break;
                
                case GlobalConstants.DB_BOOT_OBJECT:
                    prefix = GlobalConstants.OBJECT_PREFIX;
                    break;

                case GlobalConstants.DB_BOOT_ZONE:
                    prefix = GlobalConstants.ZONE_PREFIX;
                    break;

                case GlobalConstants.DB_BOOT_SHOP:
                    prefix = GlobalConstants.MOBILE_PREFIX;
                    break;

                case GlobalConstants.DB_BOOT_HELP:
                    prefix = GlobalConstants.HELP_PREFIX;
                    break;

                case GlobalConstants.DB_BOOT_TRIGGER:
                    prefix = GlobalConstants.TRIGGER_PREFIX;
                    break;

                default:
                    GlobalUtilities.Log("SYSERR: Unknown subcommand "+mode+" to IndexBoot!");
                    throw new Exception();
            }

            if (_miniMud)
                indexFilename = GlobalConstants.MINDEX_FILE;
            else
                indexFilename = GlobalConstants.INDEX_FILE;


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
