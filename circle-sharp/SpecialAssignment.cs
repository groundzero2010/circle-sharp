using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
	internal delegate bool SpecialFunctionHandler (CharacterData character, object me, int command, string argument);
	
	public partial class SharpCore
	{
		private void AssignMobile (int virtualNumber, SpecialFunctionHandler special)
		{
			int realNumber = RealMobile (virtualNumber);

			if (realNumber != GlobalConstants.NOBODY)
				_mobileIndex[realNumber].SpecialFunction += special;
			else if (!_miniMud)
				Log ("SYSERR: Attempt to assign special function to a non-existant mobile #"+ virtualNumber);
		}

		private void AssignObject (int virtualNumber, SpecialFunctionHandler special)
		{
			int realNumber = RealObject (virtualNumber);

			if (realNumber != GlobalConstants.NOTHING)
				_objectIndex[realNumber].SpecialFunction += special;
			else if (!_miniMud)
				Log ("SYSERR: Attempt to assign special function to a non-existant object #" + virtualNumber);
		}

		private void AssignRoom (int virtualNumber, SpecialFunctionHandler special)
		{
			int realNumber = RealRoom (virtualNumber);

			if (realNumber != GlobalConstants.NOWHERE)
				_rooms[realNumber].SpecialFunction += special;
			else if (!_miniMud)
				Log ("SYSERR: Attempt to assign special function to a non-existant room #" + virtualNumber);
		}

		private void AssignMobiles ()
		{
			AssignMobile (1, PuffSpecial);
		}

		private void AssignObjects ()
		{
		}

		private void AssignRooms ()
		{
			if (GlobalSettings.DeathTrapsAreDumps)
				for (int i = 0; i < _topOfRoomTable; i++)
					if (_rooms[i].RoomFlagged (RoomFlags.Death))
						_rooms[i].SpecialFunction += DumpSpecial;
		}

		private void AssignShopkeepers ()
		{

		}
	}
}
