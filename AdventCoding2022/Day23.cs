using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day23
    {
        public static string A(string input)
        {
            HashSet<IntVector2> elfPositions = new HashSet<IntVector2>();
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        elfPositions.Add(new IntVector2(x, y));
                    }
                }
            }
            int directionState = 0;
            for (int time = 0; time < 10; time++)
            {
                //DrawElves(elfPositions);
                Dictionary<IntVector2, int> proposedCounter = new Dictionary<IntVector2, int>();
                Dictionary<IntVector2, IntVector2> proposedPositions = new Dictionary<IntVector2, IntVector2>();
                // First, eliminate the elves that don't want to move
                foreach (IntVector2 pos in elfPositions)
                {
                    // make a list of the state of nearby positions
                    List<bool> neighbours = new List<bool>(8);
                    bool hasNeighbour = false; ;
                    foreach (IntVector2 direction in IntVector2.CardinalDirectionsIncludingDiagonals)
                    {
                        IntVector2 testPos = pos.Add(direction);
                        bool exists = elfPositions.Contains(testPos);
                        neighbours.Add(exists);
                        hasNeighbour = hasNeighbour || exists;
                    }

                    // Check to see if we need to move at all
                    if (hasNeighbour)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            bool done = false; ;
                            switch ((directionState + i) % 4)
                            {
                                case 0:
                                    // North
                                    if (!neighbours[0] && !neighbours[3] && !neighbours[5])
                                    {
                                        done = true;
                                        IntVector2 newPos = pos.North();
                                        proposedPositions.Add(pos, newPos);
                                        if (!proposedCounter.ContainsKey(newPos))
                                        {
                                            proposedCounter.Add(newPos, 1);
                                        }
                                        else
                                        {
                                            proposedCounter[newPos]++;
                                        }
                                    }
                                    break;
                                case 1:
                                    // South
                                    if (!neighbours[2] && !neighbours[4] && !neighbours[7])
                                    {
                                        done = true;
                                        IntVector2 newPos = pos.South();
                                        proposedPositions.Add(pos, newPos);
                                        if (!proposedCounter.ContainsKey(newPos))
                                        {
                                            proposedCounter.Add(newPos, 1);
                                        }
                                        else
                                        {
                                            proposedCounter[newPos]++;
                                        }

                                    }
                                    break;
                                case 2:
                                    // West
                                    if (!neighbours[0] && !neighbours[1] && !neighbours[2])
                                    {
                                        done = true;
                                        IntVector2 newPos = pos.West();
                                        proposedPositions.Add(pos, newPos);
                                        if (!proposedCounter.ContainsKey(newPos))
                                        {
                                            proposedCounter.Add(newPos, 1);
                                        }
                                        else
                                        {
                                            proposedCounter[newPos]++;
                                        }
                                    }
                                    break;
                                case 3:
                                    // East
                                    if (!neighbours[5] && !neighbours[6] && !neighbours[7])
                                    {
                                        done = true;
                                        IntVector2 newPos = pos.East();
                                        proposedPositions.Add(pos, newPos);
                                        if (!proposedCounter.ContainsKey(newPos))
                                        {
                                            proposedCounter.Add(newPos, 1);
                                        }
                                        else
                                        {
                                            proposedCounter[newPos]++;
                                        }
                                    }
                                    break;
                            }
                            if (done)
                            {
                                break;
                            }

                        }
                    }
                }

                directionState = (directionState + 1) % 4;

                HashSet<IntVector2> newPositions = new HashSet<IntVector2>();
                foreach (IntVector2 pos in elfPositions)
                {
                    IntVector2 newPos = pos;
                    if (proposedPositions.ContainsKey(pos))
                    {
                        newPos = proposedPositions[pos];
                    }
                    if (proposedCounter.ContainsKey(newPos) && proposedCounter[newPos] == 1)
                    {
                        newPositions.Add(newPos);
                    }
                    else
                    {
                        newPositions.Add(pos);
                    }
                }
                elfPositions = newPositions;

            }

            //DrawElves(elfPositions);
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;
            foreach (IntVector2 pos in elfPositions)
            {
                minX = Math.Min(minX, pos.X);
                minY = Math.Min(minY, pos.Y);
                maxX = Math.Max(maxX, pos.X);
                maxY = Math.Max(maxY, pos.Y);
            }
            return (((maxX - minX + 1) * (maxY - minY + 1)) - elfPositions.Count).ToString();
        }

        public static string B(string input)
        {
            HashSet<IntVector2> elfPositions = new HashSet<IntVector2>();
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        elfPositions.Add(new IntVector2(x, y));
                    }
                }
            }
            int directionState = 0;
            int time = 0;
            while(true)
            {
                time++;
                //DrawElves(elfPositions);
                Dictionary<IntVector2, int> proposedCounter = new Dictionary<IntVector2, int>();
                Dictionary<IntVector2, IntVector2> proposedPositions = new Dictionary<IntVector2, IntVector2>();
                // First, eliminate the elves that don't want to move
                foreach (IntVector2 pos in elfPositions)
                {
                    // make a list of the state of nearby positions
                    List<bool> neighbours = new List<bool>(8);
                    bool hasNeighbour = false; ;
                    foreach (IntVector2 direction in IntVector2.CardinalDirectionsIncludingDiagonals)
                    {
                        IntVector2 testPos = pos.Add(direction);
                        bool exists = elfPositions.Contains(testPos);
                        neighbours.Add(exists);
                        hasNeighbour = hasNeighbour || exists;
                    }

                    // Check to see if we need to move at all
                    if (hasNeighbour)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            bool done = false; ;
                            switch ((directionState + i) % 4)
                            {
                                case 0:
                                    // North
                                    if (!neighbours[0] && !neighbours[3] && !neighbours[5])
                                    {
                                        done = true;
                                        IntVector2 newPos = pos.North();
                                        proposedPositions.Add(pos, newPos);
                                        if (!proposedCounter.ContainsKey(newPos))
                                        {
                                            proposedCounter.Add(newPos, 1);
                                        }
                                        else
                                        {
                                            proposedCounter[newPos]++;
                                        }
                                    }
                                    break;
                                case 1:
                                    // South
                                    if (!neighbours[2] && !neighbours[4] && !neighbours[7])
                                    {
                                        done = true;
                                        IntVector2 newPos = pos.South();
                                        proposedPositions.Add(pos, newPos);
                                        if (!proposedCounter.ContainsKey(newPos))
                                        {
                                            proposedCounter.Add(newPos, 1);
                                        }
                                        else
                                        {
                                            proposedCounter[newPos]++;
                                        }

                                    }
                                    break;
                                case 2:
                                    // West
                                    if (!neighbours[0] && !neighbours[1] && !neighbours[2])
                                    {
                                        done = true;
                                        IntVector2 newPos = pos.West();
                                        proposedPositions.Add(pos, newPos);
                                        if (!proposedCounter.ContainsKey(newPos))
                                        {
                                            proposedCounter.Add(newPos, 1);
                                        }
                                        else
                                        {
                                            proposedCounter[newPos]++;
                                        }
                                    }
                                    break;
                                case 3:
                                    // East
                                    if (!neighbours[5] && !neighbours[6] && !neighbours[7])
                                    {
                                        done = true;
                                        IntVector2 newPos = pos.East();
                                        proposedPositions.Add(pos, newPos);
                                        if (!proposedCounter.ContainsKey(newPos))
                                        {
                                            proposedCounter.Add(newPos, 1);
                                        }
                                        else
                                        {
                                            proposedCounter[newPos]++;
                                        }
                                    }
                                    break;
                            }
                            if (done)
                            {
                                break;
                            }

                        }
                    }
                }

                directionState = (directionState + 1) % 4;

                if (proposedPositions.Count == 0)
                {
                    break;
                }
                HashSet<IntVector2> newPositions = new HashSet<IntVector2>();
                foreach (IntVector2 pos in elfPositions)
                {
                    IntVector2 newPos = pos;
                    if (proposedPositions.ContainsKey(pos))
                    {
                        newPos = proposedPositions[pos];
                    }
                    if (proposedCounter.ContainsKey(newPos) && proposedCounter[newPos] == 1)
                    {
                        newPositions.Add(newPos);
                    }
                    else
                    {
                        newPositions.Add(pos);
                    }
                }
                elfPositions = newPositions;

            }

            //DrawElves(elfPositions);
            return time.ToString();
        }


        private static void DrawElves(HashSet<IntVector2> elfPositions)
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;            
            foreach (IntVector2 pos in elfPositions)
            {
                minX = Math.Min(minX, pos.X);
                minY = Math.Min(minY, pos.Y);
                maxX = Math.Max(maxX, pos.X);
                maxY = Math.Max(maxY, pos.Y);
            }
            minX = Math.Min(minX, 0);
            minY = Math.Min(minY, 0);
            for (int y = minY-1; y <= maxY; y++)
            {
                if (y == minY - 1)
                {
                    for (int x = minX - 1; x <= maxX; x++)
                    {
                        if (x == 0)
                        {
                            Console.Write("V");
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }
                }
                else
                {
                    for (int x = minX - 1; x <= maxX; x++)
                    {
                        if (x == minX - 1)
                        {
                            if (y == 0)
                            {
                                Console.Write(">");
                            }
                            else
                            {
                                Console.Write(" ");
                            }
                        }
                        else
                        {
                            IntVector2 pos = new IntVector2(x, y);
                            if (elfPositions.Contains(pos))
                            {
                                Console.Write('#');
                            }
                            else
                            {
                                Console.Write('.');
                            }
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
