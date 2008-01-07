using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
	class RoomData
	{
		public int Number;
		public int Zone;
		public SectorTypes SectorType;
		public string Name;
		public string Description;
		public List<ExtraDescriptionData> ExtraDescriptions = new List<ExtraDescriptionData> ();
		public RoomDirectionData[] DirectionOptions = new RoomDirectionData[(int)DirectionTypes.Index];
		public RoomFlags Flags;

		public List<int> Triggers;
		public ScriptData Script;

		public int Light;

		// TODO: Special Function Pointer.

		public List<ObjectData> Contents;
		public List<CharacterData> People;
	}
}
