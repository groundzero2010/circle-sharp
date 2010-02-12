using System;
using System.Collections.Generic;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
    class CharacterPlayerData
    {
		public string Password;
		public string Name;
		public string ShortDescription;
		public string LongDescription;
		public string Description;
		public string Title;
		public SexTypes Sex;
		public ClassTypes Class;
		public RaceTypes Race;
		public int Level;
		public int Hometown;
		public int Age;
		public int Weight;
		public int Height;
        public TimeData Time = new TimeData();
    }
}
