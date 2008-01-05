using System;
using System.Collections.Generic;
using System.Text;

using SharpMUD.Enumerations;

namespace SharpMUD.Structures
{
    class CharacterSpecialData
    {
        private CharacterData _fighting;
        private CharacterData _hunting;

        private PositionTypes _position;

        private int _carryWeight;
        private int _carryItems;
        private int _timer;

        private CharacterSpecialDataSaved _saved;

        public CharacterData Fighting
        {
            get { return _fighting; }
            set { _fighting = value; }
        }

        public CharacterData Hunting
        {
            get { return _hunting; }
            set { _hunting = value; }
        }

        public PositionTypes Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public CharacterSpecialDataSaved Saved
        {
            get { return _saved; }
            set { _saved = value; }
        }
    }

    class CharacterSpecialDataSaved
    {
        private int _alignment;
        private long _idnum;

        private long _flags;
        private long _affectedby;

        private short[] _applysavingthrow = new short[5];

        public long Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        public long IDNumber
        {
            get { return _idnum; }
            set { _idnum = value; }
        }
    }
}
