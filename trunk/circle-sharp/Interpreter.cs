using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

using SharpMUD.Structures;
using SharpMUD.Enumerations;

namespace SharpMUD
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
    }
}
