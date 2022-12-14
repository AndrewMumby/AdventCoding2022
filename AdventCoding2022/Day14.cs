using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day14
    {
        public static string A(string input)
        {
            SandFalls falls = new SandFalls(input);
            falls.PourInfiniteSand();
            return falls.SandCount().ToString();
        }

        public static string B(string input)
        {
            SandFalls falls = new SandFalls(input);
            falls.AddFloor();
            falls.PourInfiniteSand();
            return falls.SandCount().ToString();
        }
    }

    internal class SandFalls
    {
        HashSet<IntVector2> walls;
        HashSet<IntVector2> sand;

        int maxY;

        public SandFalls(string input)
        {
            walls = ParseData(input);
            maxY = 0;
            foreach (IntVector2 wall in walls)
            {
                maxY = Math.Max(maxY, wall.Y);
                maxY++;
            }

            sand = new HashSet<IntVector2>();

        }

        private bool IsOccupied(IntVector2 pos)
        {
            return walls.Contains(pos) || sand.Contains(pos);
        }

        internal void PourInfiniteSand()
        {
            bool finished = false;
            while (!finished)
            {
                IntVector2 curr = new IntVector2(500, 0);
                finished = true;
                while (curr.Y < maxY)
                {
                    if (!IsOccupied(curr.South()))
                    {
                        curr = curr.South();
                    }
                    else if (!IsOccupied(curr.South().West()))
                    {
                        curr = curr.South().West();
                    }
                    else if (!IsOccupied(curr.South().East()))
                    {
                        curr = curr.South().East();
                    }
                    else
                    {
                        sand.Add(curr);
                        finished = false;
                        if (sand.Contains(new IntVector2(500, 0)))
                        {
                            finished = true;
                        }
                        break;
                    }
                }
            }
        }

        internal object SandCount()
        {
            return sand.Count();
        }

        private static HashSet<IntVector2> ParseData(string input)
        {
            List<List<IntVector2>> pathPoints = new List<List<IntVector2>>();
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string[] parts = line.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);
                List<IntVector2> pathPoint = new List<IntVector2>();
                foreach (string pointString in parts)
                {
                    string[] pointParts = pointString.Split(new char[] { ',' });
                    IntVector2 point = new IntVector2(Convert.ToInt32(pointParts[0]), Convert.ToInt32(pointParts[1]));
                    pathPoint.Add(point);
                }
                pathPoints.Add(pathPoint);
            }

            HashSet<IntVector2> walls = new HashSet<IntVector2>();
            foreach (List<IntVector2> pathPoint in pathPoints)
            {
                IntVector2 current = pathPoint[0];
                int i = 0;
                while (i < pathPoint.Count - 1)
                {
                    IntVector2 next = pathPoint[i + 1];
                    // Work out unit vector from current to next
                    IntVector2 direction = current.DirectionTo(next);
                    IntVector2 cursor = current;
                    while (!next.Equals(cursor))
                    {
                        walls.Add(cursor);
                        cursor = cursor.Add(direction);
                    }
                    walls.Add(cursor);
                    current = next;
                    i++;
                }
            }

            return walls;
        }

        public void Draw()
        {
            // get the dimensions
            int minX = 500;
            int minY = 0;
            int maxX = 500;
            int maxY = 0;
            foreach (IntVector2 point in walls)
            {
                minX = Math.Min(minX, point.X);
                maxX = Math.Max(maxX, point.X);
                minY = Math.Min(minY, point.Y);
                maxY = Math.Max(maxY, point.Y);
            }

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    IntVector2 loc = new IntVector2(x, y);
                    if (sand.Contains(loc))
                    {
                        Console.Write("o");
                    }
                    else if (walls.Contains(loc))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
        }

        internal void AddFloor()
        {
            // find the y
            int y = 0;
            foreach (IntVector2 wall in walls)
            {
                y = Math.Max(y, wall.Y);
            }

            // add 2 to it
            y = y + 2;

            // Create a new wall there
            for (int x = 500 - y - 5; x <= 500 + y + 5; x++)
            {
                IntVector2 newWall = new IntVector2(x, y);
                walls.Add(newWall);
            }

            // update the maxY
            maxY = y + 1;
        }
    }
}
