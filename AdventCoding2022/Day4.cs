using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day4
    {
        public static string A(string input)
        {
            string[] inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int count = 0;
            foreach (string line in inputList)
            {
                // 2-4,6-8
                string[] stringParts = line.Split(new char[] { '-', ',' }, StringSplitOptions.RemoveEmptyEntries);
                int[] parts = stringParts.Select(x => Int32.Parse(x)).ToArray();
                if ((parts[0] >= parts[2] && parts[1] <= parts[3]) || (parts[0] <= parts[2] && parts[1] >= parts[3]))
                {
                    count++;
                }
            }
            return count.ToString();
        }

        public static string B(string input)
        {
            string[] inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int count = 0;
            foreach (string line in inputList)
            {
                // 2-4,6-8
                string[] stringParts = line.Split(new char[] { '-', ',' }, StringSplitOptions.RemoveEmptyEntries);
                int[] parts = stringParts.Select(x => Int32.Parse(x)).ToArray();
                if (!((parts[1] < parts[2]) || (parts[3] < parts[0])))
                    {
                    count++;
                }

            }

            return count.ToString();
        }
    }

}
