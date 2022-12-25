using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day19
    {
        private static Dictionary<int, int> bestGeodeAtTurn;

        private static int maxTime;
        public static string A(string input)
        {
            bestGeodeAtTurn = new Dictionary<int, int>();
            maxTime = 23;
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int total = 0;
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

                bestGeodeAtTurn = new Dictionary<int, int>();
                int best = Choices(recipes, maxBots, new int[] { 1, 0, 0, 0 }, new int[] { 0, 0, 0, 0 }, 0);
                total = total + blueprintID * best;
                Console.WriteLine(blueprintID + " " + best + " " + blueprintID * best + " " + total);
            }

            return total.ToString();
        }

        public static string B(string input)
        {
            bestGeodeAtTurn = new Dictionary<int, int>();
            maxTime = 31;
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int total = 1;
            for (int lineNo = 0; lineNo < Math.Min(lines.Length, 3); lineNo++)
            {
                string line = lines[lineNo];
                int[,] recipes = new int[4, 4];
                // Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
                string[] parts = line.Split(new string[] { ": ",  ". ", "." }, StringSplitOptions.RemoveEmptyEntries);
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

                bestGeodeAtTurn = new Dictionary<int, int>();
                int best = Choices(recipes, maxBots, new int[] { 1, 0, 0, 0 }, new int[] { 0, 0, 0, 0 }, 0);
                total = total * best;
                //Console.WriteLine(blueprintID + " " + best + " " + total);
            }

            return total.ToString();
        }

        private static int Choices(int[,] recipes, int[] maxBots, int[] botCounts, int[] oreCounts, int time)
        {
            /*
            if (bestGeodeAtTurn.ContainsKey(time))
            {
                if (oreCounts[3] < bestGeodeAtTurn[time])
                {
                    return 0;
                }
            }
            */
            int bestScore = 0;
            if (CanMake(recipes, 3, oreCounts))
            {
                bestScore = MakeBot(3, recipes, maxBots, botCounts, oreCounts, time);
            }
            else
            {
                if (CanEventuallyMake(recipes, 3, botCounts))
                {
                    bestScore = Math.Max(bestScore, MakeBot(3, recipes, maxBots, botCounts, oreCounts, time));
                }
                if (botCounts[2] < maxBots[2] && CanEventuallyMake(recipes, 2, botCounts))
                {
                    bestScore = Math.Max(bestScore, MakeBot(2, recipes, maxBots, botCounts, oreCounts, time));
                }
                if (botCounts[1] < maxBots[1] && CanEventuallyMake(recipes, 1, botCounts))
                {
                    bestScore = Math.Max(bestScore, MakeBot(1, recipes, maxBots, botCounts, oreCounts, time));
                }
                if (botCounts[0] < maxBots[0] && CanEventuallyMake(recipes, 0, botCounts))
                {
                    bestScore = Math.Max(bestScore, MakeBot(0, recipes, maxBots, botCounts, oreCounts, time));
                }
            }
            
            return bestScore;
        }

        private static bool CanEventuallyMake(int[,] recipes, int recipeId, int[] botCounts)
        {
            for (int i = 0; i < 4; i++)
            {
                if (recipes[recipeId, i] >0 && botCounts[i] ==0)
                {
                    return false;
                }
            }
            return true;

        }

        private static bool CanMake(int[,] recipes, int recipeId, int[] oreCounts)
        {
            for (int i = 0; i < 4; i++)
            {
                if (recipes[recipeId, i]> oreCounts[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static int MakeBot(int botId, int[,] recipes, int[] maxBots, int[] botCounts, int[] oreCounts, int time)
        {
            /*
            if (bestGeodeAtTurn.ContainsKey(time))
            {
                if (oreCounts[3] < bestGeodeAtTurn[time])
                {
                    return 0;
                }
                bestGeodeAtTurn[time] = oreCounts[3];
            }
            else
            {
                bestGeodeAtTurn.Add(time, oreCounts[3]);
            }
            */
            if (time > maxTime)
            {
                return oreCounts[3];
            }
            bool canMakeBot = CanMake(recipes, botId, oreCounts);
            int[] newOreCounts = new int[4];
            for (int i = 0; i < 4; i++)
            {
                newOreCounts[i] = oreCounts[i] + botCounts[i];
            }
            if (canMakeBot)
            {
                int[] newBotCounts = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    newOreCounts[i] = newOreCounts[i] - recipes[botId, i];
                    newBotCounts[i] = botCounts[i];
                }
                newBotCounts[botId]++;
                return Choices(recipes, maxBots, newBotCounts, newOreCounts, time+1);
            }
            else
            {
                return MakeBot(botId, recipes, maxBots, botCounts, newOreCounts, time+1);
            }
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
}