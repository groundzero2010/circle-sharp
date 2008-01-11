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
		public long Flags;

		public List<int> Triggers;
		public ScriptData Script;

		public int Light;

		public SpecialFunctionHandler SpecialFunction;

		public List<ObjectData> Contents;
		public List<CharacterData> People;

		public bool RoomFlagged (RoomFlags flag)
		{
			return ((Flags & (byte)flag) == (byte)flag);
		}
	}
}
