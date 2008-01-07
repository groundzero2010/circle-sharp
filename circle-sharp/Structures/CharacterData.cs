using System;
using System.Collections.Generic;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
    internal class CharacterData
    {
        private int _pfilepos;
        private uint _mobrnum;
        private uint _inroom;
        private uint _wasinroom;
        private int _wait;

        private CharacterPlayerData _player;
        private CharacterAbilityData _realabils;
        private CharacterAbilityData _affabils;
        private CharacterPointData _points;
        private CharacterSpecialData _charspecials;
        private PlayerSpecialData _playerspecials;
        private MobileSpecialData _mobspecials;

        private List<AffectData> _affected;
        private ObjectData[] _equipment = new ObjectData[(int)WearTypes.Index];
        private List<ObjectData> _inventory = new List<ObjectData> ();
        private List<CharacterData> _followers = new List<CharacterData> ();
        private DescriptorData _descriptor;

        private long _id;

//   struct trig_proto_list *proto_script; /* list of default triggers      */
//   struct script_data *script;         /* script info for the object      */
//   struct script_memory *memory;       /* for mob memory triggers         */

        private CharacterData _master;

        public uint MobileRealNumber
        {
            get { return _mobrnum; }
            set { _mobrnum = value; }
        }

        public int Wait
        {
            get { return _wait; }
            set { _wait = value; }
        }

        public uint InRoom
        {
            get { return _inroom; }
            set { _inroom = value; }
        }

        public uint WasInRoom
        {
            get { return _wasinroom; }
            set { _wasinroom = value; }
        }

        public DescriptorData Descriptor
        {
            get { return _descriptor; }
            set { _descriptor = value; }
        }

        public bool IsNPC
        {
            get { return MobileFlagged(MobileFlags.IsNPC); }
        }

        public long IDNumber
        {
            get { return _charspecials.Saved.IDNumber; }
        }

        public List<ObjectData> Inventory
        {
            get { return _inventory; }
        }

        public List<CharacterData> Followers
        {
            get { return _followers; }
        }

        public CharacterData Master
        {
            get { return _master; }
            set { _master = value; }
        }

        public CharacterData Fighting
        {
            get { return _charspecials.Fighting; }
            set { _charspecials.Fighting = value; }
        }

        public CharacterData Hunting
        {
            get { return _charspecials.Hunting; }
            set { _charspecials.Hunting = value; }
        }

        public bool PlayerFlagged(PlayerFlags flag)
        {
            return ((_charspecials.Saved.Flags & (byte)flag) == (byte)flag);
        }

        public void RemovePlayerFlag(PlayerFlags flag)
        {
            _charspecials.Saved.Flags &= ~(byte)flag;
        }

        public void SetPlayerFlag(PlayerFlags flag)
        {
            _charspecials.Saved.Flags = _charspecials.Saved.Flags | (byte)flag;
        }

        public bool MobileFlagged(MobileFlags flag)
        {
            return ((_charspecials.Saved.Flags & (byte)flag) == (byte)flag);
        }

        public void RemoveMobileFlag(MobileFlags flag)
        {
            _charspecials.Saved.Flags &= ~(byte)flag;
        }

        public void SetMobileFlag(MobileFlags flag)
        {
            _charspecials.Saved.Flags = _charspecials.Saved.Flags | (byte)flag;
        }
    }
}
