using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleSharp.Structures
{
	class IndexData
	{
		public int VirtualNumber;
		public int Count;
		public event SpecialFunctionHandler SpecialFunction;
		public string SpecialArguments;
		public TriggerData Trigger;
	}
}
