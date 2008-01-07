using System;
using System.Collections.Generic;
using System.Text;

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

		public List<ExtraDescriptionData> ExtraDescriptions = new List<ExtraDescriptionData> ();
		public ObjectFlagData ObjectFlags;
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
