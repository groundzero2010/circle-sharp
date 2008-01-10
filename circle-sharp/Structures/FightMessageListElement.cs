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
		public int NumberOfAttacks = 0;
		public List<FightMessageEntry> Messages = new List<FightMessageEntry> ();

		public FightMessageListElement(SpellDefinitions def)
		{
			AttackType = def;
		}
	}
}
