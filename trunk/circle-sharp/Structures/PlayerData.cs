using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
	class PlayerData
	{
		public string Name;
		public long ID;
		public int Level;
		public long Flags;
		public DateTime Last;

		public bool IsFlagged (PlayerIndexFlags flag)
		{
			return ((Flags & (byte)flag) == (byte)flag);
		}
	}
}
