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

		public bool AutoPlayerWipe = false;
		public bool DeathTrapsAreDumps = false;

		public int ListenPort = 5000;

		public int TunnelSize = 2;

		public int StampPrice = 150;

		public int UserRestriction = 0;

		public long BeginningOfTime = 0;

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
			DeathTrapsAreDumps = bool.Parse (_settingsDocument.DocumentElement["DeathTrapsAreDumps"].InnerText);

			TunnelSize = int.Parse (_settingsDocument.DocumentElement["TunnelSize"].InnerText);
			
			StampPrice = int.Parse(_settingsDocument.DocumentElement["StampPrice"].InnerText);

			ListenPort = Int32.Parse (_settingsDocument.DocumentElement["ListenPort"].InnerText);

			UserRestriction = Int32.Parse(_settingsDocument.DocumentElement["UserRestriction"].InnerText);

			BeginningOfTime = long.Parse(_settingsDocument.DocumentElement["BeginningOfTime"].InnerText);
		}

		public void Save ()
		{
			if (_settingsFile == String.Empty)
				return;

			_settingsDocument.DocumentElement["MortalStartRoom"].InnerText = MortalStartRoom.ToString ();
			_settingsDocument.DocumentElement["ImmortalStartRoom"].InnerText = ImmortalStartRoom.ToString ();
			_settingsDocument.DocumentElement["FrozenStartRoom"].InnerText = FrozenStartRoom.ToString ();

			_settingsDocument.DocumentElement["AutoPlayerWipe"].InnerText = AutoPlayerWipe.ToString ();
			_settingsDocument.DocumentElement["DeathTrapsAreDumps"].InnerText = DeathTrapsAreDumps.ToString ();

			_settingsDocument.DocumentElement["ListenPort"].InnerText = ListenPort.ToString ();

			_settingsDocument.DocumentElement["TunnelSize"].InnerText = TunnelSize.ToString ();
			_settingsDocument.DocumentElement["StampPrice"].InnerText = StampPrice.ToString();
			_settingsDocument.DocumentElement["UserRestriction"].InnerText = UserRestriction.ToString();

			_settingsDocument.DocumentElement["BeginningOfTime"].InnerText = BeginningOfTime.ToString();

			_settingsDocument.Save (_settingsFile);
		}
	}
}
