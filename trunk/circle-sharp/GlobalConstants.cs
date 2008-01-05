using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMUD
{
    internal static class GlobalConstants
    {
        public const string VERSION = "SharpMUD, v1.0";

        public const uint NOWHERE = 0;
        public const uint NOTHING = 0;
        public const uint NOBODY =  0;

        public const int LVL_IMPL = 34;
        public const int LVL_GRGOD = 33;
        public const int LVL_GOD = 32;
        public const int LVL_IMMORT = 31;
        public const int LVL_FREEZE = LVL_GRGOD;

        public const int OPT_USEC = 100;
        public const int PASSES_PER_SEC = (1000 / OPT_USEC);

        public const int SECS_PER_MUD_HOUR = 75;
        public const int SECS_PER_MUD_DAY = (24*SECS_PER_MUD_HOUR);
        public const int SECS_PER_MUD_MONTH = (35*SECS_PER_MUD_DAY);
        public const int SECS_PER_MUD_YEAR = (17*SECS_PER_MUD_MONTH);

        public const int SECS_PER_REAL_MIN = 60;
        public const int SECS_PER_REAL_HOUR = (60*SECS_PER_REAL_MIN);
        public const int SECS_PER_REAL_DAY = (24*SECS_PER_REAL_HOUR);
        public const int SECS_PER_REAL_YEAR = (365*SECS_PER_REAL_DAY);

        public const int PULSE_ZONE = 10 * PASSES_PER_SEC;
        public const int PULSE_MOBILE = 10 * PASSES_PER_SEC;
        public const int PULSE_SCRIPT = 13 * PASSES_PER_SEC;
        public const int PULSE_VIOLENCE = 2 * PASSES_PER_SEC;
        public const int PULSE_AUTOSAVE = 60 * PASSES_PER_SEC;
        public const int PULSE_IDLEPWD = 15 * PASSES_PER_SEC;
        public const int PULSE_SANITY = 30 * PASSES_PER_SEC;
        public const int PULSE_USAGE = 5 * 60 * PASSES_PER_SEC;
        public const int PULSE_TIMESAVE = 30 * 60 * PASSES_PER_SEC;

        public const int HISTORY_SIZE = 5;
        public const int MAX_MESSAGES = 60;
        public const int MAX_TONGUE = 3;
        public const int MAX_SKILLS = 200;
        public const int MAX_AFFECT = 32;
        public const int MAX_OBJ_AFFECT = 6;
        public const int MAX_PLAYERS = 150;

        public const int DB_BOOT_WORLD = 0;
        public const int DB_BOOT_MOBILE = 1;
        public const int DB_BOOT_OBJECT = 2;
        public const int DB_BOOT_ZONE = 3;
        public const int DB_BOOT_SHOP = 4;
        public const int DB_BOOT_HELP = 5;
        public const int DB_BOOT_TRIGGER = 6;
        
        public const string LIB_WORLD = "world\\";
        public const string LIB_TEXT = "text\\";
        public const string LIB_TEXT_HELP = "text\\help\\";
        public const string LIB_MISC = "misc\\";
        public const string LIB_ETC = "etc\\";
        public const string LIB_PLRTEXT = "plrtext\\";
        public const string LIB_PLROBJS = "plrobjs\\";
        public const string LIB_PLRVARS = "plrvars\\";
        public const string LIB_PLRALIAS = "plralias\\";
        public const string LIB_HOUSE = "house\\";

        public const string INDEX_FILE = "index";
        public const string MINDEX_FILE = "index.mini";
        public const string TRIGGER_PREFIX = LIB_WORLD+"trg\\";
        public const string MOBILE_PREFIX = LIB_WORLD+"mob\\";
        public const string OBJECT_PREFIX = LIB_WORLD+"obj\\";
        public const string ZONE_PREFIX = LIB_WORLD+"zon\\";
        public const string SHOP_PREFIX = LIB_WORLD+"shp\\";

        public const string HELP_PREFIX = LIB_TEXT+"help\\";
        
        public const string GREETINGS = "Welcome to SharpMUD!\r\n";
        public const string MENU = "This is the menu: ";
    }
}
