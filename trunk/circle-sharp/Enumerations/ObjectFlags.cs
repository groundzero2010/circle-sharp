using System;
using System.Collections.Generic;
using System.Text;

namespace CircleSharp.Enumerations
{
	[Flags]
    internal enum ObjectFlags
    {
        Glow = 0, Hum, NoRent, NoDonate, NoInvisible, Invisible, Magic, NoDrop, Bless, AntiGood, AntiEvil, AntiNeutral,
        AntiMagicUser, AntiCleric, AntiThief, AntiWarrior, NoSell
    }
}
