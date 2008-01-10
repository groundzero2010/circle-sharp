using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
	class FightMessageListElement
	{
		public SpellDefinitions AttackType;
		public int NumberOfAttacks;
		public List<FightMessageEntry> Messages = new List<FightMessageEntry> ();
	}
}
