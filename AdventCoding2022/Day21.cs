using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day21
    {
        public static string A(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            List<MonkeyMaths> monkeys = new List<MonkeyMaths>();
            Dictionary<string, MonkeyMaths> monkeyLookup = new Dictionary<string, MonkeyMaths>();
            MonkeyMaths root = null;
            foreach (string line in lines)
            {
                string[] parts = line.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 2)
                {
                    // It's a maths monkey
                    // Make a barebones monkey for pointing purposes
                    MathMonkey monkey = null;
                    switch (parts[2])
                    {
                        case "+":
                            monkey = new AddMonkey(parts[0]);
                            break;
                        case "-":
                            monkey = new SubtractMonkey(parts[0]);
                            break;
                        case "*":
                            monkey = new MultiplyMonkey(parts[0]);
                            break;
                        case "/":
                            monkey = new DivideMonkey(parts[0]);
                            break;
                        default:
                            throw new Exception("Unknown operation");
                    }
                    monkeys.Add(monkey);
                    monkeyLookup.Add(parts[0], monkey);
                    if (parts[0] == "root")
                    {
                        root = monkey;
                    }
                }
                else
                {
                    // It's a constant monkey
                    MonkeyMaths monkey = new ConstantMonkey(parts[0], Int64.Parse(parts[1]));
                    monkeyLookup.Add(parts[0], monkey);
                }
            }

            foreach (string line in lines)
            {
                // Now add the links
                string[] parts = line.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 2)
                {
                    MathMonkey monkey = (MathMonkey)monkeyLookup[parts[0]];
                    monkey.Operand1 = monkeyLookup[parts[1]];
                    monkey.Operand2 = monkeyLookup[parts[3]];
                }
            }

            return root.Result().ToString();
        }

        public static string B(string input)
        {
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            List<MonkeyMaths> monkeys = new List<MonkeyMaths>();
            Dictionary<string, MonkeyMaths> monkeyLookup = new Dictionary<string, MonkeyMaths>();
            MonkeyMaths root = null;
            foreach (string line in lines)
            {
                string[] parts = line.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 2)
                {
                    // It's a maths monkey
                    // Make a barebones monkey for pointing purposes
                    MathMonkey monkey = null;
                    switch (parts[2])
                    {
                        case "+":
                            monkey = new AddMonkey(parts[0]);
                            break;
                        case "-":
                            monkey = new SubtractMonkey(parts[0]);
                            break;
                        case "*":
                            monkey = new MultiplyMonkey(parts[0]);
                            break;
                        case "/":
                            monkey = new DivideMonkey(parts[0]);
                            break;
                        default:
                            throw new Exception("Unknown operation");
                    }
                    monkeys.Add(monkey);
                    monkeyLookup.Add(parts[0], monkey);
                    if (parts[0] == "root")
                    {
                        root = monkey;
                    }
                }
                else
                {
                    // It's a constant monkey
                    MonkeyMaths monkey = new ConstantMonkey(parts[0], Int64.Parse(parts[1]));
                    monkeyLookup.Add(parts[0], monkey);
                }
            }

            foreach (string line in lines)
            {
                // Now add the links
                string[] parts = line.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 2)
                {
                    MathMonkey monkey = (MathMonkey)monkeyLookup[parts[0]];
                    monkey.Operand1 = monkeyLookup[parts[1]];
                    monkey.Operand2 = monkeyLookup[parts[3]];
                }
            }

            // We need to compare the two sides of monkeys
            MonkeyMaths a = ((MathMonkey)root).Operand1;
            MonkeyMaths b = ((MathMonkey)root).Operand2;

            // And we need to change the HUMN monkey
            ConstantMonkey humn = (ConstantMonkey)monkeyLookup["humn"];


            for (long i = 290; i < 310; i++)
            {
                humn.Value = i;
                Console.WriteLine(i + " " + a.Result() + " " + b.Result() + " " + (a.Result() == b.Result()));
            }


            // Binary search time?

            long bottom = long.MinValue / 100;
            long top = long.MaxValue / 100;
            long answer = 0;

            // test the ends to see if the difference increases or decreases over the range
            bool increasing = false;
            if (Solve(bottom, a, b, humn) < Solve(top, a, b, humn))
            {
                increasing = true;
            }

            while (true)
            {

                long testValue = (bottom + top) / 2;
                long testAnswer = Solve(testValue, a, b, humn);
                //Console.WriteLine(bottom + " - " + top + " -> " + testValue + ": " + aResult + "=" + bResult);
                if (increasing)
                {
                    if (testAnswer > 0)
                    {
                        // Too high
                        top = testValue;
                    }
                    else if (testAnswer < 0)

                    {
                        bottom = testValue;
                    }
                    else
                    {
                        answer = testValue;
                        break;
                    }
                }
                else
                {
                    if (testAnswer <0)
                    {
                        // Too high
                        top = testValue;
                    }
                    else if (testAnswer > 0)
                    {
                        bottom = testValue;
                    }
                    else
                    {
                        answer = testValue;
                        break;
                    }

                }
            }
            /*
            for (long i = 290; i < 310; i++)
            {
                humn.Value = i;
                Console.WriteLine(i + " " + a.Result() + " " + b.Result() + " " + (a.Result() == b.Result()));
            }
            */
            // We've found one answer. How many other answers are there?
            long startAnswer = answer;
            long endAnswer = answer;
            while (true)
            {
                startAnswer--;
                humn.Value = startAnswer;
                long aResult = a.Result();
                long bResult = b.Result();
                if (aResult != bResult)
                {
                    startAnswer++;
                    break;
                }
            }
            while (true)
            {
                endAnswer++;
                humn.Value = endAnswer;
                long aResult = a.Result();
                long bResult = b.Result();
                if (aResult != bResult)
                {
                    endAnswer--;
                    break;
                }
            }
            Console.WriteLine(startAnswer.ToString() + " - " + endAnswer.ToString());
            return startAnswer.ToString();
        }

        private static long Solve(long value, MonkeyMaths a, MonkeyMaths b, ConstantMonkey m)
        {
            m.Value = value;
            return b.Result() - a.Result();
        }
    }

    internal abstract class MonkeyMaths
    {
        protected string name;

        public abstract bool CanAnswer();

        public abstract long Result();

    }

    internal class ConstantMonkey:MonkeyMaths
    {
        long value;

        public long Value { get => value; set => this.value = value; }

        public override bool CanAnswer()
        {
            return true;
        }

        public override long Result()
        {
            return value;
        }


        public ConstantMonkey(string name, long value)
        {
            this.value = value;
        }
    }

    internal abstract class MathMonkey:MonkeyMaths
    {
        protected MonkeyMaths operand1;
        protected MonkeyMaths operand2;

        internal MonkeyMaths Operand1 { get => operand1; set => operand1 = value; }
        internal MonkeyMaths Operand2 { get => operand2; set => operand2 = value; }

        public override bool CanAnswer()
        {
            return operand1.CanAnswer() && operand2.CanAnswer();
        }

        public MathMonkey(string name)
        {
            this.name = name;
        }


    }

    internal class AddMonkey:MathMonkey
    {
        public AddMonkey(string name) : base(name)
        {
        }

        public override long Result()
        {
            return operand1.Result() + operand2.Result();
        }

    }
    internal class SubtractMonkey : MathMonkey
    {
        public SubtractMonkey(string name) : base(name)
        {
        }

        public override long Result()
        {
            return operand1.Result() - operand2.Result();
        }
    }
    internal class MultiplyMonkey : MathMonkey
    {
        public MultiplyMonkey(string name) : base(name)
        {
        }

        public override long Result()
        {
            return operand1.Result() * operand2.Result();
        }
    }
    internal class DivideMonkey : MathMonkey
    {
        public DivideMonkey(string name) : base(name)
        {
        }

        public override long Result()
        {
            return operand1.Result() / operand2.Result();
        }
    }

}
