using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day13
    {
        public static string A(string input)
        {
            int i = 0;
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int score = 0;
            while (i < lines.Length)
            {
                string remains;
                DistressPacket a = DistressPacket.CreatePacket(lines[i], out remains);
                Console.WriteLine(a);
                DistressPacket b = DistressPacket.CreatePacket(lines[i + 1], out remains);
                Console.WriteLine(b);

                TripBool sorted = DistressPacket.CheckSort(a, b);
                if (sorted == TripBool.True)
                {
                    score = score + (i / 2) + 1;
                }
                i += 2;
            }

            return score.ToString();

        }


        public static string B(string input)
        {
            List<DistressPacket> packets = new List<DistressPacket>();
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                string remains;
            foreach (string line in lines)
            {
                packets.Add(DistressPacket.CreatePacket(line, out remains));
            }
                packets.Add(DistressPacket.CreatePacket("[2]", out remains));
                packets.Add(DistressPacket.CreatePacket("[6]", out remains));

            packets.Sort(SortPackets);

            // Find the divider packets
            int TwoDividerPos = 0;
            int SixDividerPos = 0;

            for (int i = 0; i < packets.Count; i++)
            {
                if (packets[i].ToString()=="[2]")
                {
                    TwoDividerPos = i +1;
                }
                if (packets[i].ToString() == "[6]")
                {
                    SixDividerPos = i + 1;
                }
            }

            return (TwoDividerPos * SixDividerPos).ToString();
        }

        private static int SortPackets(DistressPacket a, DistressPacket b)
        {
            TripBool result = DistressPacket.CheckSort(a, b);
            if (result == TripBool.True)
            {
                return -1;

            }
            else if (result == TripBool.False)
            {
                return 1;
            }

            else
            {
                return 0;
            }

        }
    }

    internal class DistressPacket
    {
        public static DistressPacket CreatePacket(string input, out string output)
        {
            string remains;
            if (input[0] == '[')
            {
                // It's a list packet
                DistressPacket packet = DistressPacketList.CreatePacketList(input, out remains);
                output = remains;
                return packet;
            }
            else
            {
                // It's an int packet
                DistressPacket packet = DistressPacketInt.CreatePacketInt(input, out remains);
                output = remains;
                return packet;
            }
        }

        internal static TripBool CheckSort(DistressPacket a, DistressPacket b)
        {
            if (a is DistressPacketInt && b is DistressPacketInt)
            {
                if (((DistressPacketInt)a).Value < ((DistressPacketInt)b).Value)
                {
                    return TripBool.True;
                }
                else if (((DistressPacketInt)b).Value < ((DistressPacketInt)a).Value)
                {
                    return TripBool.False;
                }
                else
                {
                    return TripBool.Nope;
                }
            }
            else if (a is DistressPacketList && b is DistressPacketList)
            {
                List<DistressPacket> aList = ((DistressPacketList)a).Contents;
                List<DistressPacket> bList = ((DistressPacketList)b).Contents;
                for (int i = 0; i < Math.Max(aList.Count, bList.Count); i++)
                {
                    if (aList.Count <i+1)
                    {
                        return TripBool.True;
                    }
                    else if (bList.Count < i+1)
                    {
                        return TripBool.False;
                    }
                    else
                    {
                        TripBool result = CheckSort(aList[i], bList[i]);
                        if (result != TripBool.Nope)
                        {
                            return result;
                        }
                    }
                }
                return TripBool.Nope;
            }
            else
            {
                if (a is DistressPacketInt)
                {
                    string remains;
                    DistressPacket temp = DistressPacket.CreatePacket("[" + a.ToString() + "]", out remains);
                    return CheckSort(temp, b);
                }
                else
                {
                    string remains;
                    DistressPacket temp = DistressPacket.CreatePacket("[" + b.ToString() + "]", out remains);
                    return CheckSort(a, temp);

                }
            }
        }
    }

    internal class DistressPacketList : DistressPacket
    {
        List<DistressPacket> contents;

        internal List<DistressPacket> Contents { get => contents;}

        public DistressPacketList()
        {
            contents = new List<DistressPacket>();
        }

        internal static DistressPacket CreatePacketList(string input, out string output)
        {
            DistressPacketList packet = new DistressPacketList();
            // throw away the first '['
            input = input.Substring(1);
            string remains = input;
            while (input.Length > 0 && input[0] != ']')
            {
                input = input.Trim(new char[] { ',' });
                packet.Add(DistressPacket.CreatePacket(input, out remains));
                input = remains;
            }
            if (remains.Length > 0)
            {
                output = remains.Substring(1);
            }
            else
            {
                output = remains;
            }
            return packet;
        }

        private void Add(DistressPacket distressPacket)
        {
            contents.Add(distressPacket);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            foreach (DistressPacket distressPacket in contents)
            {
                sb.Append(distressPacket.ToString());
                sb.Append(',');
            }
            if (contents.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append(']');
            return sb.ToString();
        }
    }

    internal class DistressPacketInt : DistressPacket
    {
        int value;

        public DistressPacketInt(int value)
        {
            this.value = value;
        }

        public int Value { get => value;}

        public static DistressPacketInt CreatePacketInt(string input, out string output)
        {
            // Find the end of the number
            string intString = "";
            int location = 0;
            while (input[location] != ',' && input[location] != ']')
            {
                intString += input[location];
                location++;
            }
            output = input.Substring(location);
            DistressPacketInt returnPacket = new DistressPacketInt(Convert.ToInt32(intString));
            return returnPacket;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }

    enum TripBool
    {
        True,
        False,
        Nope
    }
}

