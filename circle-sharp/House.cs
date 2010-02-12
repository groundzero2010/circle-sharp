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
		private Dictionary<int, HouseData> _houses = new Dictionary<int, HouseData> ();
		private int _topOfHouseTable = 0;

		private int FindHouse (int virtualNumber)
		{
			for (int i = 0; i < _topOfHouseTable; i++)
				if (_houses[i].VirtualNumber == virtualNumber)
					return (i);

			return (GlobalConstants.NOWHERE);
		}

		private bool HouseCanEnter (CharacterData character, int house)
		{
			int houseNumber = FindHouse(house);

			if (character.Level >= GlobalConstants.LVL_GRGOD || houseNumber == GlobalConstants.NOWHERE)
				return true;

			switch (_houses[houseNumber].Type)
			{
				case HouseTypes.Private:
					if (character.IDNumber == _houses[houseNumber].Owner)
						return true;

					for (int i = 0; i < _houses[houseNumber].NumberOfGuests; i++)
						if (character.IDNumber == _houses[houseNumber].Guests[i])
							return true;
					break;
			}

			return false;
		}
	}
}
