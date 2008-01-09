using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleSharp.Enumerations
{
	[Flags]
	internal enum MagicFlags
	{
		Damage,
		Affects,
		Unaffects,
		Points,
		AlterObjects,
		Groups,
		Masses,
		Areas,
		Summons,
		Creations,
		Manual
	}
}
