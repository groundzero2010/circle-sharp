using System;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Net.Sockets;
using System.Collections.Generic;

using CircleSharp.Structures;
using CircleSharp.Enumerations;

namespace CircleSharp
{
	public partial class CircleCore
	{
		private Dictionary<long, List<MailData>> _mail = new Dictionary<long, List<MailData>>();

		internal bool LoadMail()
		{
			XmlDocument file = new XmlDocument();
			string filename = Path.Combine(Path.Combine(_baseDirectory, GlobalConstants.LIB_ETC), "Mail.xml");

			try
			{
				file.Load(filename);
			}
			catch
			{
				SaveMail();

				file.Load(filename);
			}

			XmlNodeList list = file.GetElementsByTagName("MailData");
			int count = 0;

			foreach (XmlNode node in list)
			{
				MailData mail = new MailData();
				mail.To = long.Parse(node.Attributes["To"].Value);
				mail.From = long.Parse(node.Attributes["From"].Value);
				mail.MailTime = DateTime.Parse(node.Attributes["MailTime"].Value);
				mail.Content = node.InnerText;
				
				if (!_mail.ContainsKey(mail.To))
					_mail.Add(mail.To, new List<MailData>());
				
				_mail[mail.To].Add(mail);
				count++;
			}

			Log("  "+count+" messages loaded.");

			return true;
		}

		internal void SaveMail()
		{
			XmlDocument file = new XmlDocument();
			XmlElement root;
			string filename = Path.Combine(Path.Combine(_baseDirectory, GlobalConstants.LIB_ETC), "Mail.xml");

			file.AppendChild(file.CreateXmlDeclaration("1.0", "UTF-8", "yes"));

			root = file.CreateElement("Mail");

			foreach (List<MailData> mailbox in _mail.Values)
			{
				foreach (MailData mail in mailbox)
				{
					XmlNode mailNode = file.CreateElement("MailData");

					XmlAttribute toAttribute = file.CreateAttribute("To");
					toAttribute.Value = mail.To.ToString();
					mailNode.Attributes.Append(toAttribute);

					XmlAttribute fromAttribute = file.CreateAttribute("From");
					fromAttribute.Value = mail.From.ToString();
					mailNode.Attributes.Append(fromAttribute);

					XmlAttribute dateAttribute = file.CreateAttribute("Date");
					dateAttribute.Value = mail.MailTime.ToString();
					mailNode.Attributes.Append(dateAttribute);

					mailNode.Attributes.Append(toAttribute);
					mailNode.Attributes.Append(fromAttribute);
					mailNode.Attributes.Append(dateAttribute);

					mailNode.InnerText = mail.Content;

					root.AppendChild(mailNode);
				}
			}

			file.AppendChild(root);

			file.Save(filename);
		}

		internal bool HasMail(long idNumber)
		{
			if (_mail.ContainsKey(idNumber))
			{
				if (_mail[idNumber].Count > 0)
					return true;
				else
					return false;
			}

			return false;
		}

		internal void StoreMail(long from, long to, string content)
		{
			MailData mail = new MailData();
			mail.To = to;
			mail.From = from;
			mail.Content = content;
			mail.MailTime = DateTime.Now;

			if (!_mail.ContainsKey(to))
				_mail.Add(to, new List<MailData>());

			_mail[to].Add(mail);
		}

		internal MailData RetreiveMail(long recipient)
		{
			if (_mail.ContainsKey(recipient))
			{
				if (_mail[recipient].Count > 0)
				{
					MailData mail = _mail[recipient][0];
					_mail[recipient].Remove(mail);
					return mail;
				}
				else return null;
			}
			else return null;
		}
	}
}