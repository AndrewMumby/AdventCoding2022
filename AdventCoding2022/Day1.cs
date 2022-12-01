using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day1
    {
        public static string A(string input)
        {
            string[] inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int bestTotal = 0;
            int currentTotal = 0;
            for (int i = 0; i < inputList.Length; i++)
            {
                if (inputList[i].Length == 0)
                {
                    bestTotal = Math.Max(bestTotal, currentTotal);
                    currentTotal = 0;
                }
                else
                {
                    currentTotal = currentTotal + Int32.Parse(inputList[i]);
                }
            }
            bestTotal = Math.Max(bestTotal, currentTotal);
            return bestTotal.ToString();

        }
        public static string B(string input)
        {

            string[] inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int currentTotal = 0;
            List<int> totals = new List<int>();
            for (int i = 0; i < inputList.Length; i++)
            {
                if (inputList[i].Length == 0)
                {
                    totals.Add(currentTotal);
                    currentTotal = 0;
                }
                else
                {
                    currentTotal = currentTotal + Int32.Parse(inputList[i]);
                }
            }
            totals.Add(currentTotal);
            totals.Sort();
            int last = totals.Count - 1;
            return (totals[last] + totals[last - 1] + totals[last - 2]).ToString() ;

        }
    }
}
