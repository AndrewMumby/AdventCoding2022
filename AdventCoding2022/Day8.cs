using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day8
    {

        public static string A(string input)
        {
            IntField2 treeField = ParseInput(input);
            HashSet<IntVector2> visibleTrees = new HashSet<IntVector2>();
            int bestHeight = -1;
            int x = 0;
            int y = 0;
            // The Horizontal
            bestHeight = 0;
            for (y = 0; y < treeField.GetSize(1); y++)
            {
                x = 0;
                bestHeight = -1;
                while (bestHeight != 9 && treeField.GetValue(x, y) != -1)
                {
                    if (treeField.GetValue(x, y) > bestHeight)
                    {
                        bestHeight = treeField.GetValue(x, y);
                        visibleTrees.Add(new IntVector2(x, y));
                    }
                    x++;
                }
            }
            // The other Horizontal
            for (y = 0; y < treeField.GetSize(1); y++)
            {
                x = treeField.GetSize(0)-1;
                bestHeight = -1;
                while (bestHeight != 9 && treeField.GetValue(x, y) != -1)
                {
                    if (treeField.GetValue(x, y) > bestHeight)
                    {
                        bestHeight = treeField.GetValue(x, y);
                        visibleTrees.Add(new IntVector2(x, y));
                    }
                    x--;
                }
            }
            // The Vertical
            for (x = 0; x < treeField.GetSize(0); x++)
            {
                y = 0;
                bestHeight = -1;
                while (bestHeight != 9 && treeField.GetValue(x, y) != -1)
                {
                    if (treeField.GetValue(x, y) > bestHeight)
                    {
                        bestHeight = treeField.GetValue(x, y);
                        visibleTrees.Add(new IntVector2(x, y));
                    }
                    y++;
                }
            }
            // The other Vertical
            for (x = 0; x < treeField.GetSize(0); x++)
            {
                y = treeField.GetSize(1)-1;
                bestHeight = -1;
                while (bestHeight != 9 && treeField.GetValue(x, y) != -1)
                {
                    if (treeField.GetValue(x, y) > bestHeight)
                    {
                        bestHeight = treeField.GetValue(x, y);
                        visibleTrees.Add(new IntVector2(x, y));
                    }
                    y--;
                }
            }
            return visibleTrees.Count().ToString();
        }

        public static string B (string input)
        {
            IntField2 treeField = ParseInput(input);
            int bestScore = 0;
            for (int x = 0; x < treeField.GetSize(0); x++)
            {
                for (int y = 0; y < treeField.GetSize(1); y++)
                {
                    int score = 1;
                    IntVector2 start = new IntVector2 (x, y);
                    int maxHeight = treeField.GetValue(start);
                    foreach (IntVector2 direction in IntVector2.CardinalDirections)
                    {
                        int count = 0;
                        IntVector2 cursor = new IntVector2(start);
                        do
                        {
                            cursor = cursor.Add(direction);
                            if (treeField.GetValue(cursor) != -1)
                            {
                                count++;
                            }
                        } while (treeField.GetValue(cursor) < maxHeight && treeField.GetValue(cursor) != -1);
                        score *= count;
                    }
                    bestScore = Math.Max(score, bestScore);
                }
            }
            return bestScore.ToString();    
        }


        private static IntField2 ParseInput(string input)
        { 
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            IntField2 treeField = new IntField2(inputLines[0].Length, inputLines.Length, -1);
            for (int y = 0; y < inputLines.Length;y++)
            {
                for (int x = 0; x < inputLines[0].Length;x++)
                {
                    treeField.SetValue(x, y, inputLines[y][x]-'0');
                }
            }
            return treeField;
        }

    }
}
