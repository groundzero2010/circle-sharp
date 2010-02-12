using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
	internal delegate void CommandHandler(CharacterData character, string argument, int command, int subcommand);

	public partial class CircleCore
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

		private void CommandInterpreter(CharacterData character, string command)
		{
			Console.WriteLine("CommandInterpreter");
		}

		private void SkipSpaces(ref string text)
		{
			string temp = "";

			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] != ' ')
					temp += text[i];
			}

			text = temp;
		}

		private bool ParseName(string input, out string temp)
		{
			int i;
			string str = "";

			SkipSpaces(ref input);

			temp = String.Empty;

			for (i = 0; i < input.Length; i++)
				if (char.IsLetter(input[i]))
					str += input[i];

			temp = str;

			if (i == 0)
				return false;

			return true;
		}

		private string[] _fill = new string[]
        {
          "in",
          "inside",
          "into",
          "from",
          "with",
          "the",
          "on",
          "at",
          "to",
          "\n"
        };

		private string[] _reserved = new string[]
        {
          "a",
          "an",
          "self",
          "me",
          "all",
          "room",
          "someone",
          "something",
          "\n"
        };

		private bool FillWord(string input)
		{
			foreach (string fill in _fill)
				if (fill.CompareTo(input) == 0)
					return true;

			return false;
		}

		private bool ReservedWord(string input)
		{
			foreach (string reserve in _reserved)
				if (reserve.CompareTo(input) == 0)
					return true;

			return false;
		}

		private void DisplayRaces(DescriptorData descriptor)
		{
			SendToCharacter(descriptor.Character, "\r\n@YRace Selection Menu:\r\n----------------------\r\n@n");

			for (int i = 0; i < (int)RaceTypes.Index; i++)
				if (RaceGenderMatrix[(int)descriptor.Character.Player.Sex, i])
					SendToCharacter(descriptor.Character, "[" + i + "] " + ((RaceTypes)i).ToString());

			SendToCharacter(descriptor.Character, "\n@WRace@n: ");
		}

		private void DisplayClasses(DescriptorData descriptor)
		{
			SendToCharacter(descriptor.Character, "\r\n@YClass Selection Menu:\r\n---------------------\r\n@n");

			for (int i = 0; i < (int)ClassTypes.Index; i++)
				if (RaceClassMatrix[(int)descriptor.Character.Player.Race, i])
					SendToCharacter(descriptor.Character, "[" + i + "] " + ((ClassTypes)i).ToString());

			SendToCharacter(descriptor.Character, "\n@WClass@n: ");
		}

		private enum DupeMode { None = 0, Recon, Usurp, Unswitch };

		private bool PerformDupeCheck(DescriptorData descriptor)
		{
			long ID = descriptor.Character.CharacterSpecials.Saved.IDNumber;
			CharacterData target = null;
			DupeMode mode = DupeMode.None;

			foreach (DescriptorData cycle in _descriptors)
			{
				if (descriptor == cycle)
					continue;

				if (cycle.Original != null && cycle.Original.CharacterSpecials.Saved.IDNumber == ID)
				{
					WriteToOutput(descriptor, "\r\nMultiple login detected -- disconnecting.\r\n");
					descriptor.ConnectState = ConnectState.Close;

					if (target == null)
					{
						target = cycle.Original;
						mode = DupeMode.Unswitch;
					}

					if (cycle.Character != null)
						cycle.Character.Descriptor = null;

					cycle.Character = null;
					cycle.Original = null;
				}
				else if (cycle.Character != null && cycle.Character.CharacterSpecials.Saved.IDNumber == ID && cycle.Original != null)
				{
					DoReturn(cycle.Character, null, 0, 0);
				}
				else if (cycle.Character != null && cycle.Character.CharacterSpecials.Saved.IDNumber == ID)
				{
					if (target == null && cycle.ConnectState == ConnectState.Playing)
					{
						WriteToOutput(cycle, "\r\nThis body has been usurped!\r\n");
						target = cycle.Character;
						mode = DupeMode.Usurp;
					}

					cycle.Character.Descriptor = null;
					cycle.Character = null;
					cycle.Original = null;
					WriteToOutput(cycle, "\r\nMultiple login detected -- disconnecting.\r\n");
					cycle.ConnectState = ConnectState.Close;
				}
			}

			foreach (CharacterData cycle in _characters)
			{
				if (cycle.MobileFlagged(MobileFlags.IsNPC))
					continue;

				if (cycle.CharacterSpecials.Saved.IDNumber != ID)
					continue;

				if (cycle.Descriptor != null)
					continue;

				if (cycle == target)
					continue;

				if (target == null)
				{
					target = cycle;
					mode = DupeMode.Recon;
					continue;
				}

				if (target == null)
					return false;

				FreeCharacter(descriptor.Character);
				descriptor.Character = target;
				descriptor.Character.Descriptor = descriptor;
				descriptor.Original = null;
				descriptor.Character.CharacterSpecials.Timer = 0;

				descriptor.Character.RemovePlayerFlag(PlayerFlags.Writing);
				descriptor.Character.RemovePlayerFlag(PlayerFlags.Mailing);
				descriptor.Character.RemoveAffectFlag(AffectFlags.Group);

				descriptor.ConnectState = ConnectState.Playing;

				switch (mode)
				{
					case DupeMode.Recon:
						WriteToOutput(descriptor, "Reconnecting.\r\n");
						Act("$n has reconnected.", true, descriptor.Character, null, null, GlobalConstants.TO_ROOM);

						//mudlog(NRM, MAX(LVL_IMMORT, GET_INVIS_LEV(d->character)), TRUE, "%s [%s] has reconnected.", GET_NAME(d->character), d->host);

						if (HasMail(descriptor.Character.CharacterSpecials.Saved.IDNumber))
							WriteToOutput(descriptor, "You have mail waiting.\r\n");

						// FIXME: Something about ZLIB?
						break;

					case DupeMode.Usurp:
						WriteToOutput(descriptor, "You take over your own body, already in use!\r\n");
						Act("$n suddenly keels over in pain, surrounded by a white aura...\r\n" +
							"$n's body has been taken over by a new spirit!", true, descriptor.Character, null, null, GlobalConstants.TO_ROOM);
						//mudlog(NRM, MAX(LVL_IMMORT, GET_INVIS_LEV(d->character)), TRUE, 	"%s has re-logged in ... disconnecting old socket.", GET_NAME(d->character));
						break;

					case DupeMode.Unswitch:
						WriteToOutput(descriptor, "Reconnecting to unswitched character.");
						//mudlog(NRM, MAX(LVL_IMMORT, GET_INVIS_LEV(d->character)), TRUE, "%s [%s] has reconnected.", GET_NAME(d->character), d->host);
						break;
				}
			}

			return true;
		}

		private void Nanny(DescriptorData descriptor, string input)
		{
			string tmpName = String.Empty;

			// If there is no character object yet, we need to create one.
			if (descriptor.Character == null)
			{
				descriptor.Character = new CharacterData();
				descriptor.Character.Descriptor = descriptor;
			}

			// TODO: OLC states here.

			// Not in OLC, lets do regular welcome.
			switch (descriptor.ConnectState)
			{
				case ConnectState.GetName:
					if (descriptor.Character == null)
					{
						descriptor.Character = new CharacterData();
						descriptor.Character.Descriptor = descriptor;
					}

					if (input.Length == 0)
						descriptor.ConnectState = ConnectState.Close;
					else
					{
						if (!ParseName(input, out tmpName) || tmpName.Length < 2 || tmpName.Length > 25 || !ValidName(tmpName) || FillWord(tmpName) || ReservedWord(tmpName))
						{
							WriteToOutput(descriptor, "Invalid name, please try another.\r\nPlease provide your name: ");
							return;
						}
					}

					if (LoadCharacter(tmpName, descriptor.Character))
					{
						if (descriptor.Character.PlayerFlagged(PlayerFlags.Deleted))
						{
							RemovePlayer(descriptor.Character);

							descriptor.Character = null;

							FreeCharacter(descriptor.Character);

							if (!ValidName(tmpName))
							{
								WriteToOutput(descriptor, "Invalid name, please try another.\r\nPlease provide your name: ");
								return;
							}

							descriptor.Character = new CharacterData();
							descriptor.Character.Descriptor = descriptor;
							descriptor.Character.Player.Name = GlobalUtilities.Capital(tmpName);

							WriteToOutput(descriptor, "Did I get that right, @Y" + tmpName + "@n [Y/N]? ");
							descriptor.ConnectState = ConnectState.NameConfirm;
						}
						else
						{
							/* undo it just in case they are set */
							descriptor.Character.RemovePlayerFlag(PlayerFlags.Writing);
							descriptor.Character.RemovePlayerFlag(PlayerFlags.Mailing);
							descriptor.Character.RemovePlayerFlag(PlayerFlags.Cryo);
							descriptor.Character.RemoveAffectFlag(AffectFlags.Group);
							descriptor.Character.Player.Time.Logon = DateTime.Now;
							WriteToOutput(descriptor, "Please enter the @gpassword@n: ");
							EchoOff(descriptor);

							descriptor.IdleTicks = 0;

							descriptor.ConnectState = ConnectState.Password;
						}
					}
					else
					{
						if (!ValidName(tmpName))
						{
							WriteToOutput(descriptor, "Invalid name, please try another.\r\nPlease provide your name: ");
							return;
						}

						descriptor.Character.Player.Name = tmpName;

						WriteToOutput(descriptor, "Did I get that right, @Y" + tmpName + "@n [Y/N]?");

						descriptor.ConnectState = ConnectState.NameConfirm;
					}
					break;

				case ConnectState.NameConfirm:
					if (input.ToLower()[0] == 'y')
					{
						if (IsBanned(descriptor.Hostname) > BanTypes.New)
						{
							//mudlog(NRM, LVL_GOD, TRUE, "Request for new char %s denied from [%s] (siteban)", GET_PC_NAME(d->character), d->host);
							WriteToOutput(descriptor, "@RSorry, new characters are not allowed from your site!@n\r\n");
							descriptor.ConnectState = ConnectState.Close;
							return;
						}

						if (GlobalSettings.UserRestriction > 0)
						{
							WriteToOutput(descriptor, "@RSorry, new players can't be created at the moment.@n\r\n");
							//mudlog(NRM, LVL_GOD, TRUE, "Request for new char %s denied from [%s] (wizlock)", GET_PC_NAME(d->character), d->host);
							descriptor.ConnectState = ConnectState.Close;
							return;
						}

						WriteToOutput(descriptor, "@MNew Character!@n\r\nProvide a new @gpassword@n: ");
						EchoOff(descriptor);
						descriptor.ConnectState = ConnectState.NewPassword;
					}
					else if (input.ToLower()[0] == 'n')
					{
						WriteToOutput(descriptor, "Okay, then what IS your name: ");
						descriptor.Character.Player.Name = String.Empty;
						descriptor.ConnectState = ConnectState.GetName;
					}
					else
						WriteToOutput(descriptor, "Please type Yes or No [Y/N]: ");
					break;

				case ConnectState.Password:
					EchoOn(descriptor);

					WriteToOutput(descriptor, "\r\n"); // New EchoOn() eats the return on telnet, an extra space helps just in case.

					if (String.IsNullOrEmpty(input))
						descriptor.ConnectState = ConnectState.Close;
					else
					{
						if (descriptor.Character.Player.Password.CompareTo(input) != 0)
						{
							//mudlog(BRF, LVL_GOD, TRUE, "Bad PW: %s [%s]", GET_NAME(d->character), d->host);

							descriptor.Character.PlayerSpecials.Saved.BadPasswords++;
							SaveCharacter(descriptor.Character);

							if (++descriptor.BadPasswords >= GlobalConstants.MAX_BAD_PWS)
							{
								WriteToOutput(descriptor, "@RWrong password... disconnecting.@n\r\n");
								descriptor.ConnectState = ConnectState.Close;
							}
							else
							{
								WriteToOutput(descriptor, "@RWrong password@n. Please provide the @gpassword@n: ");
								EchoOff(descriptor);
							}

							return;
						}

						int hadBadPasswords = descriptor.Character.PlayerSpecials.Saved.BadPasswords;
						descriptor.Character.PlayerSpecials.Saved.BadPasswords = 0;
						descriptor.BadPasswords = 0;

						if (IsBanned(descriptor.Hostname) == BanTypes.Select && descriptor.Character.PlayerFlagged(PlayerFlags.SiteOK))
						{
							WriteToOutput(descriptor, "Sorry, this character has not been cleared to login from your site!\r\n");
							descriptor.ConnectState = ConnectState.Close;
							//mudlog(NRM, LVL_GOD, TRUE, "Connection attempt for %s denied from %s", GET_NAME(d->character), d->host);
							return;
						}

						if (descriptor.Character.Player.Level < GlobalSettings.UserRestriction)
						{
							WriteToOutput(descriptor, "The game is temporarily restricted... please try again later.\r\n");
							descriptor.ConnectState = ConnectState.Close;
							//mudlog(NRM, LVL_GOD, TRUE, "Request for login denied for %s [%s] (wizlock)", GET_NAME(d->character), d->host);
							return;
						}

						if (PerformDupeCheck(descriptor))
							return;

						if (descriptor.Character.Player.Level >= GlobalConstants.LVL_IMMORT)
							WriteToOutput(descriptor, _textIMOTD);
						else
							WriteToOutput(descriptor, _textMOTD);

						//if (descriptor.Character.PlayerSpecials.Saved.InvisibleLevel > 0)
						//mudlog(BRF, MAX(LVL_IMMORT, GET_INVIS_LEV(d->character)), TRUE, "%s [%s] has connected. (invis %d)", GET_NAME(d->character), d->host, GET_INVIS_LEV(d->character));
						//else
						//mudlog(BRF, LVL_IMMORT, TRUE, "%s [%s] has connected.", GET_NAME(d->character), d->host);

						if (hadBadPasswords > 0)
						{
							WriteToOutput(descriptor, "\r\n\r\n\007\007\007@R" + hadBadPasswords + " LOGIN FAILURE(S) SINCE LAST SUCCESSFUL LOGIN.@n\r\n");
							descriptor.Character.PlayerSpecials.Saved.BadPasswords = 0;
						}

						WriteToOutput(descriptor, "\r\n*** PRESS RETURN: ");

						descriptor.ConnectState = ConnectState.RMOTD;
					}
					break;

				case ConnectState.NewPassword:
				case ConnectState.ChangePasswordGetNew:
					if (String.IsNullOrEmpty(input) || input.Length > GlobalConstants.MAX_PWD_LENGTH || input.Length < 3 || descriptor.Character.Player.Name.CompareTo(input) == 0)
					{
						WriteToOutput(descriptor, "\r\nIllegal password.\r\nPlease provide your @gpassword@n: ");
						return;
					}

					descriptor.Character.Player.Password = input;

					WriteToOutput(descriptor, "\r\nPlease retype your @gpassword@n: ");

					if (descriptor.ConnectState == ConnectState.NewPassword)
						descriptor.ConnectState = ConnectState.ConfirmPassword;
					else
						descriptor.ConnectState = ConnectState.ChangePasswordVerify;
					break;

				case ConnectState.ConfirmPassword:
				case ConnectState.ChangePasswordVerify:
					if (descriptor.Character.Player.Password.CompareTo(input) != 0)
					{
						WriteToOutput(descriptor, "\r\nPasswords do not match... start over.\r\nPlease provide your @gpassword@n: ");
						if (descriptor.ConnectState == ConnectState.ConfirmPassword)
							descriptor.ConnectState = ConnectState.NewPassword;
						else
							descriptor.ConnectState = ConnectState.ChangePasswordGetNew;
						return;
					}
					EchoOn(descriptor);

					if (descriptor.ConnectState == ConnectState.ConfirmPassword)
					{
						WriteToOutput(descriptor, "\r\nWhat is your character's sex [M/F]: ");
						descriptor.ConnectState = ConnectState.QuestionSex;
					}
					else
					{
						SaveCharacter(descriptor.Character);
						WriteToOutput(descriptor, "\r\nDone.\r\n" + _textMenu);
						descriptor.ConnectState = ConnectState.Menu;
					}
					break;

				case ConnectState.QuestionSex:
					switch (input.ToLower()[0])
					{
						case 'm':
							descriptor.Character.Player.Sex = SexTypes.Male;
							break;

						case 'f':
							descriptor.Character.Player.Sex = SexTypes.Female;
							break;

						case 'n':
							descriptor.Character.Player.Sex = SexTypes.Neutral;
							break;

						default:
							WriteToOutput(descriptor, "That is not a sex..\r\nWhat IS your character's sex [M/F]: ");
							return;
					}

					DisplayRaces(descriptor);

					descriptor.ConnectState = ConnectState.QuestionRace;
					break;

				case ConnectState.QuestionRace:
					RaceTypes race = (RaceTypes)int.Parse(input);

					if (race == RaceTypes.Undefined)
					{
						WriteToOutput(descriptor, "\r\nThat's not a race.\r\nRace: ");
						return;
					}
					else
						descriptor.Character.Player.Race = race;

					DisplayClasses(descriptor);

					descriptor.ConnectState = ConnectState.QuestionClass;
					break;

				case ConnectState.QuestionClass:
					ClassTypes cls = (ClassTypes)int.Parse(input);

					if (cls == ClassTypes.Undefined)
					{
						WriteToOutput(descriptor, "\r\nThat's not a class.\r\nClass: ");
						return;
					}
					else
						descriptor.Character.Player.Class = cls;

					WriteToOutput(descriptor, "\r\n*** PRESS RETURN: ");
					descriptor.ConnectState = ConnectState.QuestionRollStats;
					break;

				case ConnectState.QuestionRollStats:

					break;
			}
		}

		private int FindCommand(string command)
		{
			return 0;
		}

		private bool Special(CharacterData character, int command, string arg)
		{
			if (GetRoomSpecial(character.InRoom) != null)
				if (GetRoomSpecial(character.InRoom)(character, _rooms[character.InRoom], command, arg))
					return true;

			for (int j = 0; j < (int)WearTypes.Index; j++)
				if (character.Equipment[j] != null && GetObjectSpecial(character.Equipment[j]) != null)
					if (GetObjectSpecial(character.Equipment[j])(character, character.Equipment[j], command, arg))
						return true;

			foreach (ObjectData obj in character.Inventory)
				if (GetObjectSpecial(obj) != null)
					if (GetObjectSpecial(obj)(character, obj, command, arg))
						return true;

			foreach (CharacterData person in _rooms[character.InRoom].People)
				if (!person.MobileFlagged(MobileFlags.NotDeadYet))
					if (GetMobileSpecial(person) != null)
						if (GetMobileSpecial(person)(character, person, command, arg))
							return true;

			foreach (ObjectData obj in _rooms[character.InRoom].Contents)
				if (GetObjectSpecial(obj) != null)
					if (GetObjectSpecial(obj)(character, obj, command, arg))
						return true;

			return false;
		}
	}
}