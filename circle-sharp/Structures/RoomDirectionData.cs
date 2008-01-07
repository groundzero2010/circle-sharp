using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CircleSharp.Enumerations;

namespace CircleSharp.Structures
{
	class RoomDirectionData
	{
		private string _description;
		private string _keyword;
		private DirectionOptionFlags _exitInfo;
		private int _key;
		private int _toRoom;

		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
			}
		}

		public string Keyword
		{
			get
			{
				return _keyword;
			}
			set
			{
				_keyword = value;
			}
		}

		public DirectionOptionFlags ExitInfo
		{
			get
			{
				return _exitInfo;
			}
			set
			{
				_exitInfo = value;
			}
		}

		public int Key
		{
			get
			{
				return _key;
			}
			set
			{
				_key = value;
			}
		}

		public int ToRoom
		{
			get
			{
				return _toRoom;
			}
			set
			{
				_toRoom = value;
			}
		}
	}
}
