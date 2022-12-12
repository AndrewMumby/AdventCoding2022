using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day12
    {
        public static string A(string input)
        {
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            IntField2 hills = new IntField2(inputLines[0].Length, inputLines.Length);
            IntVector2 start = new IntVector2();
            IntVector2 end = new IntVector2();

            for (int y = 0; y < hills.GetSize(1); y++)
            {
                for (int x = 0; x < hills.GetSize(0); x++)
                {
                    if (inputLines[y][x] == 'S')
                    {
                        hills.SetValue(x, y, 1);
                        start = new IntVector2(x, y);
                    }
                    else if (inputLines[y][x] == 'E')
                    {
                        hills.SetValue(x, y, 'z' - 'a' + 1);
                        end = new IntVector2(x, y);
                    }
                    else
                    {
                        hills.SetValue(x, y, inputLines[y][x] - 'a' + 1);
                    }
                }
            }

            return FindPathLength(hills, start, end).ToString();
        }

        public static string B(string input)
        {
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            IntField2 hills = new IntField2(inputLines[0].Length, inputLines.Length);
            Queue<IntVector2> starts = new Queue<IntVector2>();
            IntVector2 end = new IntVector2();

            for (int y = 0; y < hills.GetSize(1); y++)
            {
                for (int x = 0; x < hills.GetSize(0); x++)
                {
                    if (inputLines[y][x] == 'S')
                    {
                        starts.Enqueue(new IntVector2(x, y));
                        hills.SetValue(x, y, 1);
                    }
                    else if (inputLines[y][x] == 'E')
                    {
                        hills.SetValue(x, y, 'z' - 'a' + 1);
                        end = new IntVector2(x, y);
                    }
                    else
                    {
                        hills.SetValue(x, y, inputLines[y][x] - 'a' + 1);
                        if(inputLines[y][x] - 'a' + 1 == 1)
                        {
                            starts.Enqueue(new IntVector2(x, y));
                        }
                    }
                }
            }

            int best = Int32.MaxValue;

            foreach (IntVector2 start in starts)
            {
                int length = FindPathLength(hills, start, end);
                best = Math.Min(best, length); 
            }
            return best.ToString();



        }

        private static int FindPathLength(IntField2 hills, IntVector2 start, IntVector2 end)
        {
            HashSet<IntVector2> done = new HashSet<IntVector2>();
            HashSet<IntVector2> next = new HashSet<IntVector2>();
            Queue<IntVector2> curr = new Queue<IntVector2>();
            curr.Enqueue(new IntVector2(start));
            int steps = -1;
            while (true)
            {
                steps++;
                while (curr.Count > 0)
                {
                    IntVector2 here = curr.Dequeue();
                    done.Add(here);

                    if (here.Equals(end))
                    {
                        return steps;
                    }
                    int hereHeight = hills.GetValue(here);
                    foreach (IntVector2 direction in IntVector2.CardinalDirections)
                    {
                        IntVector2 cursor = here.Add(direction);
                        if (!done.Contains(cursor))
                        {
                            if (hills.GetValue(cursor) != 0 && hills.GetValue(cursor) <= hereHeight + 1)
                            {
                                next.Add(cursor);
                            }
                        }
                    }
                }
                curr = new Queue<IntVector2>();
                foreach (IntVector2 location in next)
                {
                    curr.Enqueue(location);
                }
                next = new HashSet<IntVector2>();
                if (curr.Count ==0)
                {
                    return Int32.MaxValue;
                }
                //Console.WriteLine(steps + " " + curr.Count);
            }
        }
    }
}
