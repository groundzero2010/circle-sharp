using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
	class ObjectFlagData
	{
		public int[] values = new int[4];
		public ItemTypes Type;
		public int MinimumLevel;
		public WearTypes WearFlags;
		public ItemFlags ExtraFlags;
		public int Weight;
		public int Cost;
		public int CostPerDay;
		public int Timer;
		public long Bitvector;
	}
}
