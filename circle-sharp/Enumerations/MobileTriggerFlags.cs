using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleSharp.Enumerations
{
	[Flags]
	internal enum MobileTriggerFlags
	{
		Global,
		Random,
		Command,
		Speech,
		Act,
		Death,
		Greet,
		GreetAll,
		Entry,
		Receive,
		Fight,
		HitPercent,
		Bribe,
		Load,
		Memory,
		Cast,
		Leave,
		Door
	}
}
