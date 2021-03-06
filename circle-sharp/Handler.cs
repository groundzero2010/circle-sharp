using System;
using System.Collections.Generic;
using System.Text;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
    public partial class CircleCore
    {
        private int _extractionsPending = 0;

        private void ExtractPendingCharacters()
        {
            //if (_extractionsPending < 0)
                //Log("SYSERR: Negative (%d) extractions pending.", _extractionsPending);

            foreach (CharacterData character in _characters)
            {
                if (character.MobileFlagged(MobileFlags.NotDeadYet))
                    character.RemoveMobileFlag(MobileFlags.NotDeadYet);
                else if (character.PlayerFlagged(PlayerFlags.NotDeadYet))
                    character.RemovePlayerFlag(PlayerFlags.NotDeadYet);

                ExtractCharacterFinal(character);
                _extractionsPending--;
            }

            //if (_extractionsPending > 0)
                //Log("SYSERR: Couldn't find %d extractions as counted.", _extractionsPending);

            _extractionsPending = 0;
        }

        private void ExtractCharacterFinal(CharacterData character)
        {
            if (character.InRoom == GlobalConstants.NOWHERE)
            {
                //Log("SYSERR: NOWHERE extracting char %s. (ExtractCharFinal)", character.GetName());
                return;
            }

            if (!character.IsNPC && character.Descriptor == null)
            {
                foreach (DescriptorData descriptor in _descriptors)
                {
                    if (descriptor.Original == character)
                    {
                        //DoReturn(descriptor.Character, null, 0, 0);
                        break;
                    }
                }
            }

            if (character.Descriptor != null)
            {
                //if (character.Descriptor.Original != null)
                    //DoReturn(character, null, 0, 0);
                //else
                {
                    foreach (DescriptorData descriptor in _descriptors)
                    {
                        if (descriptor == character.Descriptor)
                            continue;
                        if (descriptor.Character != null && character.IDNumber == descriptor.Character.IDNumber)
                            descriptor.ConnectState = ConnectState.Close;
                    }

                    character.Descriptor.ConnectState = ConnectState.Menu;
                    WriteToOutput(character.Descriptor, _textMenu);
                }
            }

            // On with the character's assets.

            //if (character.Followers.Count > 0 || character.Master != null)
                //DieFollower(character);

            // Transfer objects to room, if any.
            foreach (ObjectData obj in character.Inventory)
            {
                //ObjectFromCharacter(obj);
                //ObjectToRoom(obj, character.InRoom);
            }

            // Transfer equipment to room, if any.
            //for (int i = 0; i < GlobalConstants.NUM_WEARS; i++)
                //if (character.GetEQ(i) != null)
                    //ObjectToRoom(UnEquipCharacter(character, i), character.InRoom);

            //if (character.Fighting != null)
                //StopFighting(character);

            //foreach (CharacterData fighter in _combatters)
                //if (fighter.Fighting == character)
                    //StopFighting(fighter);

            foreach (CharacterData hunter in _characters)
                if (hunter.Hunting == character)
                    hunter.Hunting = null;

            //CharacterFromRoom(character);

            if (character.IsNPC)
            {
                //if (character.MobileRealNumber != GlobalConstants.NOTHING)
                    //_mobileIndex[character.MobileRealNumber].Number--;

                //ClearMemory(character);

                //if (character.Script != null)
                    //ExtractScript(character.Script);
                //if (character.ScriptMemory != null)
                    //ExtractScriptMemory(character.ScriptMemory);
            }
            else
            {
                //SaveCharacter(character);
                //CrashDeleteCrashFile(character);
            }

            //if (character.IsNPC || character.Descriptor != null)
                //FreeCharacter(character);
        }

		private void CharacterFromRoom (CharacterData character)
		{
			if (character == null || character.InRoom == GlobalConstants.NOWHERE)
			{
				Log ("SYSERR: Null character or NOWHERE in CharacterFromRoom()");
				return;
			}

			//if (character.Fighting != null)
				//StopFighting (character);

			if (character.Equipment[(int)WearTypes.Light] != null)
				if (character.Equipment[(int)WearTypes.Light].Type == ObjectTypes.Light)
					if (character.Equipment[(int)WearTypes.Light].Flags.Values[2] > 0)
						_rooms[character.InRoom].Light--;

			_rooms[character.InRoom].People.Remove (character);
			character.InRoom = GlobalConstants.NOWHERE;
		}

		private void CharacterToRoom (CharacterData character, int number)
		{
			if (character == null || number == GlobalConstants.NOWHERE || number > _topOfRoomTable)
			{
				Log ("SYSERR: Illegal values passed to CharacterToRoom.");
				return;
			}

			_rooms[number].People.Add(character);
			character.InRoom = number;

			if (character.Equipment[(int)WearTypes.Light] != null)
				if (character.Equipment[(int)WearTypes.Light].Type == ObjectTypes.Light)
					if (character.Equipment[(int)WearTypes.Light].Flags.Values[2] > 0)
						_rooms[number].Light++;

			if (character.Fighting != null && character.InRoom != character.Fighting.InRoom)
			{
				//StopFighting (character.Fighting);
				//StopFighting (character);
			}
		}
    }
}
