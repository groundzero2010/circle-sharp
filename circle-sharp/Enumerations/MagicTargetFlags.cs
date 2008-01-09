using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleSharp.Enumerations
{
	[Flags]
	internal enum MagicTargetFlags
	{
		Ignore,
		CharacterRoom,
		CharacterWorld,
		FightSelf,
		FightVictim,
		SelfOnly,
		NotSelf,
		ObjectInventory,
		ObjectRoom,
		ObjectWorld,
		ObjectEquip
	}
}
