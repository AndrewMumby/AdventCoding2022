using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day22
    {
        public static string A(string input)
        {
            // input has two parts, split by a double line break
            string net = input.Split(new string[] { (Environment.NewLine + Environment.NewLine) }, StringSplitOptions.None)[0];
            string instructionString = input.Split(new string[] { (Environment.NewLine + Environment.NewLine) }, StringSplitOptions.None)[1];

            // which net do we have?
            bool bigNet = net.Length > 200;
            int longestLine = 0;
            string[] netLines = net.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in netLines)
            {
                longestLine = Math.Max(longestLine, line.Length);
            }
            longestLine = Math.Max(longestLine, netLines.Length);

            IntField2 cubeNet = new IntField2(longestLine, longestLine, 0);
            for (int y = 0; y < netLines.Length; y++)
            {
                for (int x = 0; x < netLines[y].Length; x++)
                {
                    if (netLines[y][x] == '.')
                    {
                        cubeNet.SetValue(x, y, 1);
                    }
                    else if (netLines[y][x] == '#')
                    {
                        cubeNet.SetValue(x, y, 2);
                    }
                }
            }

            Queue<string> instructions = new Queue<string>();
            int insStrPos = 0;
            while (insStrPos < instructionString.Length)
            {
                if (instructionString[insStrPos] == 'L')
                {
                    instructions.Enqueue("L");
                    insStrPos++;
                }
                else if (instructionString[insStrPos] == 'R')
                {
                    instructions.Enqueue("R");
                    insStrPos++;

                }
                else
                {
                    // We've got a number
                    string numberString = "";
                    while (insStrPos < instructionString.Length && instructionString[insStrPos] != 'L' && instructionString[insStrPos] != 'R')
                    {
                        numberString = numberString + instructionString[insStrPos];
                        insStrPos++;
                    }
                    instructions.Enqueue(numberString);
                }
            }

            // Set up the teleporters
            int faceSize = 4;
            if (bigNet)
            {
                faceSize = 50;
            }

            /*
             * ..x.
             * xxx.
             * ..xx
             * 
             * .xx
             * .x.
             * xx.
             * x..
             * 
             */

            Dictionary<Tuple<IntVector2, IntVector2>, Tuple<IntVector2, IntVector2>> teleporters = new Dictionary<Tuple<IntVector2, IntVector2>, Tuple<IntVector2, IntVector2>>();

            for (int i = 0; i < faceSize; i++)
            {
                // Easyish for A
                if (!bigNet)
                {
                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize + i, -1), new IntVector2(0, -1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize + i, 3 * faceSize), new IntVector2(0, 1))
                    );
                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(i, faceSize - 1), new IntVector2(0, -1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(i, 2 * faceSize), new IntVector2(0, 1))
                    );
                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize + i, faceSize - 1), new IntVector2(0, -1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize + i, 2 * faceSize), new IntVector2(0, 1))
                    );
                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(3 * faceSize + i, 2 * faceSize - 1), new IntVector2(0, -1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(3 * faceSize + i, 3 * faceSize), new IntVector2(0, 1))
                    );

                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize - 1, i), new IntVector2(-1, 0)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(3 * faceSize, i), new IntVector2(1, 0))
                    );

                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(-1, faceSize + i), new IntVector2(-1, 0)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(3 * faceSize, faceSize + i), new IntVector2(1, 0))
                    );
                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize - 1, 2 * faceSize + i), new IntVector2(-1, 0)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(4 * faceSize, 2 * faceSize + i), new IntVector2(1, 0))
                    );
                }

                else
                {
                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(i, 2 * faceSize - 1), new IntVector2(0, -1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(i, 4 * faceSize), new IntVector2(0, 1))
                    );
                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize + i, -1), new IntVector2(0, -1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize + i, 3 * faceSize), new IntVector2(0, 1))
                    );
                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize + i, -1), new IntVector2(0, -1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize + i, faceSize), new IntVector2(0, 1))
                    );
                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize - 1, i), new IntVector2(-1, 0)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(3 * faceSize, i), new IntVector2(1, 0))
                    );

                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize - 1, faceSize + i), new IntVector2(-1, 0)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize, faceSize + i), new IntVector2(1, 0))
                    );

                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(-1, 2 * faceSize + i), new IntVector2(-1, 0)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize, 2 * faceSize + i), new IntVector2(1, 0))
                    );
                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(-1, 3 * faceSize + i), new IntVector2(-1, 0)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize, 3 * faceSize + i), new IntVector2(1, 0))
                    );

                }
            }

            // Add the reverses to the dictionary too
            Dictionary<Tuple<IntVector2, IntVector2>, Tuple<IntVector2, IntVector2>> otherTeleporters = new Dictionary<Tuple<IntVector2, IntVector2>, Tuple<IntVector2, IntVector2>>();

            foreach (KeyValuePair<Tuple<IntVector2, IntVector2>, Tuple<IntVector2, IntVector2>> pair in teleporters)
            {
                otherTeleporters[pair.Value] = pair.Key;
            }

            foreach (KeyValuePair<Tuple<IntVector2, IntVector2>, Tuple<IntVector2, IntVector2>> pair in otherTeleporters)
            {
                teleporters[pair.Key] = pair.Value;
            }

            otherTeleporters.Clear();
            otherTeleporters = null;


            IntVector2 pos = new IntVector2(0, 0);
            while (cubeNet.GetValue(pos) == 0)
            {
                pos = pos.East();
            }
            IntVector2 dir = new IntVector2(1, 0);
            DrawNet(cubeNet, teleporters, pos, dir);

            while (instructions.Count > 0)
            {
                string instruction = instructions.Dequeue();
                if (instruction == "L")
                {
                    dir = dir.Left();
                }
                else if (instruction == "R")
                {
                    dir = dir.Right();
                }
                else
                {
                    int count = Int32.Parse(instruction);
                    while (count > 0)
                    {
                        IntVector2 newPos = pos.Add(dir);
                        IntVector2 newDir = dir;
                        // Have we fallen off the edge?
                        if (cubeNet.GetValue(newPos) == 0)
                        {
                            // Yes - Teleport
                            Tuple<IntVector2, IntVector2> teleEnd = teleporters[new Tuple<IntVector2, IntVector2>(newPos, dir)];
                            newPos = teleEnd.Item1;
                            newDir = teleEnd.Item2.Right().Right();
                            newPos = newPos.Add(newDir);
                        }
                        if (cubeNet.GetValue(newPos) == 2)
                        {
                            // Hit a wall
                            break;
                        }
                        else
                        {
                            pos = newPos;
                            dir = newDir;
                        }
                        count--;
                    }
                }
            }
            if (dir.Equals(new IntVector2(1, 0)))
            {
                return ((pos.Y + 1) * 1000 + (pos.X + 1) * 4 + 0).ToString();
            }
            if (dir.Equals(new IntVector2(0, 1)))
            {
                return ((pos.Y + 1) * 1000 + (pos.X + 1) * 4 + 1).ToString();
            }
            if (dir.Equals(new IntVector2(-1, 0)))
            {
                return ((pos.Y + 1) * 1000 + (pos.X + 1) * 4 + 2).ToString();
            }
            if (dir.Equals(new IntVector2(0, -1)))
            {
                return ((pos.Y + 1) * 1000 + (pos.X + 1) * 4 + 3).ToString();
            }
            return "Invalid direction";
        }
        public static string B(string input)
        {
            // input has two parts, split by a double line break
            string net = input.Split(new string[] { (Environment.NewLine + Environment.NewLine) }, StringSplitOptions.None)[0];
            string instructionString = input.Split(new string[] { (Environment.NewLine + Environment.NewLine) }, StringSplitOptions.None)[1];

            // which net do we have?
            bool bigNet = net.Length > 200;
            int longestLine = 0;
            string[] netLines = net.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in netLines)
            {
                longestLine = Math.Max(longestLine, line.Length);
            }
            longestLine = Math.Max(longestLine, netLines.Length);

            IntField2 cubeNet = new IntField2(longestLine, longestLine, 0);
            for (int y = 0; y < netLines.Length; y++)
            {
                for (int x = 0; x < netLines[y].Length; x++)
                {
                    if (netLines[y][x] == '.')
                    {
                        cubeNet.SetValue(x, y, 1);
                    }
                    else if (netLines[y][x] == '#')
                    {
                        cubeNet.SetValue(x, y, 2);
                    }
                }
            }

            Queue<string> instructions = new Queue<string>();
            int insStrPos = 0;
            while (insStrPos < instructionString.Length)
            {
                if (instructionString[insStrPos] == 'L')
                {
                    instructions.Enqueue("L");
                    insStrPos++;
                }
                else if (instructionString[insStrPos] == 'R')
                {
                    instructions.Enqueue("R");
                    insStrPos++;

                }
                else
                {
                    // We've got a number
                    string numberString = "";
                    while (insStrPos < instructionString.Length && instructionString[insStrPos] != 'L' && instructionString[insStrPos] != 'R')
                    {
                        numberString = numberString + instructionString[insStrPos];
                        insStrPos++;
                    }
                    instructions.Enqueue(numberString);
                }
            }

            // Set up the teleporters
            int faceSize = 4;
            if (bigNet)
            {
                faceSize = 50;
            }

            /*    v
             *  v.x<
             * >xxxL
             *  ^%xx<
             *    ^^
             *  vv 
             * >xx<
             * %x%
             *>xx<
             *>x%.
             * ^
             */

            Dictionary<Tuple<IntVector2, IntVector2>, Tuple<IntVector2, IntVector2>> teleporters = new Dictionary<Tuple<IntVector2, IntVector2>, Tuple<IntVector2, IntVector2>>();

            for (int i = 0; i < faceSize; i++)
            {
                // Easyish for A
                if (!bigNet)
                {

                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize + i, -1), new IntVector2(0, -1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize - (i + 1), faceSize - 1), new IntVector2(0, -1))
                    );
                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(3 * faceSize, i), new IntVector2(1, 0)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(4 * faceSize, 3 * faceSize - (i + 1)), new IntVector2(1, 0))
                    );

                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(3 * faceSize, faceSize + i), new IntVector2(1, 0)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(4 * faceSize - (i + 1), 2 * faceSize - 1), new IntVector2(0, -1))
                    );

                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(3 * faceSize + i, 3 * faceSize), new IntVector2(0, 1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(-1, 2 * faceSize - (i + 1)), new IntVector2(-1, 0))
                    );

                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize + i, 3 * faceSize), new IntVector2(0, 1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize - (i + 1), 2 * faceSize), new IntVector2(0, 1))
                    );

                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize - 1, 2 * faceSize + i), new IntVector2(-1, 0)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize - (i + 1), 2 * faceSize), new IntVector2(0, 1))
                    );

                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize + i, faceSize - 1), new IntVector2(0, -1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize - 1, i), new IntVector2(-1, 0))
                    );

                }

                else
                {

                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize + i, faceSize), new IntVector2(0, 1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize, faceSize + i), new IntVector2(1, 0))
                    );


                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(3 * faceSize, i), new IntVector2(1, 0)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize, 3 * faceSize - (i + 1)), new IntVector2(1, 0))
                    );


                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize + i, 3 * faceSize), new IntVector2(0, 1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize, 3 * faceSize + i), new IntVector2(1, 0))
                    );


                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(i, 2 * faceSize - 1), new IntVector2(0, -1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize - 1, faceSize + i), new IntVector2(-1, 0))
                    );


                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(-1, 2 * faceSize + i), new IntVector2(-1, 0)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize - 1, faceSize - (i + 1)), new IntVector2(-1, 0))
                    );

                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(-1, 3 * faceSize + i), new IntVector2(-1, 0)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(faceSize + i, -1), new IntVector2(0, -1))
                    );

                    teleporters.Add
                    (
                        new Tuple<IntVector2, IntVector2>(new IntVector2(2 * faceSize + i, -1), new IntVector2(0, -1)),
                        new Tuple<IntVector2, IntVector2>(new IntVector2(i, 4 * faceSize), new IntVector2(0, 1))
                    );
                }
            }

            // Add the reverses to the dictionary too
            Dictionary<Tuple<IntVector2, IntVector2>, Tuple<IntVector2, IntVector2>> otherTeleporters = new Dictionary<Tuple<IntVector2, IntVector2>, Tuple<IntVector2, IntVector2>>();

            foreach (KeyValuePair<Tuple<IntVector2, IntVector2>, Tuple<IntVector2, IntVector2>> pair in teleporters)
            {
                otherTeleporters[pair.Value] = pair.Key;
            }

            foreach (KeyValuePair<Tuple<IntVector2, IntVector2>, Tuple<IntVector2, IntVector2>> pair in otherTeleporters)
            {
                teleporters[pair.Key] = pair.Value;
            }

            otherTeleporters.Clear();
            otherTeleporters = null;


            IntVector2 pos = new IntVector2(0, 0);
            while (cubeNet.GetValue(pos) == 0)
            {
                pos = pos.East();
            }
            IntVector2 dir = new IntVector2(1, 0);

            while (instructions.Count > 0)
            {
                string instruction = instructions.Dequeue();
                //DrawNet(cubeNet, teleporters, pos, dir);
                Console.WriteLine(instruction);
                if (instruction == "L")
                {
                    dir = dir.Left();
                }
                else if (instruction == "R")
                {
                    dir = dir.Right();
                }
                else
                {
                    int count = Int32.Parse(instruction);
                    while (count > 0)
                    {
                        IntVector2 newPos = pos.Add(dir);
                        IntVector2 newDir = dir;
                        // Have we fallen off the edge?
                        if (cubeNet.GetValue(newPos) == 0)
                        {
                            // Yes - Teleport
                            Tuple<IntVector2, IntVector2> teleEnd = teleporters[new Tuple<IntVector2, IntVector2>(newPos, dir)];
                            newPos = teleEnd.Item1;
                            newDir = teleEnd.Item2.Right().Right();
                            newPos = newPos.Add(newDir);
                        }
                        if (cubeNet.GetValue(newPos) == 2)
                        {
                            // Hit a wall
                            break;
                        }
                        else
                        {
                            pos = newPos;
                            dir = newDir;
                        }
                        count--;
                    }
                }
            }
            DrawNet(cubeNet, teleporters, pos, dir);

            if (dir.Equals(new IntVector2(1, 0)))
            {
                return ((pos.Y + 1) * 1000 + (pos.X + 1) * 4 + 0).ToString();
            }
            if (dir.Equals(new IntVector2(0, 1)))
            {
                return ((pos.Y + 1) * 1000 + (pos.X + 1) * 4 + 1).ToString();
            }
            if (dir.Equals(new IntVector2(-1, 0)))
            {
                return ((pos.Y + 1) * 1000 + (pos.X + 1) * 4 + 2).ToString();
            }
            if (dir.Equals(new IntVector2(0, -1)))
            {
                return ((pos.Y + 1) * 1000 + (pos.X + 1) * 4 + 3).ToString();
            }
            return "Invalid direction";
        }

        private static void DrawNet(IntField2 cubeNet, Dictionary<Tuple<IntVector2, IntVector2>, Tuple<IntVector2, IntVector2>> teleporters, IntVector2 pos, IntVector2 dir)
        {
            for (int y = -1; y < cubeNet.GetSize(1) + 1; y++)
            {
                for (int x = -1; x < cubeNet.GetSize(0) + 1; x++)
                {
                    char c = '0';
                    IntVector2 testPos = new IntVector2(x,y);
                    switch (cubeNet.GetValue(testPos))
                    {
                        case 0:
                            c = ' ';
                            break;
                            case 1:
                            c = '.';
                            break;
                        case 2:
                            c = '#';
                            break;
                    }
                    char t = ' ';
                    if (teleporters.ContainsKey(new Tuple<IntVector2, IntVector2> (testPos, new IntVector2(-1,0))))
                    {
                        t = '<';
                    }
                    if (teleporters.ContainsKey(new Tuple<IntVector2, IntVector2>(testPos, new IntVector2(1, 0))))
                    {
                        t = '>';
                    }
                    if (teleporters.ContainsKey(new Tuple<IntVector2, IntVector2>(testPos, new IntVector2(0, -1))))
                    {
                        t = '^';
                    }
                    if (teleporters.ContainsKey(new Tuple<IntVector2, IntVector2>(testPos, new IntVector2(0, 1))))
                    {
                        t = 'v';
                    }

                    if (testPos.Equals(pos))
                    {
                        if (dir.Equals(new IntVector2(1,0)))
                        {
                            Console.Write('→');
                        }
                        else if (dir.Equals(new IntVector2(-1, 0)))
                        {
                            Console.Write('←');
                        }
                        else if (dir.Equals(new IntVector2(0, 1)))
                        {
                            Console.Write('↓');
                        }
                        else if (dir.Equals(new IntVector2(0, -1)))
                        {
                            Console.Write('↑');
                        }


                    }
                    else if (c != ' ' && t != ' ')
                    {
                        Console.Write('E');
                    }
                    else if (t!= ' ')
                    {
                        Console.Write(t);
                    }
                    else
                    {
                        Console.Write(c);
                    }

                }
                Console.WriteLine();
            }
        }
    }

}