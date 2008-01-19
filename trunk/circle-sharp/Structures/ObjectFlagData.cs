using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
	class ObjectFlagData
	{
		public int[] Values = new int[4];
		public ObjectTypes Type;
		public long WearFlags;
		public long ObjectFlags;
		public int MinimumLevel;
		public int Weight;
		public int Cost;
		public int CostPerDay;
		public int Timer;
		public long Bitvector;
	}
}
