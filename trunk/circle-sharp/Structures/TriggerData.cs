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

		public List<CommandListElement> Commands = new List<CommandListElement> ();
		public CommandListElement Current;

		public int NumericalArgument;
		public string ArgumentList;
		public int Depth;
		public int Loops;

		public TriggerEvent WaitEvent;
		public bool Purged;
		public List<TriggerVariableData> Variables = new List<TriggerVariableData> ();
	}
}
