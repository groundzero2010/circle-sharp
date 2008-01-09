using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
	class SpellData
	{
		public PositionTypes MinimumPosition;
		public int ManaMinimum;
		public int ManaMaximum;
		public int ManaChange;
		public int[] MinimumLevel = new int[(int)ClassTypes.Index];
		public MagicFlags Routines;
		public bool Violent;
		public MagicTargetFlags Targets;
		public string Name;
		public string WearOffMessage;
	}
}
