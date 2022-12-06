using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day6
    {
        public static string A(string input)
        {
            int cursor = 3;
            while (cursor < input.Length)
            {
                HashSet<char> sample = input.Substring(cursor - 3, 4).AsEnumerable<char>().ToHashSet();
                if (sample.Count() == 4)
                {
                    return (cursor + 1).ToString();
                }
                cursor++;
            }
            throw new Exception("No marker found");
        }

        public static string B(string input)
        {
            int cursor = 13;
            while (cursor < input.Length)
            {
                HashSet<char> sample = input.Substring(cursor - 13, 14).AsEnumerable<char>().ToHashSet();
                if (sample.Count() == 14)
                {
                    return (cursor + 1).ToString();
                }
                cursor++;
            }
            throw new Exception("No marker found");
        }
    }
}
