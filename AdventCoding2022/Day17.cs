using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day17
    {
        public static string A(string input)
        {
            Queue<RockShape> shapes = new Queue<RockShape>();
            shapes.Enqueue(new HorLine());
            shapes.Enqueue(new Cross());
            shapes.Enqueue(new Jay());
            shapes.Enqueue(new VertLine());
            shapes.Enqueue(new Square());

            Queue<bool> moves = new Queue<bool>();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '<')
                {
                    moves.Enqueue(false);
                }
                else
                {
                    moves.Enqueue(true);
                }
            }
            HashSet<IntVector2> rockField = new HashSet<IntVector2>();
            for (int x = 0; x < 7; x++)
            {
                rockField.Add(new IntVector2(x, -1));
            }
            int topOfPile = -1;
            for (int i = 0; i < 2022; i++)
            {
                RockShape shape = shapes.Dequeue();
                shapes.Enqueue(shape);
                IntVector2 pos = new IntVector2(2, topOfPile + 4);
                bool falling = true;
                while (falling)
                {
                    // work out the new position
                    
                    IntVector2 newPos;
                    bool move = moves.Dequeue();
                    moves.Enqueue(move);
                    if (move)
                    {
                        newPos = pos.East();
                    }
                    else
                    {
                        newPos = pos.West();
                    }
                    if (!HasCollided(rockField, shape, newPos))
                    {
                        pos = newPos;   
                    }
                    //DrawField(rockField, shape, pos);
                    newPos = pos.North();
                    if (!HasCollided(rockField, shape,newPos))
                    {
                        pos = newPos;
                    }
                    else
                    {
                        falling = false;

                    }
                    //DrawField(rockField, shape, pos);
                }
                // collided. Add the points to the rockpile
                foreach (IntVector2 point in shape.Points)
                {
                    IntVector2 newPoint = point.Add(pos);
                    rockField.Add(newPoint);
                    topOfPile = Math.Max(topOfPile, newPoint.Y);
                }

                 //DrawField(rockField);

            }
            int maxY = 0;
            foreach (IntVector2 rock in rockField)
            {
                maxY = Math.Max(maxY, rock.Y);
            }
            return (maxY+1).ToString();

        }

        public static string B(string input)
        {
            Queue<RockShape> shapes = new Queue<RockShape>();
            shapes.Enqueue(new HorLine());
            shapes.Enqueue(new Cross());
            shapes.Enqueue(new Jay());
            shapes.Enqueue(new VertLine());
            shapes.Enqueue(new Square());

            Queue<bool> moves = new Queue<bool>();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '<')
                {
                    moves.Enqueue(false);
                }
                else
                {
                    moves.Enqueue(true);
                }
            }
            HashSet<IntVector2> rockField = new HashSet<IntVector2>();
            List<int> topOfPileHistory = new List<int>();
            for (int x = 0; x < 7; x++)
            {
                rockField.Add(new IntVector2(x, -1));
            }
            int topOfPile = -1;
            for (int i = 0; i < 5000; i++)
            {
                RockShape shape = shapes.Dequeue();
                shapes.Enqueue(shape);
                IntVector2 pos = new IntVector2(2, topOfPile + 4);
                bool falling = true;
                while (falling)
                {
                    // work out the new position

                    IntVector2 newPos;
                    bool move = moves.Dequeue();
                    moves.Enqueue(move);
                    if (move)
                    {
                        newPos = pos.East();
                    }
                    else
                    {
                        newPos = pos.West();
                    }
                    if (!HasCollided(rockField, shape, newPos))
                    {
                        pos = newPos;
                    }
                    //DrawField(rockField, shape, pos);
                    newPos = pos.North();
                    if (!HasCollided(rockField, shape, newPos))
                    {
                        pos = newPos;
                    }
                    else
                    {
                        falling = false;

                    }
                    //DrawField(rockField, shape, pos);
                }
                // collided. Add the points to the rockpile
                foreach (IntVector2 point in shape.Points)
                {
                    IntVector2 newPoint = point.Add(pos);
                    rockField.Add(newPoint);
                    topOfPile = Math.Max(topOfPile, newPoint.Y);
                }
                topOfPileHistory.Add(topOfPile);

            }
            List<int> diffHistory = new List<int>();
            for (int i = 1; i < topOfPileHistory.Count; i++)
            {
                diffHistory.Add(topOfPileHistory[i] - topOfPileHistory[i - 1]);
            }
            // Look for cycles in diffHistory?
            List<int> cycles = new List<int>();
            for (int interval = 10; interval < diffHistory.Count; interval++)
            {
                bool found = true;
                for (int yDiff = 1; yDiff < interval+1; yDiff++)
                {
                    if (diffHistory[diffHistory.Count - yDiff] == diffHistory[diffHistory.Count - (yDiff + interval)])
                    {
                    }
                    else
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    bool isMultiple = false;
                    foreach (int cycle in cycles)
                    {
                        if (interval % cycle == 0)
                        {
                            isMultiple = true;
                            break;
                        }
                    }
                    if (!isMultiple)
                    {
                        cycles.Add(interval);
                    }
                }
            }

            // Now we need to know when it starts too loop
            int cycleSize = cycles.First();
            int yStart = 0;
            bool looping = false;
            while (!looping)
            {
                looping = true;
                for (int yDiff = 0; yDiff < cycleSize; yDiff++)
                {
                    if (diffHistory[yStart + yDiff] != diffHistory[yStart + yDiff + cycleSize])
                    {
                        looping = false;
                        break;
                    }
                }
                yStart++;
            }
            yStart--;


            // The initial non-looping section is 
            int initialHeight = 0;
            for (int y = 0; y < yStart; y++)
            {
                initialHeight += diffHistory[y];
            }


            // Now we know it starts looping at yStart and initialHeight height with period cycle
            int heightPerPeriod = 0;
            for (int y = yStart; y < yStart+cycleSize; y++)
            {
                heightPerPeriod += diffHistory[y];
            }
            // and increases by heightPerPeriod each cycle
            // remove the first non-looping values
            long requiredCycles = 1000000000000 - (long)yStart;

            // Divide by the interval
            long completedLoops = requiredCycles / (long)cycleSize;

            long remainder = requiredCycles % (long)cycleSize;

            long remainderHeight = 0;
            for (int y = 0; y <remainder; y++)
            {
                remainderHeight = remainderHeight + diffHistory[y+yStart];
            }

            long total = initialHeight + completedLoops * heightPerPeriod + remainderHeight;


            return total.ToString();
        }

        internal static bool HasCollided(HashSet<IntVector2> pile, RockShape shape, IntVector2 pos)
        {
            // Check to see if we've hit the walls
            if (pos.X < 0)
            {
                return true;
            }

            if (pos.X + shape.Width > 7)
            {
                return true;
            }

            // check to see if we're still falling
            HashSet<IntVector2> points = shape.Points.Select(x => x.Add(pos)).ToHashSet();
            HashSet<IntVector2> collides = pile.Intersect(points).ToHashSet();
            if (collides.Count > 0)
            {
                return true;
            }
            return false;

        }

        internal static void DrawField(HashSet<IntVector2> pile)
        {
            int maxY = 0;
            foreach (IntVector2 rock in pile)
            {
                maxY = Math.Max(maxY, rock.Y);
            }
            for (int y = maxY; y >= 0; y--)
            {
                Console.Write("|");
                for (int x = 0; x < 7; x++)
                {
                    if (pile.Contains(new IntVector2(x, y)))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("+-------+");
            Console.WriteLine();
        }

        internal static void DrawField(HashSet<IntVector2> pile, RockShape shape, IntVector2 pos)
        {
            int maxY = 0;
            HashSet<IntVector2> shapePoints = shape.Points.Select(x => x.Add(pos)).ToHashSet();
            foreach (IntVector2 rock in pile)
            {
                maxY = Math.Max(maxY, rock.Y);
            }

            foreach (IntVector2 rock in shapePoints)
            {
                maxY = Math.Max(maxY, rock.Y);
            }

            for (int y = maxY; y >= 0; y--)
            {
                Console.Write("|");
                for (int x = 0; x < 7; x++)
                {
                    IntVector2 location = new IntVector2(x, y);
                    if (pile.Contains(location))
                    {
                        Console.Write("#");
                    }
                    else if (shapePoints.Contains(location))
                    {
                        Console.Write('@');
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("+-------+");
            Console.WriteLine();

        }
    }

    internal class RockShape
    {
        protected List<IntVector2> points;
        protected int width;
        protected int height;

        internal List<IntVector2> Points { get => points; }

        internal int Width { get => width; }
        internal int Height { get => height; }
    }
    internal class HorLine : RockShape
    {
        // ####

        public HorLine()
        {
            points = new List<IntVector2>();
            points.Add(new IntVector2(0, 0));
            points.Add(new IntVector2(1, 0));
            points.Add(new IntVector2(2, 0));
            points.Add(new IntVector2(3, 0));
            width = 4;
            height = 1;
        }
    }

    internal class Cross : RockShape
    {
        // .#.
        // ###
        // .#.

        public Cross()
        {
            points = new List<IntVector2>();
            points.Add(new IntVector2(1, 0));
            points.Add(new IntVector2(0, 1));
            points.Add(new IntVector2(1, 1));
            points.Add(new IntVector2(2, 1));
            points.Add(new IntVector2(1, 2));
            width = 3;
            height = 3;
        }
    }

    internal class Jay : RockShape
    {
        //..#
        //..#
        //###

        public Jay()
        {
            points = new List<IntVector2>();
            points.Add(new IntVector2(2, 2));
            points.Add(new IntVector2(2, 1));
            points.Add(new IntVector2(0, 0));
            points.Add(new IntVector2(1, 0));
            points.Add(new IntVector2(2, 0));
            width = 3;
            height = 3;
        }
    }

    internal class VertLine : RockShape
    {
        //#
        //#
        //#
        //#

        public VertLine()
        {
            points = new List<IntVector2>();
            points.Add(new IntVector2(0, 0));
            points.Add(new IntVector2(0, 1));
            points.Add(new IntVector2(0, 2));
            points.Add(new IntVector2(0, 3));
            width = 1;
            height = 4;
        }
    }

    internal class Square : RockShape
    {
        public Square()
        {
            points = new List<IntVector2>();
            points.Add(new IntVector2(0, 0));
            points.Add(new IntVector2(1, 0));
            points.Add(new IntVector2(0, 1));
            points.Add(new IntVector2(1, 1));
            width = 2;
            height = 2;
        }
    }
}
