using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
	class ScriptData
	{
		public TriggerTypes Types;
		public List<TriggerData> Triggers;
		public List<TriggerVariableData> Variables;
		public bool Purged;
		public long Context;
	}
}
