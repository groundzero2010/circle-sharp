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
		private bool PuffSpecial (CharacterData character, object me, int command, string argument)
		{
			return false;
		}

		private bool DumpSpecial (CharacterData character, object me, int command, string argument)
		{
			return false;
		}
	}
}
