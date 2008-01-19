using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
	class CommandInfo
	{
		public string Command;
		public PositionTypes MinimumPosition;
		public event CommandHandler CommandPointer;
		public int MinimumLevel;
		public SubCommands SubCommand;

		public CommandInfo (string command, PositionTypes min_position, CommandHandler pointer, int min_level, SubCommands subcmd)
		{
			Command = command;
			MinimumPosition = min_position;
			CommandPointer = pointer;
			MinimumLevel = min_level;
			SubCommand = subcmd;
		}
	}
}
