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

		private bool MobileScriptCheck (CharacterData character, MobileTriggerFlags flag)
		{
			return (character.Script != null && character.Script.TypeFlagged ((long)flag));
		}

		// Utilities used for CanSee function

		private bool RoomIsDark (int number)
		{
			if (!ValidRoomRealNumber (number))
			{
				Log("RoomIsDark: Invalid room number "+number+". (0-"+_topOfRoomTable);
				return false;
			}

			if (_rooms[number].Light > 0)
				return false;

			if (_rooms[number].RoomFlagged(RoomFlags.Dark))
				return true;

			if (_rooms[number].SectorType == SectorTypes.Inside || _rooms[number].SectorType == SectorTypes.City)
				return false;

			if (_weatherInfo.Sun == SunState.Set || _weatherInfo.Sun == SunState.Dark)
				return true;

			return false;
		}

		private bool RoomIsLight (int number)
		{
			return (!RoomIsDark (number));
		}

		private bool LightOK (CharacterData character)
		{
			return (!character.AffectFlagged (AffectFlags.Blind) && RoomIsLight (character.InRoom) || character.AffectFlagged(AffectFlags.Infravision));
		}

		private bool InvisOK (CharacterData character, CharacterData victim)
		{
			return ((!victim.AffectFlagged (AffectFlags.Invisible) || character.AffectFlagged (AffectFlags.DetectInvisibility)) &&
				(victim.AffectFlagged (AffectFlags.Hide) || character.AffectFlagged (AffectFlags.SenseLife)));
		}

		private bool MortalCanSee (CharacterData character, CharacterData victim)
		{
			return (LightOK (character) && InvisOK (character, victim));
		}

		private bool ImmortalCanSee (CharacterData character, CharacterData victim)
		{
			return ((MortalCanSee (character, victim) || (!character.IsNPC && character.PreferenceFlagged(PreferenceFlags.HolyLight))));
		}

		private bool CanSee (CharacterData character, CharacterData victim)
		{
			return (character == victim || (character.RealLevel >= (victim.IsNPC ? 0 : victim.InvisLevel)) && ImmortalCanSee (character, victim));
		}

		// Utilities used for Act function.

		private bool SendOK (CharacterData character, int toSleeping)
		{
			return ((character.Descriptor != null || MobileScriptCheck (character, MobileTriggerFlags.Act)) &&
				(toSleeping > 0 || character.Awake) &&
				character.PlayerFlagged (PlayerFlags.Writing));
		}

		private bool InvisOKObject (CharacterData character, ObjectData obj)
		{
			return (!obj.ObjectFlagged (ObjectFlags.Invisible) || character.AffectFlagged (AffectFlags.DetectInvisibility));
		}
		
		private bool CanSeeObjectCarrier (CharacterData character, ObjectData obj)
		{
			return ((obj.CarriedBy != null || CanSee (character, obj.CarriedBy)) &&
				(obj.WornBy != null || CanSee (character, obj.WornBy)));
		}

		private bool MortalCanSeeObject (CharacterData character, ObjectData obj)
		{
			return (LightOK (character) && InvisOKObject (character, obj) && CanSeeObjectCarrier (character, obj));
		}

		private bool CanSeeObject (CharacterData character, ObjectData obj)
		{
			return (MortalCanSeeObject (character, obj) || (!character.IsNPC && character.PreferenceFlagged (PreferenceFlags.HolyLight)));
		}

		private string PersonName (CharacterData character, CharacterData victim)
		{
			return (CanSee (character, victim) ? character.Player.Name : "someone");
		}
		
		private string ObjectDescription (ObjectData obj, CharacterData victim)
		{
			return (CanSeeObject (victim, obj) ? obj.ShortDescription : "something");
		}

		private string ObjectName (ObjectData obj, CharacterData victim)
		{
			return (CanSeeObject (victim, obj) ? GlobalUtilities.FirstName(obj.Name) : "something");
		}

		private string HisHer (CharacterData character)
		{
			return (character.Player.Sex  == SexTypes.Neutral ? (character.Player.Sex == SexTypes.Male ? "his" : "her") : "its");
		}

		private string HeShe (CharacterData character)
		{
			return (character.Player.Sex == SexTypes.Neutral ? (character.Player.Sex == SexTypes.Male ? "he" : "she") : "it");
		}
		
		private string HimHer (CharacterData character)
		{
			return (character.Player.Sex == SexTypes.Neutral ? (character.Player.Sex == SexTypes.Male ? "him" : "her") : "it");
		}
		
		private string AnA (ObjectData obj)
		{
			string vowels = "aeiouAEIOU";

			return (vowels.IndexOf (obj.Name[0]) >= 0 ? "An" : "A");
		}

		private string SAnA (ObjectData obj)
		{
			string vowels = "aeiouAEIOU";

			return (vowels.IndexOf (obj.Name[0]) >= 0 ? "an" : "a");
		}
	}
}
