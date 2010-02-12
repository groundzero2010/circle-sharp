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
		private List<BanListElement> _bans = new List<BanListElement>();

		internal bool LoadBanned()
		{
			string filename = "BanSites.xml";
			XmlDocument xmlFile = null;

			try
			{
				xmlFile = new XmlDocument();
				xmlFile.Load(Path.Combine(Path.Combine(_baseDirectory, GlobalConstants.LIB_ETC), filename));
			}
			catch (Exception e)
			{
				Log("SYSERR: Unable to load ban sites file: " + filename);
				Log("SYSERR: Exception: " + e.Message);
				return false;
			}

			XmlNodeList list = xmlFile.GetElementsByTagName("BanListElement");

			foreach (XmlNode node in list)
			{
				BanListElement element = new BanListElement();

				try
				{
					element.Type = (BanTypes)int.Parse(node.Attributes["Type"].Value);
					element.Date = DateTime.Parse(node.Attributes["Type"].Value);
					element.Name = node.Attributes["Name"].Value;
					element.Site = node.InnerText;
				}
				catch
				{
					Log("SYSERR: Error pasing XML for ban list item: " + filename);
					return false;
				}

				_bans.Add(element);
			}

			return true;
		}

		internal bool SaveBanned()
		{
			return true;
		}

		internal void ReadInvalidList()
		{

		}

		private bool ValidName(string name)
		{
			foreach (DescriptorData descriptor in _descriptors)
			{
				if (descriptor.Character != null && descriptor.Character.GetName() != null && descriptor.Character.GetName().CompareTo(name) != 0)
					if (descriptor.Character.IDNumber == -1)
						return descriptor.Character.IsPlaying;

				int vowels = 0;

				for (int i = 0; i < name.Length; i++)
				{
					switch (name[i])
					{
						case 'a':
						case 'A':
						case 'e':
						case 'E':
						case 'i':
						case 'I':
						case 'o':
						case 'O':
						case 'u':
						case 'U':
							vowels++;
							break;
					}
				}

				if (vowels < 1)
					return false;

				// TODO: Check invalid list names
				return true;
			}

			return true;
		}

		internal BanTypes IsBanned(string hostname)
		{
			BanTypes banType = BanTypes.Not;

			if (String.IsNullOrEmpty(hostname))
				return BanTypes.Not;

			foreach (BanListElement ban in _bans)
			{
				if (String.IsNullOrEmpty(ban.Site))
					if (ban.Site.IndexOf(hostname.ToLower()) > -1)
						if (banType > ban.Type)
							banType = ban.Type;
			}

			return banType;
		}

	}
}