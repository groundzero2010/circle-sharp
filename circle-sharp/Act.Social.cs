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
		private Dictionary<string, SocialMessageListElement> _socialMessages = new Dictionary<string, SocialMessageListElement> ();

		public void BootSocialMessages()
		{
			XmlDocument file = new XmlDocument ();
			string filename = Path.Combine (_baseDirectory, GlobalConstants.LIB_MISC);
			filename = Path.Combine (filename, "SocialMessages.xml");

			file.Load (filename);
			
			XmlNodeList list = file.GetElementsByTagName ("SocialMessage");

			foreach (XmlNode node in list)
			{
				try
				{
					string action = node.Attributes["ID"].Value;

					SocialMessageListElement social = new SocialMessageListElement ();

					social.ActionNumber = FindCommand (action);
					social.Hide = bool.Parse (node["Hide"].InnerText);
					social.MinimumVictimPosition = (PositionTypes)Int32.Parse (node["MinimumPosition"].InnerText);

					social.CharacterNoArg = node["CharacterNoArg"].InnerText;
					social.OthersNoArg = node["OthersNoArg"].InnerText;
					social.CharacterFound = node["CharacterFound"].InnerText;
					social.OthersFound = node["OthersFound"].InnerText;
					social.VictimFound = node["VictimFound"].InnerText;
					social.NotFound = node["NotFound"].InnerText;
					social.CharacterAuto = node["CharacterAuto"].InnerText;
					social.OthersAuto = node["OthersAuto"].InnerText;

					_socialMessages.Add (action, social);
				}
				catch
				{
					Log ("SYSERR: Format error in XML for message file: " + filename);
				}
			}
		}
	}
}
