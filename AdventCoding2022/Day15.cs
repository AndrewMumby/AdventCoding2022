using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day15
    {
        public static string A(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            // get the target line
            int targetLine = Convert.ToInt32(lines[0]);

            // Process the rest
            List<Tuple<IntVector2, int>> sensors = new List<Tuple<IntVector2, int>>();
            HashSet<IntVector2> beacons = new HashSet<IntVector2>();
            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(new char[] { ' ', '=', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                IntVector2 start = new IntVector2(Convert.ToInt32(parts[3]), Convert.ToInt32(parts[5]));
                IntVector2 end = new IntVector2(Convert.ToInt32(parts[11]), Convert.ToInt32(parts[13]));
                int distance = start.Distance(end);
                sensors.Add(new Tuple<IntVector2, int>(start, distance));
                beacons.Add(end);
            }

            // ... where to start?
            // Find the closest sensor to the y line, check it reaches it, then use its x?
            // run from min(x-size) to max(x+size)
            int minX = Int32.MaxValue;
            int maxX = Int32.MinValue;

            foreach (Tuple<IntVector2, int> sensor in sensors)
            {
                minX = Math.Min(minX, sensor.Item1.X - sensor.Item2);
                maxX = Math.Max(maxX, sensor.Item1.X + sensor.Item2);
            }

            int count = 0;
            for (int x = minX; x <= maxX; x++)
            {

                if (beacons.Contains(new IntVector2(x, targetLine)))
                {
                    continue;
                }

                foreach (Tuple<IntVector2, int> sensor in sensors)
                {
                    if (sensor.Item1.Distance(x, targetLine) <= sensor.Item2)
                    {
                        count++;
                        break;
                    }
                }
            }

            return count.ToString();
        }

        public static string B(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            // get the area size
            int areaSize = Convert.ToInt32(lines[0])*2;

            // Process the rest
            List<Tuple<IntVector2, int>> sensors = new List<Tuple<IntVector2, int>>();
            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(new char[] { ' ', '=', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                IntVector2 start = new IntVector2(Convert.ToInt32(parts[3]), Convert.ToInt32(parts[5]));
                IntVector2 end = new IntVector2(Convert.ToInt32(parts[11]), Convert.ToInt32(parts[13]));
                int distance = start.Distance(end);
                sensors.Add(new Tuple<IntVector2, int>(start, distance));
            }

            // follow round the outside of each diamond, inside the bounds, looking for a hole
            foreach (Tuple<IntVector2, int> sensor in sensors)
            {
                IntVector2 location = sensor.Item1;
                int size = sensor.Item2;
                for (int i = 0; i <= size + 1; i++)
                {
                    /* size = 3
                     * 
                     *          x
                     *         xxx
                     *        xxxxx
                     *       xxxOxxx
                     *        xxxxx
                     *         xxx
                     *          x
                     *
                     */
                    int xDiff = i;
                    int YDiff = size + 1 - i;
                    if (CheckSweepAndInside(location.X - xDiff, location.Y - YDiff, areaSize, sensors))
                    {
                        long x = (location.X - xDiff);
                        long y = (location.Y - YDiff);
                        return ((x * 4000000) + y).ToString();
                    }
                    if (CheckSweepAndInside(location.X + xDiff, location.Y - YDiff, areaSize, sensors))
                    {
                        long x = (location.X + xDiff);
                        long y = (location.Y - YDiff);
                        return ((x * 4000000) + y).ToString();
                    }
                    if (CheckSweepAndInside(location.X - xDiff, location.Y + YDiff, areaSize, sensors))
                    {
                        long x = (location.X - xDiff);
                        long y = (location.Y + YDiff);
                        return ((x * 4000000) + y).ToString();
                    }
                    if (CheckSweepAndInside(location.X + xDiff, location.Y + YDiff, areaSize, sensors))
                    {
                        long x = (location.X + xDiff);
                        long y = (location.Y + YDiff);
                        return ((x * 4000000) + y).ToString();
                    }

                }
            }
            throw new Exception ("No hole found!");
        }

        private static bool CheckSweepAndInside(int x, int y, int area, List<Tuple<IntVector2, int>> sensors)
        {
            if (x < 0 || x > area)
            {
                return false;
            }
            if (y < 0 || y > area)
            {
                return false;
            }
            foreach (Tuple<IntVector2, int> sensor in sensors)
            {
                if (sensor.Item1.Distance(x, y) <= sensor.Item2)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
