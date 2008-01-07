using System;
using System.Collections.Generic;
using System.Text;

namespace CircleSharp.Enumerations
{
    [Flags]
    internal enum MobileFlags
    {
        Special = 0, Sentinel, Scavenger, IsNPC, Aware, Aggressive, StayZone, Wimpy, AggressiveEvil, AggressiveGood,
        AggressiveNeutral, Memory, Helper, NoCharm, NoSummon, NoSleep, NoBash, NoBlind, NotDeadYet
    }
}
