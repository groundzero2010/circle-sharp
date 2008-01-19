using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
	public partial class SharpCore
	{
		private RoomDirectionData GetExit(CharacterData character, DirectionTypes direction)
		{
			return _rooms[character.InRoom].DirectionOptions[(int)direction];
		}

		private bool ValidRoomRealNumber (int number)
		{
			return (number != GlobalConstants.NOWHERE && number <= _topOfRoomTable);
		}

		private int GetRoomVirtualNumber (int number)
		{
			return ((ValidRoomRealNumber (number) ? _rooms[number].Number : GlobalConstants.NOWHERE));
		}

		private SpecialFunctionHandler GetRoomSpecial (int number)
		{
			return (ValidRoomRealNumber (number) ? _rooms[number].SpecialFunction : null);
		}

		private bool ValidObjectRealNumber (ObjectData obj)
		{
			return (obj.Number <= _topOfObjectTable && obj.Number != GlobalConstants.NOTHING);
		}

		private int GetObjectVirtualNumber (ObjectData obj)
		{
			return (ValidObjectRealNumber (obj) ? _objectIndex[obj.Number].VirtualNumber : GlobalConstants.NOTHING);
		}

		private SpecialFunctionHandler GetObjectSpecial (ObjectData obj)
		{
			return (ValidObjectRealNumber (obj) ? _objectIndex[obj.Number].SpecialFunction : null);
		}

		private SpecialFunctionHandler GetMobileSpecial (CharacterData character)
		{
			return (character.IsNPC ? _mobileIndex[character.Number].SpecialFunction : null);
		}

		private SectorTypes GetRoomSector (int number)
		{
			return (ValidRoomRealNumber (number) ? _rooms[number].SectorType : SectorTypes.Inside);
		}

		private int NumberOfPCInRoom (RoomData room)
		{
			int count = 0;

			foreach (CharacterData character in room.People)
				if (!character.IsNPC)
					count++;

			return (count);
		}

		private bool SendOK (CharacterData character, int toSleeping)
		{
			return ((character.Descriptor != null || ScriptCheck (character, MTRIG_ACT)) &&
				(toSleeping > 0 || character.Awake) &&
				character.PlayerFlagged (PlayerFlags.Writing));
		}
	}
}
