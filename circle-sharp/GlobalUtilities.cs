using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace CircleSharp
{
    internal static class GlobalUtilities
    {
        public static int Dice(int number, int size)
        {
            int sum = 0;

            if (size <= 0 || number <= 0)
                return 0;

            while (number-- > 0)
                sum += new Random().Next(1, size);

            return (sum);
        }

        public static string Capital(string text)
        {
            return text[0].ToString().ToUpper() + text.Substring(1).ToLower();
        }

		public static string FirstName (string text)
		{
			if (String.IsNullOrEmpty (text))
				return String.Empty;

			return (text.Substring (0, text.IndexOf (' ')));
		}

		public static string ReadString (TextReader reader)
		{
			string line = reader.ReadLine ();
			string final = String.Empty;

			while (line.IndexOf ('~') < 0)
			{
				final += line + "\n";
				line = reader.ReadLine();
			}

			if (line.IndexOf('~') > 0)
				final += line.Substring(0, line.IndexOf('~'));

			return final;
		}

		public static int SearchBlock (string arg, string[] list, bool exact)
		{
			return 0;
		}
    }
}
