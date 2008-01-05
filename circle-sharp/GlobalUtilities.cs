using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMUD
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

        public static void Log(string text)
        {
            Console.WriteLine(text);
        }
    }
}
