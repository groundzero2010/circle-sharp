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

//   struct trig_proto_list *proto_script; /* list of default triggers      */
//   struct script_data *script;         /* script info for the object      */
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
    }
}