using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
	public partial class SharpCore
	{
		private void InitializeSpellLevels ()
		{
			// Magic User Spell Levels
			SpellLevel (SpellDefinitions.SpellMagicMissile, ClassTypes.MagicUser, 1);
			SpellLevel (SpellDefinitions.SpellDetectInvisible, ClassTypes.MagicUser, 2);
			SpellLevel (SpellDefinitions.SpellDetectMagic, ClassTypes.MagicUser, 2);
			SpellLevel (SpellDefinitions.SpellChillTouch, ClassTypes.MagicUser, 3);
			SpellLevel (SpellDefinitions.SpellInfravision, ClassTypes.MagicUser, 3);
			SpellLevel (SpellDefinitions.SpellInvisible, ClassTypes.MagicUser, 4);
			SpellLevel (SpellDefinitions.SpellArmor, ClassTypes.MagicUser, 4);
			SpellLevel (SpellDefinitions.SpellBurningHands, ClassTypes.MagicUser, 5);
			SpellLevel (SpellDefinitions.SpellLocateObject, ClassTypes.MagicUser, 6);
			SpellLevel (SpellDefinitions.SpellStrength, ClassTypes.MagicUser, 6);
			SpellLevel (SpellDefinitions.SpellShockingGrasp, ClassTypes.MagicUser, 7);
			SpellLevel (SpellDefinitions.SpellSleep, ClassTypes.MagicUser, 8);
			SpellLevel (SpellDefinitions.SpellLightningBolt, ClassTypes.MagicUser, 9);
			SpellLevel (SpellDefinitions.SpellBlindness, ClassTypes.MagicUser, 9);
			SpellLevel (SpellDefinitions.SpellDetectPoison, ClassTypes.MagicUser, 10);
			SpellLevel (SpellDefinitions.SpellColorSpray, ClassTypes.MagicUser, 11);
			SpellLevel (SpellDefinitions.SpellEnergyDrain, ClassTypes.MagicUser, 13);
			SpellLevel (SpellDefinitions.SpellCurse, ClassTypes.MagicUser, 14);
			SpellLevel (SpellDefinitions.SpellPoison, ClassTypes.MagicUser, 14);
			SpellLevel (SpellDefinitions.SpellFireball, ClassTypes.MagicUser, 15);
			SpellLevel (SpellDefinitions.SpellCharm, ClassTypes.MagicUser, 16);
			SpellLevel (SpellDefinitions.SpellEnchantWeapon, ClassTypes.MagicUser, 26);
			SpellLevel (SpellDefinitions.SpellClone, ClassTypes.MagicUser, 30);

			// Cleric Spell Levels
			SpellLevel (SpellDefinitions.SpellCureLight, ClassTypes.Cleric, 1);
			SpellLevel (SpellDefinitions.SpellArmor, ClassTypes.Cleric, 1);
			SpellLevel (SpellDefinitions.SpellCreateFood, ClassTypes.Cleric, 2);
			SpellLevel (SpellDefinitions.SpellCreateWater, ClassTypes.Cleric, 2);
			SpellLevel (SpellDefinitions.SpellDetectPoison, ClassTypes.Cleric, 3);
			SpellLevel (SpellDefinitions.SpellDetectAlign, ClassTypes.Cleric, 4);
			SpellLevel (SpellDefinitions.SpellCureBlind, ClassTypes.Cleric, 4);
			SpellLevel (SpellDefinitions.SpellBless, ClassTypes.Cleric, 5);
			SpellLevel (SpellDefinitions.SpellDetectInvisible, ClassTypes.Cleric, 6);
			SpellLevel (SpellDefinitions.SpellBlindness, ClassTypes.Cleric, 6);
			SpellLevel (SpellDefinitions.SpellInfravision, ClassTypes.Cleric, 7);
			SpellLevel (SpellDefinitions.SpellProtectFromEvil, ClassTypes.Cleric, 8);
			SpellLevel (SpellDefinitions.SpellPoison, ClassTypes.Cleric, 8);
			SpellLevel (SpellDefinitions.SpellGroupArmor, ClassTypes.Cleric, 9);
			SpellLevel (SpellDefinitions.SpellCureCritic, ClassTypes.Cleric, 9);
			SpellLevel (SpellDefinitions.SpellSummon, ClassTypes.Cleric, 10);
			SpellLevel (SpellDefinitions.SpellRemovePoison, ClassTypes.Cleric, 10);
			SpellLevel (SpellDefinitions.SpellWordOfRecall, ClassTypes.Cleric, 12);
			SpellLevel (SpellDefinitions.SpellEarthquake, ClassTypes.Cleric, 12);
			SpellLevel (SpellDefinitions.SpellDispelEvil, ClassTypes.Cleric, 14);
			SpellLevel (SpellDefinitions.SpellDispelGood, ClassTypes.Cleric, 14);
			SpellLevel (SpellDefinitions.SpellSanctuary, ClassTypes.Cleric, 15);
			SpellLevel (SpellDefinitions.SpellCallLightning, ClassTypes.Cleric, 16);
			SpellLevel (SpellDefinitions.SpellHeal, ClassTypes.Cleric, 17);
			SpellLevel (SpellDefinitions.SpellControlWeather, ClassTypes.Cleric, 18);
			SpellLevel (SpellDefinitions.SpellSenseLife, ClassTypes.Cleric, 19);
			SpellLevel (SpellDefinitions.SpellHarm, ClassTypes.Cleric, 22);
			SpellLevel (SpellDefinitions.SpellGroupHeal, ClassTypes.Cleric, 26);
			SpellLevel (SpellDefinitions.SpellRemoveCurse, ClassTypes.Cleric, 26);

			// Thief Spell Levels
			SpellLevel (SpellDefinitions.SkillSneak, ClassTypes.Thief, 1);
			SpellLevel (SpellDefinitions.SkillPickLock, ClassTypes.Thief, 2);
			SpellLevel (SpellDefinitions.SkillBackstab, ClassTypes.Thief, 3);
			SpellLevel (SpellDefinitions.SkillSteal, ClassTypes.Thief, 4);
			SpellLevel (SpellDefinitions.SkillHide, ClassTypes.Thief, 5);
			SpellLevel (SpellDefinitions.SkillTrack, ClassTypes.Thief, 6);

			// Warrior Spell Level
			SpellLevel (SpellDefinitions.SkillKick, ClassTypes.Warrior, 1);
			SpellLevel (SpellDefinitions.SkillRescue, ClassTypes.Warrior, 3);
			SpellLevel (SpellDefinitions.SkillTrack, ClassTypes.Warrior, 9);
			SpellLevel (SpellDefinitions.SkillBash, ClassTypes.Warrior, 12);
		}
	}
}
