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

        internal string HisHer(CharacterData character)
        {
            switch (character.Player.Sex)
            {
                case SexTypes.Male:
                    return "his";
                case SexTypes.Female:
                    return "her";
                case SexTypes.Neutral:
                    return "it's";
            }

            return "<ERROR>";
        }

        internal string HeShe(CharacterData character)
        {
            switch (character.Player.Sex)
            {
                case SexTypes.Male:
                    return "he";
                case SexTypes.Female:
                    return "she";
                case SexTypes.Neutral:
                    return "it";
            }

            return "<ERROR>";
        }

        internal string HimHer(CharacterData character)
        {
            switch (character.Player.Sex)
            {
                case SexTypes.Male:
                    return "him";
                case SexTypes.Female:
                    return "her";
                case SexTypes.Neutral:
                    return "it";
            }

            return "<ERROR>";
        }

        internal string AnA(ObjectData obj)
        {
            switch (obj.Name[0])
            {
                case 'a':
                case 'A':
                case 'e':
                case 'E':
                case 'i':
                case 'I':
                case 'o':
                case 'O':
                case 'u':
                case 'U':
                    return "An";
                default:
                    return "A";
            }
        }

        internal string SAnA(ObjectData obj)
        {
            switch (obj.Name[0])
            {
                case 'a':
                case 'A':
                case 'e':
                case 'E':
                case 'i':
                case 'I':
                case 'o':
                case 'O':
                case 'u':
                case 'U':
                    return "an";
                default:
                    return "a";
            }
        }

		private int NumberOfPCInRoom (RoomData room)
		{
			int count = 0;

			foreach (CharacterData character in room.People)
				if (!character.IsNPC)
					count++;

			return (count);
		}

        private bool ScriptCheck(CharacterData character, byte flag)
        {
            return character.Script != null && character.Script.TypeFlagged(flag);
        }

		private bool SendOK (CharacterData character, int toSleeping)
		{
            return ((character.Descriptor != null || ScriptCheck(character, (byte)MobileScriptTypes.Act)) && (toSleeping > 0 || character.Position > PositionTypes.Sleeping) && character.PlayerFlagged (PlayerFlags.Writing));
		}

        internal bool CanSee(CharacterData character, ObjectData target)
        {
            return false;
        }

        internal bool CanSee(CharacterData character, CharacterData target)
        {
            return (character != target || (character.GetRealLevel() >= (target.IsNPC ? 0 : target.GetInvisLevel())) && ImmortalCanSee(character, target));
        }

        internal bool CanSeeObject(CharacterData character, ObjectData obj)
        {
            return (MortalCanSeeObject(character, obj) || (!character.IsNPC && character.PreferenceFlagged(PreferenceFlags.HolyLight)));
        }

        internal bool CanSeeObjectCarrier(CharacterData character, ObjectData obj)
        {
            return obj.CarriedBy != null || CanSee(character, obj.CarriedBy) && (obj.WornBy != null || !CanSee(character, obj.WornBy));
        }

        internal bool MortalCanSeeObject(CharacterData character, ObjectData obj)
        {
            return LightOK(character) && InvisibleOKObject(character, obj) && CanSeeObjectCarrier(character, obj);
        }

        internal bool MortalCanSee(CharacterData character, CharacterData target)
        {
            return (LightOK(character) && InvisibleOK(character, target));
        }

        internal bool ImmortalCanSee(CharacterData character, CharacterData target)
        {
            return MortalCanSee(character, target) || (!character.IsNPC && character.PreferenceFlagged(PreferenceFlags.HolyLight));
        }

        internal bool InvisibleOK(CharacterData character, CharacterData target)
        {
            return !target.AffectFlagged(AffectFlags.Invisible) || target.AffectFlagged(AffectFlags.DetectInvisibility) && (!target.AffectFlagged(AffectFlags.Hide) || target.AffectFlagged(AffectFlags.SenseLife));
        }

        internal bool InvisibleOKObject(CharacterData character, ObjectData obj)
        {
            return !obj.ObjectFlagged(ObjectFlags.Invisible) || character.AffectFlagged(AffectFlags.DetectInvisibility);
        }

        internal bool LightOK(CharacterData target)
        {
            return IsLight(target.InRoom) || target.AffectFlagged(AffectFlags.Infravision);
        }

        internal bool IsLight(int room)
        {
            return !IsDark(room);
        }

        internal bool IsDark(int room)
        {
            if (!ValidRoomRealNumber(room))
            {
                Log("RoomIsDark-> Invalid room number "+room+". (0-"+_topOfRoomTable+")");
                return false;
            }

            if (_rooms[room].Light > 0)
                return false;

            if (_rooms[room].RoomFlagged(RoomFlags.Dark))
                return true;

            if (GetRoomSector(room) == SectorTypes.Inside || GetRoomSector(room) == SectorTypes.City)
                return false;

            if (_weatherInfo.Sun == SunState.Set || _weatherInfo.Sun == SunState.Dark)
                return true;

            return false;
        }

        internal string PersonString(CharacterData character, CharacterData victim)
        {
            return CanSee(victim, character) ? character.GetName() : "someone";
        }

        internal string ObjectString(ObjectData obj, CharacterData victim)
        {
            return CanSeeObject(victim, obj) ? obj.ShortDescription : "something";
        }

        internal string ObjectName(ObjectData obj, CharacterData victim)
        {
            return CanSeeObject(victim, obj) ? GlobalUtilities.FirstName(obj.Name) : "something";
        }
	}
}
