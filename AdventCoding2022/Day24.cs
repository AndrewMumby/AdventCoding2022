using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day24
    {
        public static string A (string input)
        {
            WindyMaze maze = new WindyMaze(input);
            return maze.Distance(maze.Start(), maze.End()).ToString();
        }

        
        public static string B(string input)
        {
            WindyMaze maze = new WindyMaze (input);
            int there = maze.Distance(maze.Start(), maze.End());
            int back = maze.Distance(maze.End(), maze.Start(), there);
            int backAgain = maze.Distance(maze.Start(), maze.End(), back);
            Console.WriteLine(there + " " + back + " " + backAgain);
            return backAgain.ToString();
        }
        
    }

    internal class WindyMaze
    {
        HashSet<IntVector2> northWinds;
        HashSet<IntVector2> eastWinds;
        HashSet<IntVector2> southWinds;
        HashSet<IntVector2> westWinds;
        int maxX;
        int maxY;
        int windLoop;
        HashSet<IntVector3> freeSpaces;
        Dictionary<IntVector3, WindyNode> nodeLookup;

        public WindyMaze(string input)
        {
            northWinds = new HashSet<IntVector2>();
            eastWinds = new HashSet<IntVector2>();
            southWinds = new HashSet<IntVector2>();
            westWinds = new HashSet<IntVector2>();

            freeSpaces = new HashSet<IntVector3>();

            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            maxY = lines.Length - 2;
            Console.WriteLine("Parsing data...");
            for (int y = 1; y < lines.Length-1; y++)
            {
                maxX = Math.Max(maxX, lines[y].Length - 2);
                for (int x = 1; x <lines[y].Length-1; x++)
                {
                    switch (lines[y][x])
                    {
                        case '.':
                            break;
                        case '^':
                            northWinds.Add(new IntVector2(x, y));
                            break;
                        case '>':
                            eastWinds.Add(new IntVector2(x, y));
                            break;
                        case 'v':
                            southWinds.Add(new IntVector2(x,y));
                            break;
                        case '<':
                            westWinds.Add(new IntVector2(x, y));
                            break;
                        default:
                            throw new Exception("Unknown character " + lines[y][x]);
                    }
                }
            }
            Console.WriteLine("Moving winds");
            int time = 0;
            HashSet<IntVector2> takenSpaces = new HashSet<IntVector2>();
            foreach (IntVector2 wind in northWinds)
            {
                takenSpaces.Add(new IntVector2(wind));
            }
            foreach (IntVector2 wind in eastWinds)
            {
                takenSpaces.Add(new IntVector2(wind));
            }
            foreach (IntVector2 wind in southWinds)
            {
                takenSpaces.Add(new IntVector2(wind));
            }
            foreach (IntVector2 wind in westWinds)
            {
                takenSpaces.Add(new IntVector2(wind));
            }

            for (int y = 1; y < maxY; y++)
            {
                for (int x = 1; x < maxX; x++)
                {
                    IntVector2 pos = new IntVector2(x, y);
                    if (!takenSpaces.Contains(pos))
                    {
                        freeSpaces.Add(new IntVector3(x, y, time));
                    }
                }
            }
            HashSet<IntVector2> workingNorth = new HashSet<IntVector2>(northWinds);
            HashSet<IntVector2> workingEast = new HashSet<IntVector2>(eastWinds);
            HashSet<IntVector2> workingSouth = new HashSet<IntVector2>(southWinds);
            HashSet<IntVector2> workingWest = new HashSet<IntVector2>(westWinds);

            // now we need to do that again till we get a loop
            do
            {
                if (time%10 == 0)
                {
                    Console.WriteLine(time);
                }
                // Console.WriteLine(time);
                // DrawMaze(workingNorth, workingEast, workingSouth, workingWest, maxX, maxY);
                // Console.WriteLine();
                time++;
                // move all the winds
                HashSet<IntVector2> newWorkingNorth = new HashSet<IntVector2>();
                foreach (IntVector2 pos in workingNorth)
                {
                    IntVector2 testPos = pos.North();
                    if ((testPos.Y) == 0)
                    {
                        testPos.Y = maxY;
                    }
                    newWorkingNorth.Add(testPos);
                }
                workingNorth = newWorkingNorth;

                HashSet<IntVector2> newWorkingEast = new HashSet<IntVector2>();
                foreach (IntVector2 pos in workingEast)
                {
                    IntVector2 testPos = pos.East();
                    if ((testPos.X) > maxX)
                    {
                        testPos.X = 1;
                    }
                    newWorkingEast.Add(testPos);
                }
                workingEast = newWorkingEast;

                HashSet<IntVector2> newWorkingSouth = new HashSet<IntVector2>();
                foreach (IntVector2 pos in workingSouth)
                {
                    IntVector2 testPos = pos.South();
                    if ((testPos.Y) > maxY)
                    {
                        testPos.Y = 1;
                    }
                    newWorkingSouth.Add(testPos);
                }
                workingSouth = newWorkingSouth;

                HashSet<IntVector2> newWorkingWest = new HashSet<IntVector2>();
                foreach (IntVector2 pos in workingWest)
                {
                    IntVector2 testPos = pos.West();
                    if ((testPos.X) == 0)
                    {
                        testPos.X = maxX;
                    }
                    newWorkingWest.Add(testPos);
                }
                workingWest = newWorkingWest;

                takenSpaces = new HashSet<IntVector2>();
                foreach (IntVector2 wind in workingNorth)
                {
                    takenSpaces.Add(new IntVector2(wind));
                }
                foreach (IntVector2 wind in workingEast)
                {
                    takenSpaces.Add(new IntVector2(wind));
                }
                foreach (IntVector2 wind in workingSouth)
                {
                    takenSpaces.Add(new IntVector2(wind));
                }
                foreach (IntVector2 wind in workingWest)
                {
                    takenSpaces.Add(new IntVector2(wind));
                }

                for (int y = 1; y <= maxY; y++)
                {
                    for (int x = 1; x <= maxX; x++)
                    {
                        IntVector2 pos = new IntVector2(x, y);
                        if (!takenSpaces.Contains(pos))
                        {
                            freeSpaces.Add(new IntVector3(x, y, time));
                        }
                    }
                }
                windLoop = time;
            } while (!Looped(workingNorth, workingEast, workingSouth, workingWest));

            // The start and end squares are always free
            for (int i = 0; i < windLoop; i++)
            {
                freeSpaces.Add(new IntVector3(1, 0, i));
                freeSpaces.Add(new IntVector3(maxX, maxY + 1, i));
            }

            // Create the node graph
            List<IntVector2> possibleDirections = IntVector2.CardinalDirections.Union(new List<IntVector2> { new IntVector2(0, 0) }).ToList();
            nodeLookup = new Dictionary<IntVector3, WindyNode>();
            // first, create all the nodes
            Console.WriteLine("Creating nodes...");
            foreach (IntVector3 nodePos in freeSpaces)
            {
                WindyNode node = new WindyNode(nodePos);
                nodeLookup.Add(nodePos, node);
            }
            Console.WriteLine(nodeLookup.Count + " nodes created");
            // Then link them up
            Console.WriteLine("Linking nodes...");
            foreach (IntVector3 nodePos in freeSpaces)
            {
                IntVector2 flatPos = new IntVector2(nodePos.X, nodePos.Y);
                WindyNode node = nodeLookup[nodePos];
                foreach (IntVector2 dir in possibleDirections)
                {

                    IntVector2 newFlatPos = flatPos.Add(dir);
                    IntVector3 newPos = new IntVector3(newFlatPos.X, newFlatPos.Y, (nodePos.Z + 1) % windLoop);

                    // check for walls
                    if (newFlatPos.X < 1)
                    {
                        continue;
                    }
                    if (newFlatPos.X > maxX)
                    {
                        continue;
                    }
                    if (newFlatPos.Y < 0)
                    {
                        continue;
                    }
                    if (newFlatPos.Y > maxY + 1)
                    {
                        continue;
                    }
                    if (newFlatPos.Y == 0 && newFlatPos.X != 1)
                    {
                        continue;
                    }
                    if (newFlatPos.Y == maxY + 1 && newFlatPos.X != maxX)
                    {
                        continue;
                    }

                    // check for wind
                    if (!freeSpaces.Contains(newPos))
                    {
                        continue;
                    }
                    // nothing blocking, so link it
                    node.AddConnection(nodeLookup[newPos]);
                }
            }

        }

        private bool Looped(HashSet<IntVector2> workingNorth, HashSet<IntVector2> workingEast, HashSet<IntVector2> workingSouth, HashSet<IntVector2> workingWest)
        {
            foreach (IntVector2 pos in workingNorth)
            {
                if (!northWinds.Contains(pos))
                {
                    return false;
                }
            }
            foreach (IntVector2 pos in workingEast)            
            {
                if (!eastWinds.Contains(pos))
                {
                    return false;
                }
            }
            foreach (IntVector2 pos in workingSouth)
            {
                if (!southWinds.Contains(pos))
                {
                    return false;
                }
            }
            foreach (IntVector2 pos in workingWest)
            {
                if (!westWinds.Contains(pos))
                {
                    return false;
                }
            }
            return true;


        }

        public static void DrawMaze(HashSet<IntVector2> north, HashSet<IntVector2> east, HashSet<IntVector2> south, HashSet<IntVector2> west, int maxX, int maxY)
        {  
            Console.WriteLine(new String('#', maxX + 1));
            for (int y = 1; y <= maxY; y++)
            {
                Console.Write('#');
                for (int x = 1; x <= maxX; x++)
                {
                    IntVector2 pos = new IntVector2(x, y);
                    if (north.Contains(pos))
                    {
                        Console.Write('^');
                    }
                    else if (east.Contains(pos))
                    {
                        Console.Write('>');
                    }
                    else if (south.Contains(pos))
                    {
                        Console.Write('v');
                    }
                    else if (west.Contains(pos))
                    {
                        Console.Write('<');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine('#');
            }
            Console.WriteLine(new String('#', maxX + 1));

        }

        internal int Distance(IntVector2 start, IntVector2 end)
        {
            return Distance(start, end, 0); 
        }

        internal int Distance(IntVector2 start, IntVector2 end, int startTime)
        {
            // get the start node
            WindyNode startNode = nodeLookup[new IntVector3(start.X, start.Y, startTime % windLoop)];

            // clear the endness from all the nodes
            foreach (WindyNode node in nodeLookup.Values)
            {
                node.IsEnd = false;
            }
            // set the end nodes
            for (int i = 0; i < windLoop; i++)
            {
                IntVector3 endPos = new IntVector3(end.X, end.Y, i);
                if (nodeLookup.ContainsKey(endPos))
                {
                    WindyNode endNode = nodeLookup[endPos];
                    endNode.IsEnd = true;
                }
            }
            // now we have a tree. BFS it to find end
            int time = startTime;
            Queue<WindyNode> states = new Queue<WindyNode>();
            HashSet<WindyNode> visitedNodes = new HashSet<WindyNode>();
            states.Enqueue(startNode);
            do
            {
                Queue<WindyNode> newStates = new Queue<WindyNode>();
                while (states.Count > 0)
                {
                    WindyNode node = states.Dequeue();
                    if (node.IsEnd)
                    {
                        return time;
                    }
                    if (visitedNodes.Contains(node))
                    {
                        continue;
                    }
                    Console.WriteLine(time + " " + node.X + " " + node.Y);
                    visitedNodes.Add(node);
                    foreach (WindyNode nextNode in node.Connections)
                    {
                        newStates.Enqueue(nextNode);
                    }
                }
                states = newStates;
                time++;
            } while (states.Count > 0);
            Console.WriteLine("Ran out of states");
            return time;
        }

        internal IntVector2 Start()
        {
            return new IntVector2(1, 0);
        }

        internal IntVector2 End()
        {
            return new IntVector2(maxX, maxY + 1);
        }
    }

    internal class WindyNode
    {
        IntVector3 nodePos;
        bool isEnd;
        List<WindyNode> connections;

        public WindyNode(IntVector3 newPos)
        {
            this.nodePos = newPos;
            isEnd = false;
            connections = new List<WindyNode>();
        }

        internal List<WindyNode> Connections { get => connections;}

        internal int X { get => nodePos.X; }
        internal int Y { get => nodePos.Y; }
        internal int Z { get => nodePos.Z; }
        public bool IsEnd { get => isEnd; set => isEnd = value; }

        internal void AddConnection(WindyNode connectedNode)
        {
            connections.Add(connectedNode);
        }
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                WindyNode v = (WindyNode)obj;
                return (X == v.X) && (Y == v.Y) && (Z == v.Z);
            }
        }

        public override int GetHashCode()
        {
            return nodePos.GetHashCode();
        }

        public override string ToString()
        {
            return nodePos.ToString();
        }

        internal void SetEnd()
        {
            isEnd = true;
        }
    }
}
