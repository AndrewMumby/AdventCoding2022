using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day25
    {
        public static string A(string input)
        {
            string[] lines = input.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            long total = 0;
            foreach(string line in lines)
            {
                total = total + SNAFU.Decode(line);
                Console.WriteLine(line + " " + SNAFU.Decode(line));
            }
            return SNAFU.Encode(total);
        }
    }

    internal class SNAFU
    {
        internal static long Decode(string numberString)
        {
            long total = 0;
            for (int i = 0; i < numberString.Length; i++)
            {
                switch (numberString[i])
                {
                    case '=':
                        total = total - 2;
                        break;
                    case '-':
                        total = total - 1;
                        break;
                    case '0':
                        break;
                    case '1':
                        total = total + 1;
                        break;
                    case '2':
                        total = total + 2;
                        break;
                }
                total = total * 5;
            }
            total = total / 5;
            return total;
        }

        internal static string Encode(long number)
        {
            if (number == 0)
            {
                return "";
            }
            long div = (number+2) / 5;
            long mod = (number+2) % 5;
            switch (mod)
            {
                case 0:
                    return Encode(div) + "=";
                case 1:
                    return Encode(div) + "-";
                case 2:
                    return Encode(div) + "0";
                case 3:
                    return Encode(div) + "1";
                case 4:
                    return Encode(div) + "2";
            }
            throw new Exception("Somehow mod 5 produces a number > 4");
        }
    }
}

