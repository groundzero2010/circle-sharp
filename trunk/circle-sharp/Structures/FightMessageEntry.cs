using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleSharp.Structures
{
	class FightMessageEntry
	{
		public FightMessage DieMessage = new FightMessage ();
		public FightMessage MissMessage = new FightMessage ();
		public FightMessage HitMessage = new FightMessage ();
		public FightMessage GodMessage = new FightMessage ();
	}
}
