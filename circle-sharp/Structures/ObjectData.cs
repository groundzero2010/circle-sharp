using System;
using System.Collections.Generic;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
    class ObjectData
    {
		public int ItemNumber;
		public int InRoom;
		public string Name;
		public string Description;
		public string ShortDescription;
		public string ActionDescription;

		public int[] Values = new int[4];
		public ObjectTypes Type;
		public ObjectWearFlags WearFlags;
		public ObjectFlags ObjectFlags;
		public int MinimumLevel;
		public int Weight;
		public int Cost;
		public int CostPerDay;
		public int Timer;
		public long Bitvector;

		public List<ExtraDescriptionData> ExtraDescriptions = new List<ExtraDescriptionData> ();
		public List<ObjectAffectData> Affects = new List<ObjectAffectData> ();
		
		public CharacterData CarriedBy;
		public CharacterData WornBy;
		public int WornOn;
		
		public ObjectData InObject;
		public List<ObjectData> Contains;

		public long TriggerID;
		// trig_proto_list
		public ScriptData Script;
   }
}
