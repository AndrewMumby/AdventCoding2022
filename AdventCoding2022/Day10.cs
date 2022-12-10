using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day10
    {
        public static string A(string input)
        {
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            List<int> values = new List<int>();
            int x = 1;
            values.Add(x);
            values.Add(x);
            foreach (string line in inputLines)
            {
                if (line == "noop")
                {
                    values.Add(x);
                }
                else
                {
                    string[] parts = line.Split(new char[] { ' ' });
                    values.Add(x);
                    x = x + Int32.Parse(parts[1]);
                    values.Add(x);
                }
            }

            return (
                20 * values[20] +
                60 * values[60] +
                100 * values[100] +
                140 * values[140] +
                180 * values[180] +
                220 * values[220]
                ).ToString();
        }
        public static string B(string input)
        {
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            List<int> values = new List<int>();
            int value = 1;
            values.Add(value);
            values.Add(value);
            foreach (string line in inputLines)
            {
                if (line == "noop")
                {
                    values.Add(value);
                }
                else
                {
                    string[] parts = line.Split(new char[] { ' ' });
                    values.Add(value);
                    value = value + Int32.Parse(parts[1]);
                    values.Add(value);
                }

            }

            StringBuilder screen = new StringBuilder();
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 40; x++)
                {
                    int tick = y * 40 + x + 1;
                    if (values[tick] == x || values[tick] == x - 1 || values[tick] == x + 1)
                    {
                        screen.Append('#');
                    }
                    else

                    {
                        screen.Append('.');
                    }
                }
                screen.Append(Environment.NewLine);
            }
            Console.WriteLine(screen.ToString());
            return "";
        }
    }
}
