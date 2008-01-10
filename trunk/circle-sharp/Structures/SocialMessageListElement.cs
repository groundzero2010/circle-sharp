using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
	class SocialMessageListElement
	{
		public int ActionNumber;
		public bool Hide;
		public PositionTypes MinimumVictimPosition;

		public string CharacterNoArg;
		public string OthersNoArg;

		public string CharacterFound;
		public string OthersFound;
		public string VictimFound;

		public string NotFound;

		public string CharacterAuto;
		public string OthersAuto;
	}
}
