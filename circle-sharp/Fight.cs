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
		private Dictionary<SpellDefinitions, FightMessageListElement> _fightMessages = new Dictionary<SpellDefinitions, FightMessageListElement> ();

		private void LoadFightMessages ()
		{
			XmlDocument file = new XmlDocument ();
			string filename = Path.Combine (_baseDirectory, GlobalConstants.LIB_MISC);
			filename = Path.Combine (filename, "FightMessages.xml");

			file.Load (filename);
			
			XmlNodeList list = file.GetElementsByTagName ("FightMessage");

			foreach (XmlNode node in list)
			{
				try
				{
					SpellDefinitions def = (SpellDefinitions)Int32.Parse (node.Attributes["ID"].Value);

					if (!_fightMessages.ContainsKey (def))
						_fightMessages.Add (def, new FightMessageListElement (def));

					_fightMessages[def].NumberOfAttacks++;

					FightMessageEntry message = new FightMessageEntry ();
					message.DieMessage.AttackerMessage = node["DieAttacker"].InnerText;
					message.DieMessage.VictimMessage = node["DieVictim"].InnerText;
					message.DieMessage.RoomMessage = node["DieRoom"].InnerText;

					message.MissMessage.AttackerMessage = node["MissAttacker"].InnerText;
					message.MissMessage.VictimMessage = node["MissVictim"].InnerText;
					message.MissMessage.RoomMessage = node["MissRoom"].InnerText;

					message.HitMessage.AttackerMessage = node["HitAttacker"].InnerText;
					message.HitMessage.VictimMessage = node["HitVictim"].InnerText;
					message.HitMessage.RoomMessage = node["HitRoom"].InnerText;

					message.GodMessage.AttackerMessage = node["GodAttacker"].InnerText;
					message.GodMessage.VictimMessage = node["GodVictim"].InnerText;
					message.GodMessage.RoomMessage = node["GodRoom"].InnerText;

					_fightMessages[def].Messages.Add (message);
				}
				catch
				{
					Log ("SYSERR: Format error in XML for message file: " + filename);
				}
			}
		}
	}
}
