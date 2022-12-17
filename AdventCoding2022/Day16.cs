using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day16
    {
        public static string A(string input)
        {
            HashSet<ValveRoom> rooms = ParseInput(input);
            ValveRoom initialRoom = null;
            foreach (ValveRoom room in rooms)
            {
                if (room.Name == "AA")
                    initialRoom = room;
            }
            if (initialRoom == null)
            {
                throw new Exception("Room not found");
            }

            HashSet<ValveRoom> usefulRooms = new HashSet<ValveRoom>();
            usefulRooms.Add(initialRoom);
            foreach (ValveRoom room in rooms)
            {
                if (room.FlowRate > 0)
                {
                    usefulRooms.Add(room);
                }
            }
            Dictionary<Tuple<ValveRoom, ValveRoom>, int> distances = CalcAllDistances(rooms, usefulRooms);
            return GetBestFlow(initialRoom, new HashSet<ValveRoom>(), usefulRooms, distances, 0, 0).ToString();

        }

        public static string B(string input)
        {
            HashSet<ValveRoom> rooms = ParseInput(input);
            ValveRoom initialRoom = null;
            foreach (ValveRoom room in rooms)
            {
                if (room.Name == "AA")
                    initialRoom = room;
            }
            if (initialRoom == null)
            {
                throw new Exception("Room not found");
            }

            HashSet<ValveRoom> usefulRooms = new HashSet<ValveRoom>();
            usefulRooms.Add(initialRoom);
            foreach (ValveRoom room in rooms)
            {
                if (room.FlowRate > 0)
                {
                    usefulRooms.Add(room);
                }
            }
            Dictionary<Tuple<ValveRoom, ValveRoom>, int> distances = CalcAllDistances(rooms, usefulRooms);
            usefulRooms.Remove(initialRoom);
            // Give the human and elephant different lists of useful rooms, and find the maximum
            List<ValveRoom> roomList = usefulRooms.ToList();
            int bestFlow = 0;
            Console.WriteLine(Math.Pow(2, roomList.Count));
            List<Task<int>> tasks = new List<Task<int>>();
            for (int i = 0; i < Math.Pow(2, roomList.Count)/2; i++)
            {
                tasks.Add(RunTeamVent(initialRoom, distances, roomList, i));

            }
            int count = 0;
            foreach (Task<int> task in tasks)
            {
                if (count % 100 == 0)
                {
                    Console.WriteLine(count);
                }
                count++;
                task.Wait();
            }
            foreach (Task<int> task in tasks)
            {
                bestFlow = Math.Max(bestFlow, task.Result);
            }



            return bestFlow.ToString();
        }

        private static Task<int> RunTeamVent(ValveRoom initialRoom, Dictionary<Tuple<ValveRoom, ValveRoom>, int> distances, List<ValveRoom> roomList, int i)
        {
            return Task.Run(() => { return TeamVent(initialRoom, distances,roomList, i); });
        }
        private static int TeamVent(ValveRoom initialRoom, Dictionary<Tuple<ValveRoom, ValveRoom>, int> distances, List<ValveRoom> roomList, int i)
        {
            HashSet<ValveRoom> humanRooms = new HashSet<ValveRoom>();
            HashSet<ValveRoom> elephantRooms = new HashSet<ValveRoom>();

            for (int j = 0; j < roomList.Count; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    humanRooms.Add(roomList[j]);
                }
                else
                {
                    elephantRooms.Add(roomList[j]);
                }
            }
            int humanFlow = GetBestFlow(initialRoom, new HashSet<ValveRoom>(), humanRooms, distances, 0, 4);
            int elephantFlow = GetBestFlow(initialRoom, new HashSet<ValveRoom>(), elephantRooms, distances, 0, 4);
            int totalFlow = humanFlow + elephantFlow;
            return totalFlow;
        }

        private static int GetBestFlow(ValveRoom currentRoom, HashSet<ValveRoom> turnedRooms,HashSet<ValveRoom> usefulRooms, Dictionary<Tuple<ValveRoom, ValveRoom>, int> distances, int flowTotal, int time)
        {
            int bestScore = 0;
            HashSet<ValveRoom> valvesLeft = usefulRooms.Except(turnedRooms).ToHashSet<ValveRoom>();
            if (valvesLeft.Count == 0)
            {
                int flowRate = 0;
                foreach (ValveRoom turnedRoom in turnedRooms)
                {
                    flowRate += turnedRoom.FlowRate;
                }
                return flowTotal + (flowRate * (30 - time));
            }
            foreach (ValveRoom targetRoom in valvesLeft)
            {
                if (targetRoom.Equals(currentRoom))
                {
                    continue;
                }
                if (turnedRooms.Contains(targetRoom))
                {
                    continue;
                }
                
                int distance = distances[new Tuple<ValveRoom, ValveRoom>(currentRoom, targetRoom)];
                if (distance + time > 30)
                {
                    // Not enough time to get there. Just work out how much the current flow rate will add in the remaining time
                    int flowRate = 0;
                    foreach (ValveRoom turnedRoom in turnedRooms)
                    {
                        flowRate += turnedRoom.FlowRate;
                    }
                    bestScore = Math.Max(bestScore, flowTotal + flowRate * (30 - time));
                }
                else 
                {
                    int flowRate = 0;
                    foreach (ValveRoom turnedRoom in turnedRooms)
                    {
                        flowRate += turnedRoom.FlowRate;
                    }
                    ValveRoom newCurrentRoom = targetRoom;
                    HashSet<ValveRoom> newTurnedRooms = new HashSet<ValveRoom>(turnedRooms);
                    newTurnedRooms.Add(targetRoom);
                    int newFlowTotal = flowTotal + flowRate * (distance+1);
                    int newTime = time + distance + 1;
                    bestScore = Math.Max(bestScore, GetBestFlow(newCurrentRoom, newTurnedRooms, usefulRooms, distances, newFlowTotal, newTime));
                }
            }
            return bestScore;
        }

        private static Dictionary<Tuple<ValveRoom, ValveRoom>, int> CalcAllDistances(HashSet<ValveRoom> rooms, HashSet<ValveRoom> usefulRooms)
        {
            Dictionary<Tuple<ValveRoom, ValveRoom>, int> allDistances = new Dictionary<Tuple<ValveRoom,ValveRoom>, int>();
            foreach ( ValveRoom room in usefulRooms)
            {
                Dictionary<ValveRoom, int> distances = CalcDistances(room, rooms, usefulRooms);
                foreach (KeyValuePair<ValveRoom, int> distance in distances)
                {
                    allDistances.Add(new Tuple<ValveRoom, ValveRoom>(room, distance.Key), distance.Value);
                }
            }
            return allDistances;
        }

        private static Dictionary<ValveRoom, int> CalcDistances(ValveRoom room, HashSet<ValveRoom> rooms, HashSet<ValveRoom> usefulRooms)
        {
            Dictionary<ValveRoom, int> distancesIncUseless = new Dictionary<ValveRoom, int>();
            Queue<Tuple<ValveRoom, int>> queue = new Queue<Tuple<ValveRoom, int>>();
            queue.Enqueue(new Tuple<ValveRoom, int>(room, 0));
            while (queue.Count > 0)
            {
                Tuple<ValveRoom,int> tuple = queue.Dequeue();
                if (distancesIncUseless.ContainsKey(tuple.Item1))
                {
                    if (distancesIncUseless[tuple.Item1] < tuple.Item2)
                    {
                        continue;
                    }
                    else
                    {
                        distancesIncUseless.Remove(tuple.Item1);
                    }
                }

                distancesIncUseless.Add(tuple.Item1, tuple.Item2);
                foreach (ValveRoom connected in tuple.Item1.Connections)
                {
                    queue.Enqueue(new Tuple<ValveRoom, int>(connected, tuple.Item2+1));
                }
            }
            Dictionary<ValveRoom, int> distances = new Dictionary<ValveRoom, int>();
            foreach (ValveRoom usefulRoom in usefulRooms)
            {
                distances.Add(usefulRoom, distancesIncUseless[usefulRoom]);
            }
            return distances;
        }

        private static HashSet<ValveRoom> ParseInput(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            HashSet<ValveRoom> rooms = new HashSet<ValveRoom>();

            // First, create the rooms
            foreach (string line in lines)
            {
                // Valve OU has flow rate=8; tunnels lead to valves ZV, YA, NZ, OP
                string[] parts = line.Split(new char[] { ' ', ',', '=', ';' }, StringSplitOptions.RemoveEmptyEntries);
                rooms.Add(new ValveRoom(parts[1], Int32.Parse(parts[5])));
            }

            // Then link them up
            foreach (string line in lines)
            {
                // Valve OU has flow rate=8; tunnels lead to valves ZV, YA, NZ, OP
                string[] parts = line.Split(new char[] { ' ', ',', '=' }, StringSplitOptions.RemoveEmptyEntries);
                ValveRoom room = null;

                foreach (ValveRoom testRoom in rooms)
                {
                    if (testRoom.Name == parts[1])
                    {
                        room = testRoom;
                        break;
                    }
                }

                if (room == null)
                {
                    throw new Exception("Room not found");
                }

                for (int i = 10; i < parts.Length; i++)
                {
                    foreach (ValveRoom testRoom in rooms)
                    {
                        if (testRoom.Name == parts[i])
                        {
                            room.AddConnection(testRoom);
                            break;
                        }
                    }
                }
            }
            return rooms;
        }
    }

    internal class ValveRoom
    {
        string name;
        int flowRate;
        List<ValveRoom> connections;

        public string Name { get => name; }
        public int FlowRate { get => flowRate; }
        internal List<ValveRoom> Connections { get => connections; }

        internal ValveRoom(string name, int flowRate)
        {
            this.name = name;
            this.flowRate = flowRate;
            connections = new List<ValveRoom>();
        }

        internal void AddConnection(ValveRoom neighbour)
        {
            connections.Add(neighbour);
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override string ToString()
        {
            return name.ToString();
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
                ValveRoom v = (ValveRoom)obj;
                return name == v.Name;
            }
        }
    }
}
