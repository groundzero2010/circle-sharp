using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleSharp.Structures
{
	class TriggerEvent
	{
		// TODO: Event Function Pointer

		public object EventObject;
		public Queue<TriggerQueueElement> QueueElements;
	}
}
