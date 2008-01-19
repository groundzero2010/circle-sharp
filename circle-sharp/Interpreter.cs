using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
	internal delegate void CommandHandler (CharacterData character, string argument, int command, int subcommand);

    public partial class SharpCore
    {

		private CommandInfo[] _commandInfo = {
			new CommandInfo ("RESERVED", PositionTypes.Dead, null, 0, 0),	/* this must be first -- for specprocs */

			//new CommandInfo ("north",		PositionTypes.Standing,	DoMove,		0,	SubCommands.North),
			//new CommandInfo ("east",		PositionTypes.Standing,	DoMove,		0,	SubCommands.East),
			//new CommandInfo ("south",		PositionTypes.Standing,	DoMove,		0,	SubCommands.South),
			//new CommandInfo ("west",		PositionTypes.Standing,	DoMove,		0,	SubCommands.West),
			//new CommandInfo ("up",			PositionTypes.Standing,	DoMove,		0,	SubCommands.Up),
			//new CommandInfo ("down",		PositionTypes.Standing,	DoMove,		0,	SubCommands.Down),

			/*new CommandInfo ("at",			PositionTypes.Dead,		DoAt,		GlobalConstants.LVL_IMMORT,	SubCommands.None),
			new CommandInfo ("advance",		PositionTypes.Dead,		DoAdvance,	GlobalConstants.LVL_IMPL,	SubCommands.None),
			new CommandInfo ("alias",		PositionTypes.Dead,		DoAlias,	0,	SubCommands.None),
			new CommandInfo ("accuse",		PositionTypes.Sitting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("applaud",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("assist",		PositionTypes.Fighting,	DoAssist,	1,	SubCommands.None),
			new CommandInfo ("ask",			PositionTypes.Resting,	DoSpecComm,	0,	SubCommands.Ask),
			new CommandInfo ("auction",		PositionTypes.Sleeping,	DoGenComm,	0,	SubCommands.Auction),
			new CommandInfo ("autoexit",	PositionTypes.Dead,		DoGenTog,	0,	SubCommands.AutoExit),

			new CommandInfo ("bounce",		PositionTypes.Standing,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("backstab",	PositionTypes.Standing,	DoBackstab,	1,	SubCommands.None),
			new CommandInfo ("ban",			PositionTypes.Dead,		DoBan,		GlobalConstants.LVL_GRGOD, SubCommands.None),
			new CommandInfo ("balance",		PositionTypes.Standing,	DoNotHere,	1,	SubCommands.None),
			new CommandInfo ("bash",		PositionTypes.Fighting,	DoBash,		1,	SubCommands.None),
			new CommandInfo ("beg",			PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("bleed",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("blush",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("bow",			PositionTypes.Standing,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("brb",			PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("brief",		PositionTypes.Dead,		DoGenTog,	0,	SubCommands.Brief),
			new CommandInfo ("burp",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("buy",			PositionTypes.Standing,	DoNotHere,	0,	SubCommands.None),
			new CommandInfo ("bug",			PositionTypes.Dead,		DoGenWrite,	0,	SubCommands.Bug),

			new CommandInfo ("cast",		PositionTypes.Sitting,	DoCast,		1,	SubCommands.None),
			new CommandInfo ("cackle",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("check",		PositionTypes.Standing,	DoNotHere,	1,	SubCommands.None),
			new CommandInfo ("chuckle",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("clap",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("clear",		PositionTypes.Dead,		DoGenPs,	0,	SubCommands.Clear),
			new CommandInfo ("close",		PositionTypes.Sitting,	DoGenDoor,	0,	SubCommands.Close),
			new CommandInfo ("cls",			PositionTypes.Dead,		DoGenPs,	0,	SubCommands.Clear),
			new CommandInfo ("consider",	PositionTypes.Resting,	DoConsider,	0,	SubCommands.None),
			new CommandInfo ("color",		PositionTypes.Dead,		DoColor,	0,	SubCommands.None),
			new CommandInfo ("comfort",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("comb",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("commands",	PositionTypes.Dead,		DoCommands,	0,	SubCommands.Commands),
			new CommandInfo ("compact",		PositionTypes.Dead,		DoGenTog,	0,	SubCommands.Compact),
			new CommandInfo ("cough",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("credits",		PositionTypes.Dead,		DoGenPs,	0,	SubCommands.Credits),
			new CommandInfo ("cringe",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("cry",			PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("cuddle",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("curse",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("curtsey",		PositionTypes.Standing,	DoAction,	0,	SubCommands.None),

  			new CommandInfo ("dance",		PositionTypes.Standing,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("date",		PositionTypes.Dead,		DoDate,		GlobalConstants.LVL_IMMORT, SubCommands.Date),
			new CommandInfo ("daydream",	PositionTypes.Sleeping,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("dc",			PositionTypes.Dead,		DoDc,		GlobalConstants.LVL_GOD, SubCommands.None),
			new CommandInfo ("deposit",		PositionTypes.Standing,	DoNotHere,	1,	SubCommands.None),
			new CommandInfo ("diagnose",	PositionTypes.Resting,	DoDiagnose,	0,	SubCommands.None),
			new CommandInfo ("display",		PositionTypes.Dead,		DoDisplay,	0,	SubCommands.None),
			new CommandInfo ("donate",		PositionTypes.Resting,	DoDrop,		0,	SubCommands.Donate),
			new CommandInfo ("drink",		PositionTypes.Resting,	DoDrink,	0,	SubCommands.Drink),
			new CommandInfo ("drop",		PositionTypes.Resting,	DoDrop,		0,	SubCommands.Drop),
			new CommandInfo ("drool",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),

			new CommandInfo ("eat",			PositionTypes.Resting,	DoEat,		0,	SubCommands.Eat),
			new CommandInfo ("echo",		PositionTypes.Sleeping,	DoEcho,		GlobalConstants.LVL_IMMORT, SubCommands.Echo),
			new CommandInfo ("emote",		PositionTypes.Resting,	DoEcho,		1,	SubCommands.Emote),
			new CommandInfo (":",			PositionTypes.Resting,	DoEcho,		1,	SubCommands.Emote),
			new CommandInfo ("embrace",		PositionTypes.Standing,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("enter",		PositionTypes.Standing,	DoEnter,	0,	SubCommands.None),
			new CommandInfo ("equipment",	PositionTypes.Sleeping,	DoEquipment,0,	SubCommands.None),
			new CommandInfo ("exits",		PositionTypes.Resting,	DoExits,	0,	SubCommands.None),
			new CommandInfo ("examine",		PositionTypes.Sitting,	DoExamine,	0,	SubCommands.None),
			new CommandInfo ("edit",		PositionTypes.Dead,		DoEdit,		GlobalConstants.LVL_IMPL, SubCommands.None),

			new CommandInfo ("force",		PositionTypes.Sleeping,	DoForce,	GlobalConstants.LVL_GOD, SubCommands.None),
			new CommandInfo ("fart",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("fill",		PositionTypes.Standing,	DoPour,		0,	SubCommands.Fill),
			new CommandInfo ("flee",		PositionTypes.Fighting,	DoFlee,		1,	SubCommands.None),
			new CommandInfo ("flip",		PositionTypes.Standing,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("flirt",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("follow",		PositionTypes.Resting,	DoFollow,	0,	SubCommands.None),
			new CommandInfo ("fondle",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("freeze",		PositionTypes.Dead,		DoWizUtil,	GlobalConstants.LVL_FREEZE, SubCommands.Freeze),
			new CommandInfo ("french",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("frown",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("fume",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),

  			new CommandInfo ("get",			PositionTypes.Resting,	DoGet,		0,	SubCommands.None),
			new CommandInfo ("gasp",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("gecho",		PositionTypes.Dead,		DoGEcho,	GlobalConstants.LVL_GOD, SubCommands.None),
			new CommandInfo ("give",		PositionTypes.Resting,	DoGive,		0,	SubCommands.None),
			new CommandInfo ("giggle",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("glare",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("goto",		PositionTypes.Sleeping,	DoGoto,		GlobalConstants.LVL_IMMORT, SubCommands.None),
			new CommandInfo ("gold",		PositionTypes.Resting,	DoGold,		0,	SubCommands.None),
			new CommandInfo ("gossip",		PositionTypes.Sleeping,	DoGenComm,	0,	SubCommands.Gossip),
			new CommandInfo ("group",		PositionTypes.Resting,	DoGroup,	1,	SubCommands.None),
			new CommandInfo ("grab",		PositionTypes.Resting,	DoGrab,		0,	SubCommands.None),
			new CommandInfo ("grats",		PositionTypes.Sleeping,	DoGenComm,	0,	SubCommands.Gratz),
			new CommandInfo ("greet",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("grin",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("groan",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("grope",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("grovel",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("growl",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("gsay",		PositionTypes.Sleeping,	DoGSay,		0,	SubCommands.None),
			new CommandInfo ("gtell",		PositionTypes.Sleeping,	DoGSay,		0,	SubCommands.None),

			new CommandInfo ("help",		PositionTypes.Dead,		DoHelp,		0,	SubCommands.None),
			new CommandInfo ("handbook",	PositionTypes.Dead,		DoGenPs,	GlobalConstants.LVL_IMMORT, SubCommands.Handbook),
			new CommandInfo ("hcontrol",	PositionTypes.Dead,		DoHControl,	GlobalConstants.LVL_GRGOD, SubCommands.None),
			new CommandInfo ("hiccup",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("hide",		PositionTypes.Resting,	DoHide,		1,	SubCommands.None),
			new CommandInfo ("hit",			PositionTypes.Fighting,	DoHit,		0,	SubCommands.Hit),
			new CommandInfo ("hold",		PositionTypes.Resting,	DoGrab,		1,	SubCommands.None),
			new CommandInfo ("holler",		PositionTypes.Resting,	DoGenComm,	1,	SubCommands.Holler),
			new CommandInfo ("holylight",	PositionTypes.Dead,		DoGenTog,	GlobalConstants.LVL_IMMORT, SubCommands.HolyLight),
			new CommandInfo ("hop",			PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("house",		PositionTypes.Resting,	DoHouse,	0,	SubCommands.None),
			new CommandInfo ("hug",			PositionTypes.Resting,	DoAction,	0,	SubCommands.None),

  			new CommandInfo ("inventory",	PositionTypes.Dead,		DoInventory,0,	SubCommands.None),
			new CommandInfo ("idea",		PositionTypes.Dead,		DoGenWrite,	0,	SubCommands.Idea),
			new CommandInfo ("imotd",		PositionTypes.Dead,		DoGenPs,	GlobalConstants.LVL_IMMORT, SubCommands.IMOTD),
			new CommandInfo ("immlist",		PositionTypes.Dead,		DoGenPs,	0,	SubCommands.ImmList),
			new CommandInfo ("info",		PositionTypes.Sleeping,	DoGenPs,	0,	SubCommands.Info),
			new CommandInfo ("insult",		PositionTypes.Resting,	DoInsult,	0,	SubCommands.None),
			new CommandInfo ("invis",		PositionTypes.Dead,		DoInvis,	GlobalConstants.LVL_IMMORT, SubCommands.None),

			new CommandInfo ("junk",		PositionTypes.Resting,	DoDrop,		0,	SubCommands.Junk),

  			new CommandInfo ("kill",		PositionTypes.Fighting,	DoKill,		0,	SubCommands.None),
			new CommandInfo ("kick",		PositionTypes.Fighting,	DoKick,		1,	SubCommands.None),
			new CommandInfo ("kiss",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),

  			new CommandInfo ("look",		PositionTypes.Resting,	DoLook,		0, SubCommands.Look),
			new CommandInfo ("laugh",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("last",		PositionTypes.Dead,		DoLast,		GlobalConstants.LVL_GOD, SubCommands.None),
			new CommandInfo ("leave",		PositionTypes.Standing,	DoLeave,	0, SubCommands.None),
			new CommandInfo ("levels",		PositionTypes.Dead,		DoLevels,	0, SubCommands.None),
			new CommandInfo ("list",		PositionTypes.Standing,	DoNotHere,	0, SubCommands.None),
			new CommandInfo ("lick",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("lock",		PositionTypes.Sitting,	DoGenDoor,	0, SubCommands.Lock),
			new CommandInfo ("load",		PositionTypes.Dead,		DoLoad,		GlobalConstants.LVL_GOD, SubCommands.None),
			new CommandInfo ("love",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),

  			new CommandInfo ("moan",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("motd",		PositionTypes.Dead,		DoGenPs,	0, SubCommands.MOTD),
			new CommandInfo ("mail",		PositionTypes.Standing,	DoNotHere,	1, SubCommands.None),
			new CommandInfo ("massage",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("medit",		PositionTypes.Dead,		DoOLC,		GlobalConstants.LVL_BUILDER, SubCommands.MEdit),
			new CommandInfo ("mute",		PositionTypes.Dead,		DoWizUtil,	GlobalConstants.LVL_GOD, SubCommands.Squelch),
			new CommandInfo ("murder",		PositionTypes.Fighting,	DoHit,		0, SubCommands.Murder),

  			new CommandInfo ("news",		PositionTypes.Sleeping,	DoGenPs,	0, SubCommands.News),
			new CommandInfo ("nibble",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("nod",			PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("noauction",	PositionTypes.Dead,		DoGenTog,	0, SubCommands.NoAuction),
			new CommandInfo ("nogossip",	PositionTypes.Dead,		DoGenTog,	0, SubCommands.NoGossip),
			new CommandInfo ("nograts",		PositionTypes.Dead,		DoGenTog,	0, SubCommands.NoGratz),
			new CommandInfo ("nohassle",	PositionTypes.Dead,		DoGenTog,	GlobalConstants.LVL_IMMORT, SubCommands.NoHassle),
			new CommandInfo ("norepeat",	PositionTypes.Dead,		DoGenTog,	0, SubCommands.NoRepeat),
			new CommandInfo ("noshout",		PositionTypes.Sleeping,	DoGenTog,	1, SubCommands.Deaf),
			new CommandInfo ("nosummon",	PositionTypes.Dead,		DoGenTog,	1, SubCommands.NoSummon),
			new CommandInfo ("notell",		PositionTypes.Dead,		DoGenTog,	1, SubCommands.NoTell),
			new CommandInfo ("notitle",		PositionTypes.Dead,		DoWizUtil,	GlobalConstants.LVL_GOD, SubCommands.NoTitle),
			new CommandInfo ("nowiz",		PositionTypes.Dead,		DoGenTog,	GlobalConstants.LVL_IMMORT, SubCommands.NoWiz),
			new CommandInfo ("nudge",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("nuzzle",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),

  			new CommandInfo ("order",		PositionTypes.Resting,	DoOrder,	1,	SubCommands.None),
			new CommandInfo ("offer",		PositionTypes.Standing,	DoNotHere,	1,	SubCommands.None),
			new CommandInfo ("open",		PositionTypes.Sitting,	DoGenDoor,	0,	SubCommands.Open),
			new CommandInfo ("olc",			PositionTypes.Dead,		DoOLC,		GlobalConstants.LVL_BUILDER, SubCommands.SaveInfo),
			new CommandInfo ("oedit",		PositionTypes.Dead,		DoOLC,		GlobalConstants.LVL_BUILDER, SubCommands.OEdit),

  			new CommandInfo ("put",			PositionTypes.Resting,	DoPut,		0, SubCommands.None),
			new CommandInfo ("pat",			PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("page",		PositionTypes.Dead,		DoPage,		GlobalConstants.LVL_GOD, SubCommands.None),
			new CommandInfo ("pardon",		PositionTypes.Dead,		DoWizUtil,	GlobalConstants.LVL_GOD, SubCommands.Pardon),
			new CommandInfo ("peer",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("pick",		PositionTypes.Standing,	DoGenDoor,	1, SubCommands.Pick),
			new CommandInfo ("point",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("poke",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("policy",		PositionTypes.Dead,		DoGenPs,	0, SubCommands.Policies),
			new CommandInfo ("ponder",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("poofin",		PositionTypes.Dead,		DoPoofSet,	GlobalConstants.LVL_IMMORT, SubCommands.PoofIn),
			new CommandInfo ("poofout",		PositionTypes.Dead,		DoPoofSet,	GlobalConstants.LVL_IMMORT, SubCommands.PoofOut),
			new CommandInfo ("pour",		PositionTypes.Standing,	DoPour,		0, SubCommands.Pour),
			new CommandInfo ("pout",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("prompt",		PositionTypes.Dead,		DoDisplay,	0, SubCommands.None),
			new CommandInfo ("practice",	PositionTypes.Resting,	DoPractice,	1, SubCommands.None),
			new CommandInfo ("pray",		PositionTypes.Sitting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("puke",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("punch",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("purr",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("purge",		PositionTypes.Dead,		DoPurge,	GlobalConstants.LVL_GOD, SubCommands.None),

  			new CommandInfo ("quaff",		PositionTypes.Resting,	DoUse,		0, SubCommands.Quaff),
			new CommandInfo ("qecho",		PositionTypes.Dead,		DoQComm,	GlobalConstants.LVL_IMMORT, SubCommands.QEcho),
			new CommandInfo ("quest",		PositionTypes.Dead,		DoGenTog,	0, SubCommands.Quest),
			new CommandInfo ("qui",			PositionTypes.Dead,		DoQuit,		0, SubCommands.None),
			new CommandInfo ("quit",		PositionTypes.Dead,		DoQuit,		0, SubCommands.Quit),
			new CommandInfo ("qsay",		PositionTypes.Resting,	DoQComm,	0, SubCommands.QSay),

  			new CommandInfo ("reply",		PositionTypes.Sleeping,	DoReply,	0,	SubCommands.None),
			new CommandInfo ("rest",		PositionTypes.Resting,	DoRest,		0,	SubCommands.None),
			new CommandInfo ("read",		PositionTypes.Resting,	DoLook,		0,	SubCommands.Read),
			new CommandInfo ("reload",		PositionTypes.Dead,		DoReboot,	GlobalConstants.LVL_IMPL, SubCommands.None),
			new CommandInfo ("recite",		PositionTypes.Resting,	DoUse,		0,	SubCommands.Recite),
			new CommandInfo ("receive",		PositionTypes.Standing,	DoNotHere,	1,	SubCommands.None),
			new CommandInfo ("remove",		PositionTypes.Resting,	DoRemove,	0,	SubCommands.None),
			new CommandInfo ("rent",		PositionTypes.Standing,	DoNotHere,	1,	SubCommands.None),
			new CommandInfo ("report",		PositionTypes.Resting,	DoReport,	0,	SubCommands.None),
			new CommandInfo ("reroll",		PositionTypes.Dead,		DoWizUtil,	GlobalConstants.LVL_GRGOD, SubCommands.ReRoll),
			new CommandInfo ("rescue",		PositionTypes.Fighting,	DoRescue,	1,	SubCommands.None),
			new CommandInfo ("restore",		PositionTypes.Dead,		DoRestore,	GlobalConstants.LVL_GOD, SubCommands.None),
			new CommandInfo ("return",		PositionTypes.Dead,		DoReturn,	0,	SubCommands.None),
			new CommandInfo ("redit",		PositionTypes.Dead,		DoOLC,		GlobalConstants.LVL_BUILDER, SubCommands.REdit),
			new CommandInfo ("roll",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("roomflags",	PositionTypes.Dead,		DoGenTog,	GlobalConstants.LVL_IMMORT, SubCommands.RoomFlags),
			new CommandInfo ("ruffle",		PositionTypes.Standing,	DoAction,	0,	SubCommands.None),

    		new CommandInfo ("say",			PositionTypes.Resting,	DoSay,		0,	SubCommands.None),
			new CommandInfo ("'",			PositionTypes.Resting,	DoSay,		0,	SubCommands.None),
			new CommandInfo ("save",		PositionTypes.Sleeping,	DoSave,		0,	SubCommands.None),
			new CommandInfo ("saveall",		PositionTypes.Dead,		DoSaveAll,	GlobalConstants.LVL_BUILDER, SubCommands.None),
			new CommandInfo ("score",		PositionTypes.Dead,		DoScore,	0,	SubCommands.None),
			new CommandInfo ("scream",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("sell",		PositionTypes.Standing,	DoNotHere,	0,	SubCommands.None),
			new CommandInfo ("send",		PositionTypes.Sleeping,	DoSend,		GlobalConstants.LVL_GOD, SubCommands.None),
			new CommandInfo ("set",			PositionTypes.Dead,		DoSet,		GlobalConstants.LVL_GOD, SubCommands.None),
			new CommandInfo ("sedit",		PositionTypes.Dead,		DoOLC,		GlobalConstants.LVL_BUILDER, SubCommands.SEdit),
			new CommandInfo ("shout",		PositionTypes.Resting,	DoGenComm,	0,	SubCommands.Shout),
			new CommandInfo ("shake",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("shiver",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("show",		PositionTypes.Dead,		DoShow,		GlobalConstants.LVL_IMMORT, SubCommands.None),
			new CommandInfo ("shrug",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("shutdow",		PositionTypes.Dead,		DoShutdown,	GlobalConstants.LVL_IMPL, SubCommands.None),
			new CommandInfo ("shutdown",	PositionTypes.Dead,		DoShutdown,	GlobalConstants.LVL_IMPL, SubCommands.Shutdown),
			new CommandInfo ("sigh",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("sing",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("sip",			PositionTypes.Resting,	DoDrink,	0,	SubCommands.Sip),
			new CommandInfo ("sit",			PositionTypes.Resting,	DoSit,		0,	SubCommands.None),
			new CommandInfo ("skillset",	PositionTypes.Sleeping,	DoSkillSet,	GlobalConstants.LVL_GRGOD, SubCommands.None),
  			new CommandInfo ("sleep",		PositionTypes.Sleeping,	DoSleep,	0,	SubCommands.None),
			new CommandInfo ("slap",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("slowns",		PositionTypes.Dead,		DoGenTog,	GlobalConstants.LVL_IMPL, SubCommands.SlowNS),
			new CommandInfo ("smile",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("smirk",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("snicker",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("snap",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("snarl",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("sneeze",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("sneak",		PositionTypes.Standing,	DoSneak,	0,	SubCommands.None),
			new CommandInfo ("sniff",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("snore",		PositionTypes.Sleeping,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("snowball",	PositionTypes.Standing,	DoAction,	GlobalConstants.LVL_IMMORT, SubCommands.None),
			new CommandInfo ("snoop",		PositionTypes.Dead,		DoSnoop,	GlobalConstants.LVL_GOD, SubCommands.None),
			new CommandInfo ("snuggle",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("socials",		PositionTypes.Dead,		DoCommands,	0, SubCommands.Socials),
			new CommandInfo ("split",		PositionTypes.Sitting,	DoSplit,	1, SubCommands.None),
			new CommandInfo ("spank",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("spit",		PositionTypes.Standing, DoAction,	0, SubCommands.None),
			new CommandInfo ("squeeze",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("stand",		PositionTypes.Resting,	DoStand,	0, SubCommands.None),
			new CommandInfo ("stare",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("stat",		PositionTypes.Dead,		DoStat,		GlobalConstants.LVL_IMMORT, SubCommands.None),
			new CommandInfo ("steal",		PositionTypes.Standing, DoSteal,	1, SubCommands.None),
			new CommandInfo ("steam",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("stroke",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("strut",		PositionTypes.Standing,	DoAction,	0, SubCommands.None),
			new CommandInfo ("sulk",		PositionTypes.Resting,	DoAction,	0, SubCommands.None),
			new CommandInfo ("switch",		PositionTypes.Dead,		DoSwitch,	GlobalConstants.LVL_GRGOD, SubCommands.None),
			new CommandInfo ("syslog",		PositionTypes.Dead,		DoSysLog,	GlobalConstants.LVL_IMMORT, SubCommands.None),

  			new CommandInfo ("tell",		PositionTypes.Dead,		DoTell,		0,	SubCommands.None),
			new CommandInfo ("tackle",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("take",		PositionTypes.Resting,	DoGet,		0,	SubCommands.None),
			new CommandInfo ("tango",		PositionTypes.Standing,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("taunt",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("taste",		PositionTypes.Resting,	DoEat,		0,	SubCommands.Taste),
			new CommandInfo ("teleport",	PositionTypes.Dead,		DoTeleport,	GlobalConstants.LVL_GOD, SubCommands.None),
			new CommandInfo ("tedit",		PositionTypes.Dead,		DoTEdit,	GlobalConstants.LVL_GRGOD, SubCommands.None),
			new CommandInfo ("thank",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("think",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("thaw",		PositionTypes.Dead,		DoWizUtil,	GlobalConstants.LVL_FREEZE, SubCommands.Thaw),
			new CommandInfo ("title",		PositionTypes.Dead,		DoTitle,	0,	SubCommands.None),
			new CommandInfo ("tickle",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("time",		PositionTypes.Dead,		DoTime,		0,	SubCommands.None),
			new CommandInfo ("toggle",		PositionTypes.Dead,		DoToggle,	0,	SubCommands.None),
			new CommandInfo ("track",		PositionTypes.Standing,	DoTrack,	0,	SubCommands.None),
			new CommandInfo ("trackthru",	PositionTypes.Dead,		DoGenTog,	GlobalConstants.LVL_IMPL, SubCommands.Track),
			new CommandInfo ("transfer",	PositionTypes.Sleeping,	DoTrans,	GlobalConstants.LVL_GOD, SubCommands.None),
			new CommandInfo ("trigedit",	PositionTypes.Dead,		DoOLC,		GlobalConstants.LVL_BUILDER, SubCommands.TEdit),
			new CommandInfo ("twiddle",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("typo",		PositionTypes.Dead,		DoGenWrite, 0,	SubCommands.Typo),

  			new CommandInfo ("unlock",		PositionTypes.Sitting,	DoGenDoor,	0,	SubCommands.Unlock),
			new CommandInfo ("ungroup",		PositionTypes.Dead,		DoUngroup,	0,	SubCommands.None),
			new CommandInfo ("unban",		PositionTypes.Dead,		DoUnban,	GlobalConstants.LVL_GRGOD, SubCommands.None),
			new CommandInfo ("unaffect",	PositionTypes.Dead,		DoWizUtil,	GlobalConstants.LVL_GOD, SubCommands.Unaffect),
			new CommandInfo ("uptime",		PositionTypes.Dead,		DoDate,		GlobalConstants.LVL_IMMORT, SubCommands.UpTime),
			new CommandInfo ("use",			PositionTypes.Sitting,	DoUse,		1,	SubCommands.Use),
			new CommandInfo ("users",		PositionTypes.Dead,		DoUsers,	GlobalConstants.LVL_IMMORT, SubCommands.None),

			new CommandInfo ("value",		PositionTypes.Standing, DoNotHere,	0, SubCommands.None),
			new CommandInfo ("version",		PositionTypes.Dead,		DoGenPs,	0, SubCommands.Version),
			new CommandInfo ("visible",		PositionTypes.Resting,	DoVisible,	1, SubCommands.None),
			new CommandInfo ("vnum",		PositionTypes.Dead,		DoVNum,		GlobalConstants.LVL_IMMORT, SubCommands.None),
			new CommandInfo ("vstat",		PositionTypes.Dead,		DoVStat,	GlobalConstants.LVL_IMMORT, SubCommands.None),

  			new CommandInfo ("wake",		PositionTypes.Sleeping, DoWake,		0,	SubCommands.None),
			new CommandInfo ("wave",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("wear",		PositionTypes.Resting,	DoWear,		0,	SubCommands.None),
			new CommandInfo ("weather",		PositionTypes.Resting,	DoWeather,	0,	SubCommands.None),
			new CommandInfo ("who",			PositionTypes.Dead,		DoWho,		0,	SubCommands.None),
			new CommandInfo ("whoami",		PositionTypes.Dead,		DoGenPs,	0,	SubCommands.WhoAmI),
			new CommandInfo ("where",		PositionTypes.Resting,	DoWhere,	1,	SubCommands.None),
			new CommandInfo ("whisper",		PositionTypes.Resting,	DoSpecComm, 0,	SubCommands.Whisper),
			new CommandInfo ("whine",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("whistle",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("wield",		PositionTypes.Resting,	DoWield,	0,	SubCommands.None),
			new CommandInfo ("wiggle",		PositionTypes.Standing, DoAction,	0,	SubCommands.None),
			new CommandInfo ("wimpy",		PositionTypes.Dead,		DoWimpy,	0,	SubCommands.None),
			new CommandInfo ("wink",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("withdraw",	PositionTypes.Standing, DoNotHere,	1,	SubCommands.None),
			new CommandInfo ("wiznet",		PositionTypes.Dead,		DoWizNet,	GlobalConstants.LVL_IMMORT, SubCommands.None),
			new CommandInfo (";",			PositionTypes.Dead,		DoWizNet,	GlobalConstants.LVL_IMMORT, SubCommands.None),
			new CommandInfo ("wizhelp",		PositionTypes.Sleeping, DoCommands, GlobalConstants.LVL_IMMORT, SubCommands.WizHelp),
			new CommandInfo ("wizlist",		PositionTypes.Dead,		DoGenPs,	0,	SubCommands.WizList),
			new CommandInfo ("wizlock",		PositionTypes.Dead,		DoWizLock,	GlobalConstants.LVL_IMPL, SubCommands.None),
			new CommandInfo ("worship",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("write",		PositionTypes.Standing, DoWrite,	1,	SubCommands.None),

  			new CommandInfo ("yawn",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),
			new CommandInfo ("yodel",		PositionTypes.Resting,	DoAction,	0,	SubCommands.None),

  			new CommandInfo ("zedit",		PositionTypes.Dead,		DoOLC,		GlobalConstants.LVL_BUILDER, SubCommands.ZEdit),
			new CommandInfo ("zreset",		PositionTypes.Dead,		DoZReset,	GlobalConstants.LVL_GRGOD, SubCommands.None),

			// Trigger Commands
  			new CommandInfo ("attach",		PositionTypes.Dead,		DoAttach,		GlobalConstants.LVL_IMPL, SubCommands.None),
			new CommandInfo ("detach",		PositionTypes.Dead,		DoDetach,		GlobalConstants.LVL_IMPL, SubCommands.None),
			new CommandInfo ("tlist",		PositionTypes.Dead,		DoTList,		GlobalConstants.LVL_GOD, SubCommands.None),
			new CommandInfo ("tstat",		PositionTypes.Dead,		DoTStat,		GlobalConstants.LVL_GOD, SubCommands.None),
			new CommandInfo ("masound",		PositionTypes.Dead,		DoMASound,		-1,	SubCommands.None),
			new CommandInfo ("mkill",		PositionTypes.Standing,	DoMKill,		-1, SubCommands.None),
			new CommandInfo ("mjunk",		PositionTypes.Sitting,	DoMJunk,		-1, SubCommands.None),
			new CommandInfo ("mdamage",		PositionTypes.Dead,		DoMDamage,		-1, SubCommands.None),
			new CommandInfo ("mdoor",		PositionTypes.Dead,		DoMDoor,		-1, SubCommands.None),
			new CommandInfo ("mecho",		PositionTypes.Dead,		DoMEcho,		-1, SubCommands.None),
			new CommandInfo ("mechoaround",	PositionTypes.Dead,		DoMEchoAround,	-1, SubCommands.None),
			new CommandInfo ("msend",		PositionTypes.Dead,		DoMSend,		-1, SubCommands.None),
			new CommandInfo ("mload",		PositionTypes.Dead,		DoMLoad,		-1, SubCommands.None),
			new CommandInfo ("mpurge",		PositionTypes.Dead,		DoMPurge,		-1, SubCommands.None),
			new CommandInfo ("mgoto",		PositionTypes.Dead,		DoMGoto,		-1, SubCommands.None),
			new CommandInfo ("mat",			PositionTypes.Dead,		DoMAt,			-1, SubCommands.None),
			new CommandInfo ("mteleport",	PositionTypes.Dead,		DoMTeleport,	-1, SubCommands.None),
			new CommandInfo ("mforce",		PositionTypes.Dead,		DoMForce,		-1, SubCommands.None),
			new CommandInfo ("mhunt",		PositionTypes.Dead,		DoMHunt,		-1, SubCommands.None),
			new CommandInfo ("mremember",	PositionTypes.Dead,		DoMRemember,	-1, SubCommands.None),
			new CommandInfo ("mforget",		PositionTypes.Dead,		DoMForget,		-1, SubCommands.None),
			new CommandInfo ("mtransform",	PositionTypes.Dead,		DoMTransform,	-1, SubCommands.None),
			new CommandInfo ("mzoneecho",	PositionTypes.Dead,		DoMZoneEcho,	-1, SubCommands.None),
			new CommandInfo ("vdelete",		PositionTypes.Dead,		DoVDelete,		GlobalConstants.LVL_IMPL, SubCommands.None),
			*/
  			new CommandInfo ("\n", PositionTypes.Dead, null, 0, SubCommands.None), // This must be the last
		};

        private void CommandInterpreter (CharacterData character, string command)
        {
            Console.WriteLine("CommandInterpreter");
        }

        private void Nanny(DescriptorData descriptor, string command)
        {
            Console.WriteLine("Nanny");
        }

		private int FindCommand(string command)
		{
			return 0;
		}

		private bool Special (CharacterData character, int command, string arg)
		{
			if (GetRoomSpecial(character.InRoom) != null)
				if (GetRoomSpecial (character.InRoom) (character, _rooms[character.InRoom], command, arg))
					return true;

			for (int j = 0; j < (int)WearTypes.Index; j++)
				if (character.Equipment[j] != null && GetObjectSpecial (character.Equipment[j]) != null)
					if (GetObjectSpecial(character.Equipment[j]) (character, character.Equipment[j], command, arg))
						return true;

			foreach (ObjectData obj in character.Inventory)
				if (GetObjectSpecial(obj) != null)
					if (GetObjectSpecial (obj) (character, obj, command, arg))
						return true;

			foreach (CharacterData person in _rooms[character.InRoom].People)
				if (!person.MobileFlagged (MobileFlags.NotDeadYet))
					if (GetMobileSpecial (person) != null)
						if (GetMobileSpecial (person) (character, person, command, arg))
							return true;

			foreach (ObjectData obj in _rooms[character.InRoom].Contents)
				if (GetObjectSpecial (obj) != null)
					if (GetObjectSpecial (obj) (character, obj, command, arg))
						return true;

			return false;
		}
    }
}
