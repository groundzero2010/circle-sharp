using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
	class HouseData
	{
		public int VirtualNumber;
		public int AtriumNumber;
		public int ExitNumber;
		public DateTime BuiltOn;
		public HouseTypes Type;
		public long Owner;
		public int NumberOfGuests;
		public long[] Guests = new long[GlobalConstants.MAX_GUESTS];
		public DateTime LastPayment;
	}
}
