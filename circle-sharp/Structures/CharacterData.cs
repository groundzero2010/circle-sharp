using System;
using System.Collections.Generic;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
    internal class CharacterData
    {
		public int Number;
		public int InRoom;
		public int WasInRoom;
		public int Wait;

		public CharacterPlayerData Player = new CharacterPlayerData ();
		public CharacterAbilityData RealAbilities = new CharacterAbilityData ();
		public CharacterAbilityData AffectedAbilities = new CharacterAbilityData ();
		public CharacterPointData Points = new CharacterPointData ();
		public CharacterSpecialData CharacterSpecials = new CharacterSpecialData ();
		public PlayerSpecialData PlayerSpecials = new PlayerSpecialData ();
		public MobileSpecialData MobileSpecials = new MobileSpecialData ();

		public List<AffectData> Affected = new List<AffectData> ();
		public ObjectData[] Equipment = new ObjectData[(int)WearTypes.Index];
		public List<ObjectData> Inventory = new List<ObjectData> ();
		public List<CharacterData> Followers = new List<CharacterData> ();
		public DescriptorData Descriptor;

		public long ID;

		public ScriptData Script;
//   struct trig_proto_list *proto_script; /* list of default triggers      */
//   struct script_memory *memory;       /* for mob memory triggers         */

        public CharacterData Master;

        public bool IsNPC
        {
            get { return MobileFlagged(MobileFlags.IsNPC); }
        }

        public long IDNumber
        {
            get { return CharacterSpecials.Saved.IDNumber; }
        }

		public PositionTypes Position
		{
			get { return CharacterSpecials.Position; }
		}

		public bool HasFollowers
		{
			get
			{
				if (Followers != null && Followers.Count > 0)
					return true;
				else
					return false;
			}
		}

		public bool Awake
		{
			get { return (this.Position > PositionTypes.Sleeping); }
		}

		public int Level
		{
			get { return Player.Level; }
		}

		public int RealLevel
		{
			get
			{
				return (Descriptor != null && Descriptor.Original != null ? Descriptor.Original.Level : Level);
			}
		}

		public int InvisLevel
		{
			get
			{
				return (this.PlayerSpecials.Saved.InvisibleLevel);
			}
		}

        public CharacterData Fighting
        {
			get
			{
				return CharacterSpecials.Fighting;
			}
			set
			{
				CharacterSpecials.Fighting = value;
			}
        }

        public CharacterData Hunting
        {
			get
			{
				return CharacterSpecials.Hunting;
			}
			set
			{
				CharacterSpecials.Hunting = value;
			}
        }

		public bool AffectFlagged (AffectFlags flag)
		{
			return ((CharacterSpecials.Saved.AffectedBy & (byte)flag) == (byte)flag);
		}

		public void RemoveAffectFlag (AffectFlags flag)
		{
			CharacterSpecials.Saved.AffectedBy &= ~(byte)flag;
		}

		public void SetAffectFlag (AffectFlags flag)
		{
			CharacterSpecials.Saved.AffectedBy = CharacterSpecials.Saved.AffectedBy | (byte)flag;
		}

        public bool PlayerFlagged(PlayerFlags flag)
        {
			return ((CharacterSpecials.Saved.Flags & (byte)flag) == (byte)flag);
        }

        public void RemovePlayerFlag(PlayerFlags flag)
        {
			CharacterSpecials.Saved.Flags &= ~(byte)flag;
        }

        public void SetPlayerFlag(PlayerFlags flag)
        {
			CharacterSpecials.Saved.Flags = CharacterSpecials.Saved.Flags | (byte)flag;
        }

        public bool MobileFlagged(MobileFlags flag)
        {
			return ((CharacterSpecials.Saved.Flags & (byte)flag) == (byte)flag);
        }

        public void RemoveMobileFlag(MobileFlags flag)
        {
			CharacterSpecials.Saved.Flags &= ~(byte)flag;
        }

        public void SetMobileFlag(MobileFlags flag)
        {
			CharacterSpecials.Saved.Flags = CharacterSpecials.Saved.Flags | (byte)flag;
        }

		public bool PreferenceFlagged (PreferenceFlags flag)
		{
			return ((PlayerSpecials.Saved.Preferences & (byte)flag) == (byte)flag);
		}

		public void RemovePreferenceFlag (PreferenceFlags flag)
		{
			PlayerSpecials.Saved.Preferences &= ~(byte)flag;
		}

		public void SetPreferenceFlag (PreferenceFlags flag)
		{
			PlayerSpecials.Saved.Preferences = PlayerSpecials.Saved.Preferences | (byte)flag;
		}

		public void Clear()
		{
			InRoom = GlobalConstants.NOWHERE;
			Number = GlobalConstants.NOBODY;
			WasInRoom = GlobalConstants.NOWHERE;
			Position = PositionTypes.Standing;
			MobileSpecials.DefaultPosition = PositionTypes.Standing;

			Points.ArmorClass = 100;

			if (Points.MaxMana < 100)
				Points.MaxMana = 100;
		}
    }
}
