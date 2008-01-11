using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleSharp.Enumerations
{
	internal enum SubCommands
	{
		// DoMove
		North,
		East,
		South,
		West,
		Up,
		Down,

		// DoGenPs
		Info,
		Handbook,
		Credits,
		News,
		WizList,
		Policies,
		Version,
		ImmList,
		MOTD,
		IMOTD,
		Clear,
		WhoAmI,

		// DoGenTog
		NoSummon,
		NoHassle,
		Brief,
		Compact,
		NoTell,
		NoAuction,
		Deaf,
		NoGossip,
		NoGratz,
		NoWiz,
		Quest,
		RoomFlags,
		NoRepeat,
		HolyLight,
		SlowNS,
		AutoExit,
		Track,
		
		// DoWizUtil
		ReRoll,
		Pardon,
		NoTitle,
		Squelch,
		Freeze,
		Thaw,
		Unaffect,

		// DoSpecCom
		Whisper,
		Ask,

		// DoGenCom
		Holler,
		Shout,
		Gossip,
		Auction,
		Gratz,

		// DoShutdown
		Shutdow,
		Shutdown,

		// DoQuit
		Qui,
		Quit,

		// DoDate
		Date,
		UpTime,

		// DoCommands
		Commands,
		Socials,
		WizHelp,

		// DoDrop
		Drop,
		Junk,
		Donate,

		// DoGenWrite
		Bug,
		Typo,
		Idea,

		// DoLook
		Look,
		Read,

		// DoQComm
		QSay,
		QEcho,

		// DoPour
		Pour,
		Fill,

		// DoPoof
		PoofIn,
		PoofOut,

		// DoHit
		Hit,
		Murder,

		// DoEat
		Eat,
		Taste,
		Drink,
		Sip,

		// DoUse
		Use,
		Quaff,
		Recite,

		// DoEcho
		Echo,
		Emote,

		// DoGenDoor
		Open,
		Close,
		Unlock,
		Lock,
		Pick,

		// DoOlc
		REdit,
		OEdit,
		ZEdit,
		MEdit,
		SEdit,
		TEdit,
		SaveInfo,

		Index
	}
}
