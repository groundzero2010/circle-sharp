using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleSharp.Structures
{
	class ZoneData
	{
		public int Number;
		public string Name;
		public int Lifespan;
		public int Age;
		public int Bottom;
		public int Top;
		public int ResetMode;
		public Dictionary<int, ResetCommand> Commands = new Dictionary<int, ResetCommand>();
	}
}
