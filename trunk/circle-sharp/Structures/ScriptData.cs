using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
	class ScriptData
	{
		public long Types;
		public List<TriggerData> Triggers;
		public List<TriggerVariableData> Variables;
		public bool Purged;
		public long Context;

		public bool TypeFlagged (long flag)
		{
			return ((Types & (byte)flag) == (byte)flag);
		}

		public void RemoveTypeFlag (long flag)
		{
			Types &= ~(byte)flag;
		}

		public void SetTypeFlag (long flag)
		{
			Types = Types | (byte)flag;
		}
	}
}
