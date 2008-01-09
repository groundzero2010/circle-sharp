using System;
using System.Collections.Generic;
using System.Text;

namespace CircleSharp.Structures
{
    class PlayerSpecialData
    {
		public PlayerSpecialDataSaved Saved = new PlayerSpecialDataSaved ();

		public string PoofIn;
		public string PoofOut;
		public List<AliasData> Aliases = new List<AliasData> ();
		public long LastTell;
		public object LastOLCTarget;
		public int LastOLCMode;
		public string Host;
    }
}
