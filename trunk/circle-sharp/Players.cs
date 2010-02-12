using System;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
    public partial class CircleCore
    {
		private Dictionary<int, PlayerData> _players = new Dictionary<int, PlayerData>();
		private int _topOfPlayerTable = 0;

		private void BuildPlayerIndex()
		{
			XmlDocument file = new XmlDocument();
			string filename = Path.Combine(_baseDirectory, GlobalConstants.LIB_PLAYERS);
			filename = Path.Combine(filename, "Players.xml");

			file.Load(filename);

			XmlNodeList list = file.GetElementsByTagName("PlayerData");

			foreach (XmlNode node in list)
			{
				PlayerData player = new PlayerData();
				player.ID = 0;

				try
				{
					player.ID = Int32.Parse(node.Attributes["ID"].Value);

					foreach (XmlNode child in node.ChildNodes)
					{
						switch (child.Name)
						{
							case "Name":
								player.Name = child.InnerText;
								break;

							case "Level":
								player.Level = Int32.Parse(child.InnerText);
								break;

							case "Flags":
								player.Flags = long.Parse(child.InnerText);
								break;

							case "Last":
								player.Last = DateTime.Parse(child.InnerText);
								break;
						}
					}
				}
				catch (Exception e)
				{
					Log("SYSERR: Error parsing XML for player [" + player.ID + "] in file: " + filename);
					return;
				}

				_players.Add(_topOfPlayerTable++, player);
			}
		}

		private void AddPlayerValue(XmlDocument file, XmlNode node, string name, string value)
		{
			XmlElement element = file.CreateElement(name);
			XmlText text = file.CreateTextNode(value);
			element.AppendChild(text);
			node.AppendChild(element);
		}

		private void SavePlayerIndex()
		{
			XmlDocument file = new XmlDocument();
			XmlElement root;
			string filename = Path.Combine(_baseDirectory, GlobalConstants.LIB_PLAYERS);

			filename = Path.Combine(filename, "Players.xml");

			file.AppendChild(file.CreateXmlDeclaration("1.0", "UTF-8", "yes"));

			root = file.CreateElement("Players");

			foreach (PlayerData player in _players.Values)
			{
				XmlNode playerNode = file.CreateElement("PlayerData");

				XmlAttribute idAttribute = file.CreateAttribute("ID");
				idAttribute.Value = player.ID.ToString();
				playerNode.Attributes.Append(idAttribute);

				AddPlayerValue(file, playerNode, "Name", player.Name);
				AddPlayerValue(file, playerNode, "Level", player.Level.ToString());
				AddPlayerValue(file, playerNode, "Flags", player.Flags.ToString());
				AddPlayerValue(file, playerNode, "Last", player.Last.ToString("G"));

				root.AppendChild(playerNode);
			}

			file.AppendChild(root);

			file.Save(filename);
		}

		private void CleanPlayerIndex()
		{
			List<int> toRemove = new List<int>();

			// FIXME: This will not do the same thing the CircleMUD code does, which is to remove
			// the character based on level and time. For now this will suffice.
			foreach (int key in _players.Keys)
			{
				PlayerData player = _players[key];

				if (player.IsFlagged(PlayerIndexFlags.NoDelete))
					continue;

				if (player.IsFlagged(PlayerIndexFlags.Deleted))
					toRemove.Add(key);
				else
				{
					// Find out how many days have passed since the player last logged in.
					TimeSpan span = DateTime.Now.Subtract(player.Last);

					if (player.IsFlagged(PlayerIndexFlags.SelfDelete) && span.Days >= 10)
						toRemove.Add(key);
					else if (player.Level == 1 && span.Days >= 1)
						toRemove.Add(key);
					else if ((player.Level > 1 && player.Level < GlobalConstants.LVL_IMMORT) && span.Days >= 30)
						toRemove.Add(key);
				}
			}

			foreach (int key in toRemove)
			{
				Log("WARNING: Removing player [" + _players[key].Name + "] from file.");
				_players.Remove(key);
			}

			SavePlayerIndex();
		}

		internal bool SaveCharacter(CharacterData character)
		{
			return true;
		}

        internal bool LoadCharacter(string name, CharacterData character)
        {

            return false;
        }

		internal bool FreeCharacter(CharacterData character)
		{
			return true;
		}

        internal bool RemovePlayer(CharacterData character)
        {
			int toRemove = -1;

			foreach (int key in _players.Keys)
			{
				PlayerData player = _players[key];
				if (player.Name.CompareTo(character.Player.Name) == 0)
				{
					Log("PCLEAN: " + player.Name + " Lev: " + player.Level + " Last: " + player.Last.ToLongDateString());
					toRemove = key;
					break;
				}
			}

			if (toRemove > -1)
			{
				_players.Remove(toRemove);

				SavePlayerIndex();

				return true;
			}

			Log("ERROR: Unable to find player index to remove for: " + character.GetName());

			return false;
        }
    }
}