using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
	public partial class CircleCore
	{
		private void DoReturn(CharacterData character, string argument, int command, SubCommands subcommand)
		{
			if (character.Descriptor != null && character.Descriptor.Original != null)
			{
				SendToCharacter(character, "You return to your original body.\r\n");

				if (character.Descriptor.Original.Descriptor != null)
				{
					character.Descriptor.Original.Descriptor.Character = null;
					character.Descriptor.Original.Descriptor.ConnectState = ConnectState.Disconnect;
				}

				character.Descriptor.Character = character.Descriptor.Original;
				character.Descriptor.Original = null;

				character.Descriptor.Character.Descriptor = character.Descriptor;
				character.Descriptor = null;
			}
		}
	}
}