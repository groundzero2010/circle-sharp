using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
	public partial class CircleCore
	{
		private Dictionary<SpellDefinitions, SpellData> _spells = new Dictionary<SpellDefinitions, SpellData> ();

		private void MagicAssignSpells ()
		{
			// Define castable spells.

			SpellDefine (SpellDefinitions.SpellAnimateDead, "animate dead", 35, 10, 3, PositionTypes.Standing,
				MagicTargetFlags.ObjectRoom, false, MagicFlags.Summons, String.Empty);

			SpellDefine (SpellDefinitions.SpellArmor, "armor", 30, 15, 3, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom, false, MagicFlags.Affects,
				"You feel less protected.");

			SpellDefine (SpellDefinitions.SpellBless, "bless", 35, 5, 3, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.ObjectInventory, false, MagicFlags.Affects | MagicFlags.AlterObjects,
				"You feel less righteous.");

			SpellDefine (SpellDefinitions.SpellBlindness, "blindness", 35, 25, 1, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.NotSelf, false, MagicFlags.Affects,
				"You feel a cloak of blindness dissolve.");

			SpellDefine (SpellDefinitions.SpellBurningHands, "burning hands", 35, 25, 1, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.FightVictim, true, MagicFlags.Damage, String.Empty);

			SpellDefine (SpellDefinitions.SpellCallLightning, "call lightning", 40, 25, 3, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.FightVictim, true, MagicFlags.Damage, String.Empty);

			SpellDefine (SpellDefinitions.SpellCharm, "charm", 75, 50, 2, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.NotSelf, true, MagicFlags.Manual,
				"You feel more self-confident.");

			SpellDefine (SpellDefinitions.SpellChillTouch, "chill touch", 30, 10, 3, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.FightVictim, true, MagicFlags.Damage | MagicFlags.Affects,
				"You feel your strength return.");

			SpellDefine (SpellDefinitions.SpellClone, "clone", 80, 65, 5, PositionTypes.Standing,
				MagicTargetFlags.SelfOnly, false, MagicFlags.Summons, String.Empty);

			SpellDefine (SpellDefinitions.SpellColorSpray, "color spray", 30, 15, 3, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.FightVictim, true, MagicFlags.Damage, String.Empty);

			SpellDefine (SpellDefinitions.SpellControlWeather, "control weather", 75, 25, 5, PositionTypes.Standing,
				MagicTargetFlags.Ignore, false, MagicFlags.Manual, String.Empty);

			SpellDefine (SpellDefinitions.SpellCreateFood, "create food", 30, 5, 4, PositionTypes.Standing,
				MagicTargetFlags.Ignore, false, MagicFlags.Creations, String.Empty);

			SpellDefine (SpellDefinitions.SpellCreateWater, "create water", 30, 5, 4, PositionTypes.Standing,
				MagicTargetFlags.Ignore, false, MagicFlags.Creations, String.Empty);

			SpellDefine (SpellDefinitions.SpellCureBlind, "cure blind", 30, 5, 2, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom, false, MagicFlags.Unaffects, String.Empty);

			SpellDefine (SpellDefinitions.SpellCureCritic, "cure critical", 30, 10, 2, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom, false, MagicFlags.Points, String.Empty);

			SpellDefine (SpellDefinitions.SpellCureLight, "cure light", 30, 10, 2, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom, false, MagicFlags.Points, String.Empty);

			SpellDefine (SpellDefinitions.SpellCurse, "curse", 80, 50, 2, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.ObjectInventory, true, MagicFlags.Affects | MagicFlags.AlterObjects,
				"You feel more optimistic.");

			SpellDefine (SpellDefinitions.SpellDetectAlign, "detect alignment", 20, 10, 2, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.SelfOnly, false, MagicFlags.Affects,
				"You feel less aware.");

			SpellDefine (SpellDefinitions.SpellDetectInvisible, "detect invisibility", 20, 10, 2, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.SelfOnly, false, MagicFlags.Affects,
				"Your eyes stop tingling.");

			SpellDefine (SpellDefinitions.SpellDetectMagic, "detect magic", 20, 10, 2, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.SelfOnly, false, MagicFlags.Affects,
				"The detect magic wears off.");

			SpellDefine (SpellDefinitions.SpellDetectPoison, "detect poison", 15, 5, 1, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.ObjectInventory | MagicTargetFlags.ObjectRoom, false, MagicFlags.Manual,
				"The detect poison wears off.");

			SpellDefine (SpellDefinitions.SpellDispelEvil, "dispel evil", 40, 25, 3, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.FightVictim, true, MagicFlags.Damage, String.Empty);

			SpellDefine (SpellDefinitions.SpellDispelGood, "dispel good", 40, 25, 3, PositionTypes.Fighting,
							MagicTargetFlags.CharacterRoom | MagicTargetFlags.FightVictim, true, MagicFlags.Damage, String.Empty);

			SpellDefine (SpellDefinitions.SpellEarthquake, "earthquake", 40, 25, 3, PositionTypes.Fighting,
				MagicTargetFlags.Ignore, true, MagicFlags.Areas, String.Empty);

			SpellDefine (SpellDefinitions.SpellEnchantWeapon, "enchant weapon", 150, 100, 10, PositionTypes.Standing,
				MagicTargetFlags.ObjectInventory, false, MagicFlags.Manual, String.Empty);

			SpellDefine (SpellDefinitions.SpellEnergyDrain, "energy drain", 40, 25, 1, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.FightVictim, true, MagicFlags.Damage | MagicFlags.Manual, String.Empty);

			SpellDefine (SpellDefinitions.SpellGroupArmor, "group armor", 50, 30, 2, PositionTypes.Standing,
				MagicTargetFlags.Ignore, false, MagicFlags.Groups, String.Empty);

			SpellDefine (SpellDefinitions.SpellFireball, "fireball", 40, 30, 2, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.FightVictim, true, MagicFlags.Damage, String.Empty);

			SpellDefine (SpellDefinitions.SpellGroupHeal, "group heal", 80, 60, 5, PositionTypes.Standing,
				MagicTargetFlags.Ignore, false, MagicFlags.Groups, String.Empty);

			SpellDefine (SpellDefinitions.SpellHarm, "harm", 75, 45, 3, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.FightVictim, true, MagicFlags.Damage, String.Empty);

			SpellDefine (SpellDefinitions.SpellHeal, "heal", 60, 40, 3, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom, false, MagicFlags.Points | MagicFlags.Unaffects, String.Empty);

			SpellDefine (SpellDefinitions.SpellInfravision, "infravision", 25, 10, 1, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.SelfOnly, false, MagicFlags.Affects,
				"Your night vision seems to fade.");

			SpellDefine (SpellDefinitions.SpellInvisible, "invisibility", 35, 25, 1, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.ObjectInventory | MagicTargetFlags.ObjectRoom, false, MagicFlags.Affects | MagicFlags.AlterObjects,
				"You feel yourself exposed.");

			SpellDefine (SpellDefinitions.SpellLightningBolt, "lightning bolt", 30, 15, 1, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.FightVictim, true, MagicFlags.Damage, String.Empty);

			SpellDefine (SpellDefinitions.SpellLocateObject, "locate object", 25, 20, 1, PositionTypes.Standing,
				MagicTargetFlags.ObjectWorld, false, MagicFlags.Manual, String.Empty);

			SpellDefine (SpellDefinitions.SpellMagicMissile, "magic missile", 25, 10, 3, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.FightVictim, true, MagicFlags.Damage, String.Empty);
			
			SpellDefine (SpellDefinitions.SpellPoison, "poison", 50, 20, 3, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.NotSelf | MagicTargetFlags.ObjectInventory, true, MagicFlags.Affects | MagicFlags.AlterObjects,
				"You feel less sick.");

			SpellDefine (SpellDefinitions.SpellProtectFromEvil, "protection from evil", 40, 10, 3, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.SelfOnly, false, MagicFlags.Affects,
				"You feel less protected.");

			SpellDefine (SpellDefinitions.SpellRemoveCurse, "remove curse", 45, 25, 5, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.ObjectInventory | MagicTargetFlags.ObjectEquip, false,
				MagicFlags.Unaffects | MagicFlags.AlterObjects, String.Empty);

			SpellDefine (SpellDefinitions.SpellRemovePoison, "remove poison", 40, 8, 4, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.ObjectInventory | MagicTargetFlags.ObjectRoom, false,
				MagicFlags.Unaffects | MagicFlags.AlterObjects, String.Empty);

			SpellDefine (SpellDefinitions.SpellSanctuary, "sanctuary", 110, 85, 5, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom, false, MagicFlags.Affects,
				"The white aura around yoru body fades.");

			SpellDefine (SpellDefinitions.SpellSenseLife, "sense life", 20, 10, 2, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.SelfOnly, false, MagicFlags.Affects,
				"You feel less aware of your surroundings.");

			SpellDefine (SpellDefinitions.SpellShockingGrasp, "shocking grasp", 30, 15, 3, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.FightVictim, true, MagicFlags.Damage, String.Empty);

			SpellDefine (SpellDefinitions.SpellSleep, "sleep", 40, 25, 5, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom, true, MagicFlags.Affects,
				"You feel less tired.");

			SpellDefine (SpellDefinitions.SpellStrength, "strength", 35, 30, 1, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom, false, MagicFlags.Affects,
				"You feel weaker.");

			SpellDefine (SpellDefinitions.SpellSummon, "summon", 75, 50, 3, PositionTypes.Standing,
				MagicTargetFlags.CharacterWorld | MagicTargetFlags.NotSelf, false, MagicFlags.Manual, String.Empty);

			SpellDefine (SpellDefinitions.SpellTeleport, "teleport", 75, 50, 3, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom, false, MagicFlags.Manual, String.Empty);

			SpellDefine (SpellDefinitions.SpellWaterwalk, "waterwalk", 40, 20, 2, PositionTypes.Standing,
				MagicTargetFlags.CharacterRoom, false, MagicFlags.Affects,
				"Your feet seem less buoyant.");

			SpellDefine (SpellDefinitions.SpellWordOfRecall, "word of recall", 20, 10, 2, PositionTypes.Fighting,
				MagicTargetFlags.CharacterRoom, false, MagicFlags.Manual, String.Empty);

			// Define non-castable spells.

			SpellDefine (SpellDefinitions.SpellIdentify, "identify", 0, 0, 0, PositionTypes.Dead,
				MagicTargetFlags.CharacterRoom | MagicTargetFlags.ObjectInventory | MagicTargetFlags.ObjectRoom, false, MagicFlags.Manual, String.Empty);
			
			// Define spells that are currently not used or implemented and not castable.
			// Values for the "breath" spells are filled in assuming a dragon's breath.

			SpellDefine (SpellDefinitions.SpellFireBreath, "fire breath", 0, 0, 0, PositionTypes.Sitting,
				MagicTargetFlags.Ignore, true, MagicFlags.Damage, String.Empty);

			SpellDefine (SpellDefinitions.SpellGasBreath, "gas breath", 0, 0, 0, PositionTypes.Sitting,
				MagicTargetFlags.Ignore, true, MagicFlags.Damage, String.Empty);

			SpellDefine (SpellDefinitions.SpellFrostBreath, "frost breath", 0, 0, 0, PositionTypes.Sitting,
				MagicTargetFlags.Ignore, true, MagicFlags.Damage, String.Empty);

			SpellDefine (SpellDefinitions.SpellAcidBreath, "acid breath", 0, 0, 0, PositionTypes.Sitting,
				MagicTargetFlags.Ignore, true, MagicFlags.Damage, String.Empty);

			SpellDefine (SpellDefinitions.SpellLightningBreath, "lightning breath", 0, 0, 0, PositionTypes.Sitting,
				MagicTargetFlags.Ignore, true, MagicFlags.Damage, String.Empty);

			// Spell defined for use with triggers.

			SpellDefine (SpellDefinitions.SpellAffect, "script-inflicted", 0, 0, 0, PositionTypes.Sitting,
				MagicTargetFlags.Ignore, true, MagicFlags.Damage, String.Empty);
			
			// Define skills.

			SkillDefine (SpellDefinitions.SkillBackstab, "backstab");
			SkillDefine (SpellDefinitions.SkillBash, "bash");
			SkillDefine (SpellDefinitions.SkillHide, "hide");
			SkillDefine (SpellDefinitions.SkillKick, "kick");
			SkillDefine (SpellDefinitions.SkillPickLock, "pick lock");
			SkillDefine (SpellDefinitions.SkillRescue, "rescue");
			SkillDefine (SpellDefinitions.SkillSneak, "sneak");
			SkillDefine (SpellDefinitions.SkillSteal, "steal");
			SkillDefine (SpellDefinitions.SkillTrack, "track");
		}

		private void SkillDefine (SpellDefinitions def, string name)
		{
			SpellDefine(def, name, 0, 0, 0, PositionTypes.Dead, MagicTargetFlags.Ignore, false, MagicFlags.Affects, String.Empty);
		}

		private void SpellDefine (SpellDefinitions def, string name, int maxMana, int minMana, int manaChange, PositionTypes position,
			MagicTargetFlags targets, bool violent, MagicFlags routines, string wearOffMessage)
		{
			SpellData spell = new SpellData ();

			spell.Name = name;
			spell.ManaMaximum = maxMana;
			spell.ManaMinimum = minMana;
			spell.ManaChange = manaChange;
			spell.MinimumPosition = position;
			spell.Targets = targets;
			spell.Violent = violent;
			spell.Routines = routines;
			spell.WearOffMessage = wearOffMessage;

			for (int i = 0; i < (int)ClassTypes.Index; i++)
				spell.MinimumLevel[i] = GlobalConstants.LVL_IMMORT;

			_spells.Add (def, spell);
		}

		private string SkillName (SpellDefinitions def)
		{
			return (_spells[def].Name);
		}

		private void SpellLevel (SpellDefinitions def, ClassTypes clas, int level)
		{
			bool bad = false;

			if (level < 1 || level > GlobalConstants.LVL_IMPL)
			{
				Log ("SYSERR: Assigning '" + SkillName (def) + "' to illegal level " + level + "/" + GlobalConstants.LVL_IMPL + ".");
				bad = true;
			}

			if (!bad)
			{
				_spells[def].MinimumLevel[(int)clas] = level;
			}
		}
	}
}
