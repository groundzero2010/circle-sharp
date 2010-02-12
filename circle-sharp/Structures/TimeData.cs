using System;
using System.Collections.Generic;
using System.Text;

namespace CircleSharp.Structures
{
    class TimeData
    {
        DateTime _birth;
        DateTime _logon;
        int _played;

        public DateTime Birth
        {
            get { return _birth; }
            set { _birth = value; }
        }

        public DateTime Logon
        {
            get { return _logon; }
            set { _logon = value; }
        }

        public int Played
        {
            get { return _played; }
            set { _played = value; }
        }
    }
}
