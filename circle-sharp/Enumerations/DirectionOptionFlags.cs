using System;
using System.Collections.Generic;
using System.Text;

namespace CircleSharp.Enumerations
{
	[Flags]
    internal enum DirectionOptionFlags
    {
        None = 0, IsDoor, Closed, Locked, PickProof
    }
}
