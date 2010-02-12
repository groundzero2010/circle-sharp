using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
	public partial class CircleCore
	{
		private WearTypes FindEquipmentPosition (CharacterData character, ObjectData obj, string arg)
		{
			WearTypes where = WearTypes.Index;

			string[] keywords = new string[] {
				"!RESERVED!",
				"finger",
				"!RESERVED!",
				"neck",
				"!RESERVED!",
				"body",
				"head",
				"legs",
				"feet",
				"hands",
				"arms",
				"shield",
				"about",
				"waist",
				"wrist",
				"!RESERVED!",
				"!RESERVED!",
				"!RESERVED!",
				"\n"
			};

			if (String.IsNullOrEmpty (arg))
			{
				if (obj.WearFlagged (ObjectWearFlags.Finger)) where = WearTypes.RightFinger;
				if (obj.WearFlagged (ObjectWearFlags.Neck)) where = WearTypes.Neck1;
				if (obj.WearFlagged (ObjectWearFlags.Body)) where = WearTypes.Body;
				if (obj.WearFlagged (ObjectWearFlags.Head)) where = WearTypes.Head;
				if (obj.WearFlagged (ObjectWearFlags.Legs)) where = WearTypes.Legs;
				if (obj.WearFlagged (ObjectWearFlags.Feet)) where = WearTypes.Feet;
				if (obj.WearFlagged (ObjectWearFlags.Hands)) where = WearTypes.Hands;
				if (obj.WearFlagged (ObjectWearFlags.Arms)) where = WearTypes.Arms;
				if (obj.WearFlagged (ObjectWearFlags.Shield)) where = WearTypes.Shield;
				if (obj.WearFlagged (ObjectWearFlags.About)) where = WearTypes.About;
				if (obj.WearFlagged (ObjectWearFlags.Waist)) where = WearTypes.Waist;
				if (obj.WearFlagged (ObjectWearFlags.Wrist)) where = WearTypes.RightWrist;
			}
			else if (((where = (WearTypes)GlobalUtilities.SearchBlock(arg, keywords, false)) < 0) || (String.IsNullOrEmpty(arg) && arg[0] == '!'))
			{
				SendToCharacter (character, "'%s'? What part of your body is THAT?\r\n", arg);
				return WearTypes.None;
			}
			
			return (where);
		}
	}
}
