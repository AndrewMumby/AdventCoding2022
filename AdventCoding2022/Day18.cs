using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day18
    {
        public static string A(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            HashSet<IntVector3> cubes = new HashSet<IntVector3>();
            foreach (string line in lines)
            {
                // 2,2,2
                string[] parts = line.Split(new char[] { ',' });
                cubes.Add(new IntVector3(Int32.Parse(parts[0]), Int32.Parse(parts[1]), Int32.Parse(parts[2])));
            }

            int total = 0;

            foreach (IntVector3 cube in cubes)
            {
                HashSet<IntVector3> adjacent = IntVector3.CardinalDirections.Select(x => IntVector3.Add(x, cube)).ToHashSet();
                total = total + (adjacent.Except(cubes).Count());
            }

            return total.ToString();
        }

        public static string B(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            HashSet<IntVector3> cubes = new HashSet<IntVector3>();
            foreach (string line in lines)
            {
                // 2,2,2
                string[] parts = line.Split(new char[] { ',' });
                cubes.Add(new IntVector3(Int32.Parse(parts[0]), Int32.Parse(parts[1]), Int32.Parse(parts[2])));
            }


            // Make a list of all the points adjacent to the cubes (inc dupes)
            List<IntVector3> adjacent = new List<IntVector3>();
            foreach (IntVector3 cube in cubes)
            {
                adjacent.AddRange(IntVector3.CardinalDirections.Select(x => IntVector3.Add(x, cube)));
            }

            // remove adjacent that are actually cubes
            List<IntVector3> newAdjacent = new List<IntVector3>();
            foreach (IntVector3 adj in adjacent)
            {
                if (!cubes.Contains(adj))
                {
                    newAdjacent.Add(adj);
                }
            }

            adjacent = newAdjacent;

            // Flood from the origin to one more than the max point

            int maxX = 0;
            int maxY = 0;
            int maxZ = 0;
            foreach (IntVector3 cube in adjacent)
            {
                maxX = Math.Max(maxX, cube.X);
                maxY = Math.Max(maxY, cube.Y);
                maxZ = Math.Max(maxZ, cube.Z);
            }
            maxX++;
            maxY++;
            maxZ++;


            // Now we have a bounding box

            HashSet<IntVector3> outside = new HashSet<IntVector3>();
            Queue<IntVector3> toDo = new Queue<IntVector3>();
            toDo.Enqueue(new IntVector3(-1, -1, -1));
            outside.Add(new IntVector3(-1, -1, -1));

            while (toDo.Count > 0)
            {
                IntVector3 point = toDo.Dequeue();
                HashSet<IntVector3> neighbours = IntVector3.CardinalDirections.Select(x => IntVector3.Add(x, point)).ToHashSet();
                // remove the ones outside the bounding box
                neighbours.RemoveWhere(x => x.X < -1 || x.X > maxX || x.Y < -1 || x.Y > maxY || x.Z < -1 || x.Z > maxZ);
                // remove the ones already in the set
                neighbours = neighbours.Except(outside).ToHashSet();
                // remove the ones that are cubes
                neighbours = neighbours.Except(cubes).ToHashSet();

                foreach (IntVector3 item in neighbours)
                {
                    toDo.Enqueue(item);
                    outside.Add(item);
                }
            }
            int count = 0;
            foreach (IntVector3 cube in adjacent)
            {
                if (outside.Contains(cube))
                {
                    count++;
                }
            }
            return count.ToString();

        }
    }
}
