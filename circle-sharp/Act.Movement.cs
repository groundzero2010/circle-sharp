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
		public int[] MovementLoss = new int[] {
			1,
			1,
			2,
			3,
			4,
			6,
			4,
			1,
			1,
			5
		};

		private bool HasBoat (CharacterData character)
		{
			if (character.Level > GlobalConstants.LVL_IMMORT)
				return true;

			if (character.AffectFlagged (AffectFlags.Waterwalk))
				return true;

			foreach (ObjectData obj in character.Inventory)
			{
				if (obj.Type == ObjectTypes.Boat && FindEquipmentPosition (character, obj, null) < WearTypes.None)
					return true;
			}
			
			for (int i = 0; i < (int)WearTypes.Index; i++)
				if (character.Equipment[i] != null && character.Equipment[i].Type == ObjectTypes.Boat)
					return true;

			return false;
		}

		private void DoMove (CharacterData character, string argument, int command, SubCommands subcommand)
		{
			DirectionTypes direction = DirectionTypes.Index;

			switch (subcommand)
			{
				case SubCommands.North:
					direction = DirectionTypes.North;
					break;

				case SubCommands.South:
					direction = DirectionTypes.South;
					break;

				case SubCommands.East:
					direction = DirectionTypes.East;
					break;

				case SubCommands.West:
					direction = DirectionTypes.West;
					break;

				case SubCommands.Up:
					direction = DirectionTypes.Up;
					break;

				case SubCommands.Down:
					direction = DirectionTypes.Down;
					break;
			}
			
			PerformMove(character, direction, false);
		}
		
		private bool PerformMove (CharacterData character, DirectionTypes direction, bool needSpecialsCheck)
		{
			int wasInRoom = 0;

			if (character == null || character.Fighting != null)
				return false;
			else if (GetExit(character, direction) == null || GetExit(character, direction).ToRoom == GlobalConstants.NOWHERE)
				SendToCharacter (character, "Alas, you cannot go that way...\r\n");
			else if (GetExit(character, direction).ExitFlagged(DirectionOptionFlags.Closed))
			{
				if (!String.IsNullOrEmpty(GetExit(character, direction).Keyword))
					SendToCharacter (character, "The " + GlobalUtilities.FirstName(GetExit(character, direction).Keyword) + " seems to be closed.\r\n");
				else
					SendToCharacter (character, "It seems to be closed.\r\n");
			}
			else
			{
				if (!character.HasFollowers)
					return (DoSimpleMove (character, direction, needSpecialsCheck));

				wasInRoom = character.InRoom;

				if (!DoSimpleMove (character, direction, needSpecialsCheck))
					return false;

				foreach (CharacterData follower in character.Followers)
				{
					if ((follower.InRoom == wasInRoom) && follower.Position >= PositionTypes.Standing)
					{
						Act("You follow $N.\r\n", false, follower, null, character, GlobalConstants.TO_CHAR);
						PerformMove (follower, direction, true);
					}
				}

				return true;
			}

			return false;
		}

		private bool DoSimpleMove (CharacterData character, DirectionTypes direction, bool needsSpecialsCheck)
		{
			int needMovement;

			if (needsSpecialsCheck && Special (character, (int)direction + 1, String.Empty))
				return false;

			/* blocked by a leave trigger ? */
			//if (!leave_mtrigger (ch, dir))
			//	return 0;
			//if (!leave_wtrigger (&world[IN_ROOM (ch)], ch, dir))
			//	return 0;

			if (character.AffectFlagged (AffectFlags.Charm) && character.Master != null &&
				character.InRoom == character.Master.InRoom)
			{
				SendToCharacter (character, "The thought of leaving your master makes you weep.\r\n");
				Act("$n bursts into tears.", false, character, null, null, GlobalConstants.TO_ROOM);
				return false;
			}

			if ((GetRoomSector (character.InRoom) == SectorTypes.WaterNoSwim) ||
				GetRoomSector (GetExit (character, direction).ToRoom) == SectorTypes.WaterNoSwim)
			{
				if (!HasBoat (character))
				{
					SendToCharacter (character, "You need a boat to go there.\r\n");
					return false;
				}
			}

			needMovement = (MovementLoss[(int)GetRoomSector (character.InRoom)] +
				MovementLoss[(int)GetRoomSector (GetExit (character, direction).ToRoom)]) / 2;

			if (character.Points.Move < needMovement && !character.IsNPC)
			{
				if (needsSpecialsCheck && character.Master != null)
					SendToCharacter (character, "You are too exhausted to follow.\r\n");
				else
					SendToCharacter (character, "You are too exhausted.\r\n");

				return false;
			}

			if (_rooms[character.InRoom].RoomFlagged (RoomFlags.Atrium))
			{
				if (!HouseCanEnter (character, GetRoomVirtualNumber (GetExit (character, direction).ToRoom)))
				{
					SendToCharacter (character, "That's private property -- no trespassing!\r\n");
					return false;
				}
			}

			if (_rooms[GetExit (character, direction).ToRoom].RoomFlagged (RoomFlags.Tunnel) &&
				NumberOfPCInRoom (_rooms[GetExit (character, direction).ToRoom]) >= GlobalSettings.TunnelSize)
			{
				if (GlobalSettings.TunnelSize > 1)
					SendToCharacter (character, "There isn't enough room for you to go there!\r\n");
				else
					SendToCharacter (character, "There isn't enough room there for more than one person!\r\n");

				return false;
			}

			if (_rooms[GetExit (character, direction).ToRoom].RoomFlagged (RoomFlags.GodRoom) &&
				character.Level < GlobalConstants.LVL_GRGOD)
			{
				SendToCharacter (character, "You aren't godly enough to use that room!\r\n");
				return false;
			}

			if (character.Level < GlobalConstants.LVL_IMMORT && !character.IsNPC)
				character.Points.Move -= needMovement;

			if (!character.AffectFlagged (AffectFlags.Sneak))
			{
				Act("$n leaves "+GlobalConstants.Directions[(int)direction], true, character, null, null, GlobalConstants.TO_ROOM);
			}

			int wasInRoom = character.InRoom;
			CharacterFromRoom (character);
			CharacterToRoom (character, _rooms[wasInRoom].DirectionOptions[(int)direction].ToRoom);

			// Trigger Stuff
			/* move them first, then move them back if they aren't allowed to go. */
			/* see if an entry trigger disallows the move */
			//if (!entry_mtrigger(ch) || !enter_wtrigger(&world[IN_ROOM(ch)], ch, dir))
			//{
			//char_from_room(ch);
			//char_to_room(ch, was_in);
			//return 0;
			//}

			if (!character.AffectFlagged (AffectFlags.Sneak))
				Act ("$n has arrived.", true, character, null, null, GlobalConstants.TO_ROOM);

			if (character.Descriptor != null)
				//LookAtRoom (character, 0);

			if (_rooms[character.InRoom].RoomFlagged (RoomFlags.Death) && character.Level < GlobalConstants.LVL_IMMORT)
			{
				//LogDeathTrap (character);
				//DeathCry (character);
				//ExtractCharacter (character);
				return false;
			}

			//entry_memory_mtrigger (ch);

			//if (!greet_mtrigger (ch, dir))
			//{
			//	char_from_room (ch);
			//	char_to_room (ch, was_in);
			//	look_at_room (ch, 0);
			//}
			//else
			//	greet_memory_mtrigger (ch);

			return true;
		}
	}
}
