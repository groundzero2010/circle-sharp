using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace CircleSharp
{
    internal static class GlobalConstants
    {
        public const string VERSION = "CircleSharp, v1.0";

        public const int NOWHERE = -1;
        public const int NOTHING = -1;
        public const int NOBODY =  -1;

        public const int LVL_IMPL = 34;
        public const int LVL_GRGOD = 33;
        public const int LVL_GOD = 32;
		public const int LVL_BUILDER = LVL_GOD;
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
		public const int MAX_BAD_PWS = 3;
		public const int MAX_PWD_LENGTH = 16;
		public const int MAX_MAIL_SIZE = 4096;
		public const int MAX_MAIL_LEVEL = 2;
        public const int MAX_MESSAGES = 60;
        public const int MAX_TONGUE = 3;
        public const int MAX_SKILLS = 200;
        public const int MAX_AFFECT = 32;
        public const int MAX_OBJ_AFFECT = 6;
        public const int MAX_PLAYERS = 150;

        public const int DB_BOOT_ROOM = 0;
        public const int DB_BOOT_MOBILE = 1;
        public const int DB_BOOT_OBJECT = 2;
        public const int DB_BOOT_ZONE = 3;
        public const int DB_BOOT_SHOP = 4;
        public const int DB_BOOT_HELP = 5;
        public const int DB_BOOT_TRIGGER = 6;

		public const string LIB_DIR = "Library";

        public const string LIB_WORLD = "World";
        public const string LIB_TEXT = "Text";
        public const string LIB_TEXT_HELP = "Help";
        public const string LIB_MISC = "Misc";
        public const string LIB_ETC = "Etc";
		public const string LIB_PLAYERS = "Players";
        public const string LIB_PLRTEXT = "PlayerText";
        public const string LIB_PLROBJS = "PlayerObjects";
        public const string LIB_PLRVARS = "PlayerVariables";
        public const string LIB_PLRALIAS = "PlayerAlias";
        public const string LIB_HOUSE = "House";

        public const string INDEX_FILE = "Index";
        public const string MINDEX_FILE = "Index.mini";

        public const string TRIGGER_PREFIX = "Trigger";
        public const string MOBILE_PREFIX = "Mobile";
        public const string OBJECT_PREFIX = "Object";
        public const string ZONE_PREFIX = "Zone";
        public const string SHOP_PREFIX = "Shop";
		public const string ROOM_PREFIX = "Room";
        public const string HELP_PREFIX = "Help";

		public const int DEFAULT_STAFF_LEVEL = 12;
		public const int DEFAULT_WAND_LEVEL = 12;

		public const int TO_ROOM = 1;
		public const int TO_VICT = 2;
		public const int TO_NOTVICT = 3;
		public const int TO_CHAR = 4;
        public const int TO_GMOTE = 5;
		public const int TO_SLEEP = 128;
		public const int DG_NO_TRIG = 256;

		public const int RoomIDBase = 50000;
		public const int MobileIDBase = 100000;
		public const int ObjectIDBase = 200000;
		
		public const int MAX_HOUSES = 100;
		public const int MAX_GUESTS = 10;
		
        public const int RF_ARRAY_MAX = 4;
        public const int PR_ARRAY_MAX = 4;
        public const int AF_ARRAY_MAX = 4;
        public const int TW_ARRAY_MAX = 4;
        public const int EF_ARRAY_MAX = 4;
        public const int ZF_ARRAY_MAX = 4;

        public const string KNRM = "\x1B[0m";
        public const string KRED = "\x1B[31m";
        public const string KGRN = "\x1B[32m";
        public const string KYEL = "\x1B[33m";
        public const string KBLU = "\x1B[34m";
        public const string KMAG = "\x1B[35m";
        public const string KCYN = "\x1B[36m";
        public const string KWHT = "\x1B[37m";
        public const string KNUL = "";

        public const int C_OFF = 0;
        public const int C_BRI = 1;
        public const int C_NRM = 2;
        public const int C_CMP = 3;

        public const int IAC = 255;
        public const int DONT = 254;
        public const int DO	= 253;		/* please, you use option */
        public const int WONT = 252;		/* I won't use option */
        public const int WILL = 251;		/* I will use option */
        public const int SB = 250;		/* interpret as subnegotiation */
        public const int GA = 249;		/* you may reverse the line */
        public const int EL = 248;		/* erase the current line */
        public const int EC = 247;		/* erase the current character */
        public const int AYT = 246;		/* are you there */
        public const int AO = 245;		/* abort output--but let prog finish */
        public const int IP = 244;		/* interrupt process--permanently */
        public const int BREAK = 243;		/* break */
        public const int DM = 242;		/* data mark--for connect. cleaning */
        public const int NOP = 241;
        public const int SE = 240;
        public const int EOR = 239;
        public const int ABORT = 238;
        public const int SUSP = 237;
        public const int xEOF = 236;
        public const int SYNCH = 242;		/* for telfunc calls */

        public const int TELOPT_BINARY = 0;	/* 8-bit data path */
        public const int TELOPT_ECHO = 1;	/* echo */
        public const int TELOPT_RCP = 2;	/* prepare to reconnect */
        public const int TELOPT_SGA = 3;	/* suppress go ahead */
        public const int TELOPT_NAMS = 4;	/* approximate message size */
        public const int TELOPT_STATUS = 5;	/* give status */
        public const int TELOPT_TM = 6;	/* timing mark */
        public const int TELOPT_RCTE = 7;	/* remote controlled transmission and echo */
        public const int TELOPT_NAOL = 8;	/* negotiate about output line width */
        public const int TELOPT_NAOP = 9;	/* negotiate about output page size */
        public const int TELOPT_NAOCRD = 10;	/* negotiate about CR disposition */
        public const int TELOPT_NAOHTS = 11;	/* negotiate about horizontal tabstops */
        public const int TELOPT_NAOHTD = 12;	/* negotiate about horizontal tab disposition */
        public const int TELOPT_NAOFFD = 13;	/* negotiate about formfeed disposition */
        public const int TELOPT_NAOVTS = 14;	/* negotiate about vertical tab stops */
        public const int TELOPT_NAOVTD = 15;	/* negotiate about vertical tab disposition */
        public const int TELOPT_NAOLFD = 16;	/* negotiate about output LF disposition */
        public const int TELOPT_XASCII = 17;	/* extended ascic character set */
        public const int TELOPT_LOGOUT = 18;	/* force logout */
        public const int TELOPT_BM = 19;	/* byte macro */
        public const int TELOPT_DET = 20;	/* data entry terminal */
        public const int TELOPT_SUPDUP = 21;	/* supdup protocol */
        public const int TELOPT_SUPDUPOUTPUT = 22;	/* supdup output */
        public const int TELOPT_SNDLOC = 23;	/* send location */
        public const int TELOPT_TTYPE = 24;	/* terminal type */
        public const int TELOPT_EOR = 25;	/* end or record */
        public const int TELOPT_TUID = 26;	/* TACACS user identification */
        public const int TELOPT_OUTMRK = 27;	/* output marking */
        public const int TELOPT_TTYLOC = 28;	/* terminal location number */
        public const int TELOPT_3270REGIME = 29;	/* 3270 regime */
        public const int TELOPT_X3PAD = 30;	/* X.3 PAD */
        public const int TELOPT_NAWS = 31;	/* window size */
        public const int TELOPT_TSPEED = 32;	/* terminal speed */
        public const int TELOPT_LFLOW = 33;	/* remote flow control */
        public const int TELOPT_LINEMODE = 34;	/* Linemode option */
        public const int TELOPT_XDISPLOC = 35;	/* X Display Location */
        public const int TELOPT_OLD_ENVIRON = 36;	/* Old - Environment variables */
        public const int TELOPT_AUTHENTICATION = 37;/* Authenticate */
        public const int TELOPT_ENCRYPT = 38;	/* Encryption option */
        public const int TELOPT_NEW_ENVIRON = 39;	/* New - Environment variables */
        public const int TELOPT_EXOPL = 255;	/* extended-options-list */

		public static string[] Directions = new string[] {
		   "north",
		   "east",
		   "south",
		   "west",
		   "up",
		   "down",
		   "\n"
		};


    }
}
