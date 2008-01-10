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
        private void CommandInterpreter (CharacterData character, string command)
        {
            Console.WriteLine("CommandInterpreter");
        }

        private void Nanny(DescriptorData descriptor, string command)
        {
            Console.WriteLine("Nanny");
        }

		private int FindCommand(string command)
		{
			return 0;
		}
    }
}
