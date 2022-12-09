using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day9
    {
        public static string A(string input)
        {
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            KnottedString stringy = new KnottedString(2);

            foreach (string line in inputLines)
            {
                string[] parts = line.Split(new char[] { ' ' });
                //R 4
                for (int i = 0; i < Int32.Parse(parts[1]); i++)
                {
                    switch (parts[0])
                    {
                        case "U":
                            stringy.Move(new IntVector2(0, -1));
                            break;
                        case "D":
                            stringy.Move(new IntVector2(0, 1));
                            break;
                        case "L":
                            stringy.Move(new IntVector2(-1, 0));
                            break;
                        case "R":
                            stringy.Move(new IntVector2(1, 0));
                            break;
                        default:
                            throw new Exception("Invalid direction");
                    }
                    //stringy.PrintStringPos();
                }
            }

            return stringy.GetCoverage().Count().ToString();
        }
        public static string B(string input)
        {
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            KnottedString stringy = new KnottedString(10);

            foreach (string line in inputLines)
            {
                string[] parts = line.Split(new char[] { ' ' });
                //R 4
                for (int i = 0; i < Int32.Parse(parts[1]); i++)
                {
                    switch (parts[0])
                    {
                        case "U":
                            stringy.Move(new IntVector2(0, -1));
                            break;
                        case "D":
                            stringy.Move(new IntVector2(0, 1));
                            break;
                        case "L":
                            stringy.Move(new IntVector2(-1, 0));
                            break;
                        case "R":
                            stringy.Move(new IntVector2(1, 0));
                            break;
                        default:
                            throw new Exception("Invalid direction");
                    }
                    //stringy.PrintString();
//                    stringy.PrintStringPos();
                }
            }

            return stringy.GetCoverage().Count().ToString();
        }

    }


    internal class KnottedString
    {
        private List<StringKnot> knots;
        private HashSet<IntVector2> coverage;

        internal KnottedString()
        {
            knots = new List<StringKnot>();
            coverage = new HashSet<IntVector2>();
        }

        internal KnottedString(int length)
        {
            knots = new List<StringKnot>();
            coverage = new HashSet<IntVector2>();
            for (int i = 0; i < length;i++)
            {
                knots.Add(new StringKnot());
            }
        }

        internal void PrintStringPos()
        {
            foreach(StringKnot knot in knots)
            {
                System.Console.Write(knot.ToString() + " ");
            }
            System.Console.WriteLine();
        }

        internal void PrintString()
        {
            for (int y = -30; y < 30; y++)
            {
                for (int x = -30; x < 30; x++)
                {
                    IntVector2 location = new IntVector2(x, y);
                    bool found = false;
                    for (int i = 0; i < knots.Count; i++)
                    {
                        if (location.Equals(knots[i].GetPosition()))
                        {
                            System.Console.Write(i);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }

        }

        internal void AddKnot(StringKnot knot)
        {
            knots.Add(knot);
        }

        internal void Move(IntVector2 direction)
        {
            // Move the first knot
            knots[0].Move(direction);
            // Move each following knot using the previous knot's position
            for (int i = 1; i < knots.Count; i++)
            {
                knots[i].MoveRelativeTo(knots[i - 1]);
            }
            coverage.Add(knots.Last().GetPosition());
        }

        internal HashSet<IntVector2> GetCoverage()
        {
            return new HashSet<IntVector2>(coverage);
        }
    }


    internal class StringKnot
    {
        private IntVector2 position;

        internal IntVector2 GetPosition()
        {
            return new IntVector2(position);
        }

        public StringKnot()
        {
            this.position = new IntVector2();
        }

        public StringKnot(IntVector2 position)
        {
            this.position = position;
        }

        internal IntVector2 Move(IntVector2 direction)
        {
            position = position.Add(direction);
            return GetPosition();
        }

        override public string ToString()
        {
            return position.ToString();
        }

        internal IntVector2 MoveRelativeTo(StringKnot stringKnot)
        {
            IntVector2 targetPos = stringKnot.GetPosition();
            // Do we need to move at all?
            if (IntVector2.CrowDistance(this.position, targetPos) > 1)
            {
                // Yes
                // Horizontal/Vertical?
                if (this.position.X == targetPos.X || this.position.Y == targetPos.Y)
                {
                    // Yes!
                    foreach (IntVector2 direction in IntVector2.CardinalDirections)
                    {
                        IntVector2 testPos = position.Add(direction);
                        if (testPos.CrowDistance(targetPos) == 1)
                        {
                            position = testPos;
                            return GetPosition();
                        }
                    }
                    throw new Exception("No movement connects the string");
                }
                else
                {
                    // diagonal
                    foreach (IntVector2 direction in IntVector2.DiagonalDirections)
                    {
                        IntVector2 testPos = position.Add(direction);
                        if (testPos.CrowDistance(targetPos) == 1)
                        {
                            position = testPos;
                            return GetPosition();
                        }
                    }
                    throw new Exception("No movement connects the string");

                }
            }
            return GetPosition();
        }
    }
}
