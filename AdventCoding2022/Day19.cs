using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day19
    {
        public static string A(string input)
        {
            long total = 0;
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                int[,] recipes = new int[4, 4];
                // Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
                string[] parts = line.Split(new string[] { ": ", ". ", "." }, StringSplitOptions.RemoveEmptyEntries);
                int blueprintID = Int32.Parse(parts[0].Split(new char[] { ' ' })[1]);
                for (int i = 1; i < 5; i++)
                {
                    string[] bytes = parts[i].Split(new string[] { "Each ", " robot costs ", " and " }, StringSplitOptions.RemoveEmptyEntries);
                    int resultId = OreNameToId(bytes[0]);
                    for (int j = 1; j < bytes.Length; j++)
                    {
                        string[] bits = bytes[j].Split(new char[] { ' ' });
                        int inputId = OreNameToId(bits[1]);
                        recipes[resultId, inputId] = Int32.Parse(bits[0]);
                    }
                }

                int[] maxBots = new int[] { 0, 0, 0, 0 };
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        maxBots[j] = Math.Max(maxBots[j], recipes[i, j]);
                    }
                }
                Queue<OreBotState> newStateQueue = new Queue<OreBotState>();
                newStateQueue.Enqueue(new OreBotState(new int[] { 1, 0, 0, 0 }, new int[] { 0, 0, 0, 0 }));
                int time = 0;
                do
                {
                    Queue<OreBotState> stateQueue = newStateQueue;
                    newStateQueue = new Queue<OreBotState>();
                    HashSet<OreBotState> newStates = new HashSet<OreBotState>();
                    while (stateQueue.Count > 0)
                    {
                        OreBotState state = stateQueue.Dequeue();
                        // Building a geode bot is always the best choice if we can
                        // Building a better bot is better than making a worse bot
                        for (int i = 3; i >= 0; i--)
                        {
                            if (state.CanBuild(recipes, maxBots, i))
                            {
                                newStates.Add(state.Build(recipes, i));
                                break;
                            }
                        }
                        // Couldn't build anything. Idle
                        newStates.Add(state.Idle());
                        
                    }
                    // Remove dupes -- FREE* WITH HASHSET!
                    // create next queue
                    Console.WriteLine(time + " " + newStates.Count);
                    /*
                    foreach (OreBotState state in newStates)
                    {
                        Console.WriteLine(state.ToString());
                    }
                    */
                    /*
                    int timeBest = 0;
                    foreach (OreBotState state in newStates)
                    {
                        timeBest = Math.Max(timeBest, state.GeodeCount());
                    }
                    if (timeBest > 0)
                    {
                        foreach( OreBotState state in newStates)
                        {
                            if (state.GeodeCount() == timeBest)
                            {
                                Console.WriteLine(state.ToString());    
                            }
                        }
                    }
                    */
                    foreach (OreBotState state in newStates)
                    {
                        newStateQueue.Enqueue(state);
                    }
                    time++;
                } while (time <= 23);
                // now we have a queue of the finished states. Go through each and find the highest geode count
                int best = 0;
                while (newStateQueue.Count > 0)
                {
                    OreBotState state = newStateQueue.Dequeue();
                    best = Math.Max(best, state.GeodeCount());
                }
                total = total + best * blueprintID;
            }
            return total.ToString();
        }

        private static int OreNameToId(string name)
        {
            switch (name)
            {
                case "ore":
                    return 0;
                case "clay":
                    return 1;
                case "obsidian":
                    return 2;
                case "geode":
                    return 3;
                default:
                    throw new Exception("Unknown ore type");
            }
        }
    }

    internal class OreBotState
    {
        private int[] botCounts;
        private int[] oreCounts;

        public OreBotState(int[] botCounts, int[] oreCounts)
        {
            this.botCounts = botCounts;
            this.oreCounts = oreCounts;
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                OreBotState v = (OreBotState)obj;
                for (int i = 0; i < 4; i++)
                {
                    if (botCounts[i] != v.botCounts[i])
                    {
                        return false;
                    }
                    if (oreCounts[i] != v.oreCounts[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public override int GetHashCode()
        {
            return botCounts.GetHashCode() ^ oreCounts.GetHashCode();
        }

        internal OreBotState Build(int[,] recipes, int botType)
        {
            int[] newOreCounts = new int[4];
            for (int i = 0; i < 4; i++)
            {
                newOreCounts[i] = oreCounts[i] - recipes[botType, i] + botCounts[i];
            }

            int[] newBotCounts = (int[])botCounts.Clone();
            newBotCounts[botType]++;
            return new OreBotState(newBotCounts, newOreCounts);

        }

        internal bool CanBuild(int[,] recipes, int[] maxBots, int botType)
        {
            if (botType < 3 && botCounts[botType] >= maxBots[botType])
            {
                return false;
            }
            for (int i = 0; i < 4; i++)
            {
                if (oreCounts[i] < recipes[botType, i])
                {
                    return false;
                }
            }
            return true;
        }

        internal OreBotState Idle()
        {
            int[] newOreCounts = (int[])oreCounts.Clone();
            for (int i = 0; i < 4; i++)
            {
                newOreCounts[i] += botCounts[i];
            }
            return new OreBotState((int[])botCounts.Clone(), newOreCounts);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < 4; i++)
            {
                sb.Append(botCounts[i].ToString());
                if (i != 3)
                {
                    sb.Append(",");
                }
            }
            sb.Append(") (");
            for (int i = 0; i < 4; i++)
            {
                sb.Append(oreCounts[i].ToString());
                if (i != 3)
                {
                    sb.Append(",");
                }
            }
            sb.Append(')');
            return sb.ToString();
        }

        internal int GeodeCount()
        {
            return oreCounts[3];
        }
    }
}