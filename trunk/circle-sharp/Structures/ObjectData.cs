using System;
using System.Collections.Generic;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
    class ObjectData
    {
		public int Number;
		public int InRoom;
		public string Name;
		public string Description;
		public string ShortDescription;
		public string ActionDescription;

		public ObjectFlagData Flags = new ObjectFlagData ();

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

		public ObjectTypes Type
		{
			get { return Flags.Type; }
			set { Flags.Type = value; }
		}

		public bool WearFlagged (ObjectWearFlags flag)
		{
			return ((Flags.WearFlags & (byte)flag) == (byte)flag);
		}

		public void RemoveWearFlag (ObjectWearFlags flag)
		{
			Flags.WearFlags &= ~(byte)flag;
		}

		public void SetWearFlag (ObjectWearFlags flag)
		{
			Flags.WearFlags = Flags.WearFlags | (byte)flag;
		}

		public bool ObjectFlagged (ObjectFlags flag)
		{
			return ((Flags.ObjectFlags & (byte)flag) == (byte)flag);
		}

		public void RemoveObjectFlag (ObjectFlags flag)
		{
			Flags.ObjectFlags &= ~(byte)flag;
		}

		public void SetObjectFlag (ObjectFlags flag)
		{
			Flags.ObjectFlags = Flags.ObjectFlags | (byte)flag;
		}
   }
}
