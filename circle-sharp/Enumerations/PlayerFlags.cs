using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMUD.Enumerations
{
    [Flags]
    internal enum PlayerFlags
    {
        Killer = 0, Thief, Frozen, DontSet, Writing, Mailing, Crash, SiteOK, NoShout, NoTitle, Deleted, LoadRoom,
        NoWizList, NoDelete, InvisibleStart, Cryo, NotDeadYet
    }
}
