using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleSharp.Enumerations
{
	[Flags]
	internal enum PlayerIndexFlags : long
	{
		Deleted, NoDelete, SelfDelete
	}
}
