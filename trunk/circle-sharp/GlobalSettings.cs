﻿using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleSharp
{
	public class GlobalSettings
	{
		public int MortalStartRoom = GlobalConstants.NOWHERE;
		public int ImmortalStartRoom = GlobalConstants.NOWHERE;
		public int FrozenStartRoom = GlobalConstants.NOWHERE;

		public bool AutoPlayerWipe = false;

		public string ListenAddress = "127.0.0.1";
		public int ListenPort = 5000;
		
		private string _settingsFile = String.Empty;
		private XmlDocument _settingsDocument = new XmlDocument ();
		
		public void Load (string path)
		{
			_settingsFile = Path.Combine (path, "GlobalSettings.xml");

			_settingsDocument.Load (_settingsFile);

			MortalStartRoom = Int32.Parse (_settingsDocument.DocumentElement["MortalStartRoom"].InnerText);
			ImmortalStartRoom = Int32.Parse (_settingsDocument.DocumentElement["ImmortalStartRoom"].InnerText);
			FrozenStartRoom = Int32.Parse (_settingsDocument.DocumentElement["FrozenStartRoom"].InnerText);

			AutoPlayerWipe = bool.Parse (_settingsDocument.DocumentElement["AutoPlayerWipe"].InnerText);

			ListenAddress = _settingsDocument.DocumentElement["ListenAddress"].InnerText;
			ListenPort = Int32.Parse (_settingsDocument.DocumentElement["ListenPort"].InnerText);
		}

		public void Save ()
		{
			if (_settingsFile == String.Empty)
				return;

			_settingsDocument.DocumentElement["MortalStartRoom"].InnerText = MortalStartRoom.ToString ();
			_settingsDocument.DocumentElement["ImmortalStartRoom"].InnerText = ImmortalStartRoom.ToString ();
			_settingsDocument.DocumentElement["FrozenStartRoom"].InnerText = FrozenStartRoom.ToString ();

			_settingsDocument.DocumentElement["AutoPlayerWipe"].InnerText = AutoPlayerWipe.ToString ();

			_settingsDocument.DocumentElement["ListenAddress"].InnerText = ListenAddress;
			_settingsDocument.DocumentElement["ListenPort"].InnerText = ListenPort.ToString ();

			_settingsDocument.Save (_settingsFile);
		}
	}
}
