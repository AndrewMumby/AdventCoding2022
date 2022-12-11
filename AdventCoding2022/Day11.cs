using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day11
    {
        public static string A (string input)
        {
            string[] monkeyStrings = input.Split(new string[] { "Monkey " }, StringSplitOptions.RemoveEmptyEntries);

            List<Monkey> monkeys = new List<Monkey>();
            foreach (string monkeyString in monkeyStrings)
            {
                monkeys.Add(new Monkey(monkeyString));
            }

            for (int i = 0; i < 20; i++)
            {
                foreach (Monkey monkey in monkeys)
                {
                    monkey.ProcessQueue(monkeys);
                }
            }

            // find the two highest inspection counts
            List<int> inspectionCounts = new List<int>();
            foreach (Monkey monkey in monkeys)
            {
                inspectionCounts.Add(monkey.InspectionCount);
            }

            inspectionCounts.Sort();
            return (inspectionCounts[inspectionCounts.Count - 1] * inspectionCounts[inspectionCounts.Count-2]).ToString();

        }

        public static string B(string input)
        {
            string[] monkeyStrings = input.Split(new string[] { "Monkey " }, StringSplitOptions.RemoveEmptyEntries);

            List<Monkey> monkeys = new List<Monkey>();
            foreach (string monkeyString in monkeyStrings)
            {
                monkeys.Add(new Monkey(monkeyString));
            }
            int moduloNumber = 1;
            foreach (Monkey monkey in monkeys)
            {
                moduloNumber *= monkey.Test;
            }

            for (int i = 0; i < 10000; i++)
            {
                foreach (Monkey monkey in monkeys)
                {
                    monkey.ProcessQueueWithBullshitModuloMaths(monkeys, moduloNumber);
                }
            }

            // find the two highest inspection counts
            List<long> inspectionCounts = new List<long>();
            foreach (Monkey monkey in monkeys)
            {
                inspectionCounts.Add(monkey.InspectionCount);
            }

            inspectionCounts.Sort();
            return (inspectionCounts[inspectionCounts.Count - 1] * inspectionCounts[inspectionCounts.Count - 2]).ToString();

        }

    }

    internal class Monkey
    {
        Queue<long> items;
        Operator operation;
        int parameter; // -1 is old
        int test;
        int trueTarget;
        int falseTarget;
        int inspectionCount;

        public int InspectionCount { get => inspectionCount;}
        public int Test { get => test;}

        internal Monkey (string monkeyInput)
        {
            inspectionCount = 0;
            items = new Queue<long>();   
            //"0:\r\n  Starting items: 79, 98\r\n  Operation: new = old * 19\r\n  Test: divisible by 23\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 3\r\n\r\n"
            // Split on linebreaks
            string[] inputLines = monkeyInput.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            /*
            "0:
            Starting items: 79, 98
            Operation: new = old * 19
            Test: divisible by 23
            If true: throw to monkey 2
            If false: throw to monkey 3"
            */
            // Do the item queue
            string[] itemStrings = inputLines[1].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries); 
            for (int i = 2; i < itemStrings.Length; i++)
            {
                items.Enqueue(Convert.ToInt32(itemStrings[i]));
            }

            string[] parts = inputLines[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            // Do the operator
            if (parts[4] == "+") 
            {
                operation = Operator.Add;
            }
            else
            {
                operation = Operator.Multiply;
            }

            // Do the parameter

            if (parts[5] == "old")
            {
                parameter = -1;
            }
            else
            {
                parameter = Convert.ToInt32(parts[5]);
            }

            // Do the test
            parts = inputLines[3].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            test = Convert.ToInt32(parts[3]);

            // Do the true target
            parts = inputLines[4].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            trueTarget = Convert.ToInt32(parts[5]);

            // Do the false target
            parts = inputLines[5].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            falseTarget = Convert.ToInt32(parts[5]);
        }

        /// <summary>
        /// Processes the monkey's item queue
        /// </summary>
        /// <param name="monkeys">The list of monkeys for throwing purposes</param>
        internal void ProcessQueue(List<Monkey> monkeys)
        {
            while (items.Count > 0)
            {
                long item = items.Dequeue();

                // operation
                long parameterValue;
                if (parameter == -1)
                {
                    parameterValue = item;
                }
                else
                {
                    parameterValue = parameter;
                }

                long newValue;
                if (operation == Operator.Add)
                {
                    newValue = item + parameterValue;
                }
                else
                {
                    newValue = item * parameterValue;
                }

                // relief
                newValue = newValue / 3;

                // test
                bool testResult = ((newValue % Test) == 0);

                // throw
                if (testResult)
                {
                    //Console.WriteLine("Throwing " + newValue + " to " + trueTarget);
                    monkeys[trueTarget].AddToQueue(newValue);
                }
                else
                {
                    //Console.WriteLine("Throwing " + newValue + " to " + falseTarget);
                    monkeys[falseTarget].AddToQueue(newValue);
                }
                inspectionCount++;
            }
        }

        internal void AddToQueue(long item)
        {
            items.Enqueue(item);
        }

        internal void ProcessQueueWithBullshitModuloMaths(List<Monkey> monkeys, int moduloNumber)
        {
            while (items.Count > 0)
            {
                long item = items.Dequeue();

                // operation
                long parameterValue;
                if (parameter == -1)
                {
                    parameterValue = item;
                }
                else
                {
                    parameterValue = parameter;
                }

                long newValue;
                if (operation == Operator.Add)
                {
                    newValue = item + parameterValue;
                }
                else
                {
                    newValue = item * parameterValue;
                }

                // relief
                newValue = newValue % moduloNumber;

                // test
                bool testResult = ((newValue % Test) == 0);

                // throw
                if (testResult)
                {
                    //Console.WriteLine("Throwing " + newValue + " to " + trueTarget);
                    monkeys[trueTarget].AddToQueue(newValue);
                }
                else
                {
                    //Console.WriteLine("Throwing " + newValue + " to " + falseTarget);
                    monkeys[falseTarget].AddToQueue(newValue);
                }
                inspectionCount++;
            }
        }
    }

    internal enum Operator
    {
        Add,
        Multiply
    }
}
