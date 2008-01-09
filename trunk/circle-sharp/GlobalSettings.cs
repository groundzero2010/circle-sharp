using System;
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
		
		public string GreetingText = String.Empty;
		public string MenuText = String.Empty;
		
		private string _settingsFile = String.Empty;
		private XmlDocument _settingsDocument = new XmlDocument ();
		
		public void Load (string path)
		{
			_settingsFile = Path.Combine (path, "GlobalSettings.xml");

			_settingsDocument.Load (_settingsFile);

			MortalStartRoom = Int32.Parse (_settingsDocument.DocumentElement["MortalStartRoom"].InnerText);
			ImmortalStartRoom = Int32.Parse (_settingsDocument.DocumentElement["ImmortalStartRoom"].InnerText);
			FrozenStartRoom = Int32.Parse (_settingsDocument.DocumentElement["FrozenStartRoom"].InnerText);

			GreetingText = _settingsDocument.DocumentElement["Greeting"].InnerText;
			MenuText = _settingsDocument.DocumentElement["Menu"].InnerText;
		}

		public void Save ()
		{
			if (_settingsFile == String.Empty)
			{
				GlobalUtilities.Log ("Attempted to save global settings before initial load! Aborting.");
				return;
			}

			_settingsDocument.DocumentElement["MortalStartRoom"].InnerText = MortalStartRoom.ToString ();
			_settingsDocument.DocumentElement["ImmortalStartRoom"].InnerText = ImmortalStartRoom.ToString ();
			_settingsDocument.DocumentElement["FrozenStartRoom"].InnerText = FrozenStartRoom.ToString ();

			_settingsDocument.DocumentElement["Greeting"].InnerText = GreetingText;
			_settingsDocument.DocumentElement["Menu"].InnerText = MenuText;

			_settingsDocument.Save (_settingsFile);
		}
	}
}
