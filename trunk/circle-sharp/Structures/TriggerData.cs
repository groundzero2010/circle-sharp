using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleSharp.Structures
{
	class TriggerData
	{
		public int Number;
		public byte AttachType;
		public byte DataType;
		public string Name;
		public long Type;

		public List<CommandListElement> _commands;
		public CommandListElement _current;

		public int NumericalArgument;
		public string ArgumentList;
		public int Depth;
		public int Loops;

		public TriggerEvent _waitEvent;
		public bool _purged;
		public List<TriggerVariableData> _variables;
	}
}
