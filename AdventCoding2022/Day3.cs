using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day3
    {
        public static string A(string input)
        {
            string[] inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int total = 0;
            foreach (string line in inputList)
            {
                // split each line into 2
                string first = line.Substring(0, line.Length / 2);
                string second = line.Substring(line.Length / 2);
                HashSet<int> firstSet = GetPrioritySet(first);
                HashSet<int> secondSet = GetPrioritySet(second);
                HashSet<int> overlap = firstSet.Intersect(secondSet).ToHashSet<int>();
                if (overlap.Count != 1)
                {
                    throw new Exception("Invalid overlap: " + overlap.Count);
                }
                total = total + overlap.First();
            }
            return total.ToString();
        }

        public static string B(string input)
        {
            string[] inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int total = 0;
            int index = 0;
            do
            {
                int[] counts = new int[(26 * 2)+1];
                AddToCounts(inputList[index], counts);
                AddToCounts(inputList[index+1], counts);
                AddToCounts(inputList[index+2], counts);
                // find the one that's 3
                for (int i = 1; i < counts.Length; i++)
                {
                    if (counts[i] == 3)
                    {
                        total = total + i;
                        break;
                    }
                }
                index += 3;
            }
            while (index < inputList.Length);
            return total.ToString();
        }

        private static void AddToCounts(string line, int[] counts)
        {
            int[] totals = new int[(26 * 2) + 1];
            foreach (char c in line)
            {
                int priority = GetPriority(c); 
                totals[priority]++;
            }
            for (int i = 1; i < counts.Length;i++)
            {
                if (totals[i]>0)
                {
                    counts[i]++;
                }
            }
        }

        private static HashSet<int> GetPrioritySet (string s)
        {
            HashSet<int> prioritySet = new HashSet<int>();
            foreach (char c in s)
            {
                prioritySet.Add(GetPriority(c));
            }
            return prioritySet;
        }

        private static int GetPriority(char c)
        {
            if (c >= 'a')
            {
                return c - 'a' + 1;
            }
            else
            {
                return c - 'A' + 27;
            }
        }
    }
}
