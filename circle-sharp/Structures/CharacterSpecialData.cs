using System;
using System.Collections.Generic;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
    class CharacterSpecialData
    {
        public CharacterData Fighting;
		public CharacterData Hunting;

		public PositionTypes Position;

		public int CarryWeight;
		public int CarryItems;
		public int Timer;

		public CharacterSpecialDataSaved Saved = new CharacterSpecialDataSaved ();
    }

    class CharacterSpecialDataSaved
    {
        public int Alignment;
		public long IDNumber;

		public long Flags;
		public long AffectedBy;

		public short[] ApplySavingThrow = new short[5];
    }
}
