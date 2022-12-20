using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day20
    {
        public static string A(string input)
        {

            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Queue<IntLL> queue = new Queue<IntLL>();
            IntLL first = new IntLL(Int64.Parse(lines[0]));
            IntLL current = first;
            queue.Enqueue(first);
            for (int i = 1; i < lines.Length; i++)
            {
                IntLL prev = current;
                current = new IntLL(Int64.Parse(lines[i]));
                queue.Enqueue(current);
                prev.Next = current;
                current.Prev = prev;
            }
            current.Next = first;
            first.Prev = current;

            while(queue.Count > 0)
            {
                IntLL item = queue.Dequeue();
                long value = item.Value;
                IntLL next = item.Next;
                IntLL prev = item.Prev;
                // Stitch together the neighbours
                next.Prev = prev;
                prev.Next = next;

                // go forwards (or backwards) value entries
                bool negative = value < 0;
                value = Math.Abs(value);
                for (int i = 0; i < value; i++)
                {
                    if (negative)
                    {
                        next = prev;
                        prev = next.Prev;
                    }
                    else
                    {
                        prev = next;
                        next = next.Next;
                    }
                }

                // insert the item into this position
                prev.Next = item;
                next.Prev = item;
                item.Next = next; 
                item.Prev = prev;
            }

            // Find the zero
            current = first;
            while (current.Value != 0)
            {
                current = current.Next;
            }

            long total = 0;
            for (int count = 0; count < 3; count++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    // Go forwards  1000
                    current = current.Next;

                }
                total = total + current.Value;
            }
            return total.ToString();
        }

        public static string B (string input)
        {
            int key = 811589153;
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Queue<IntLL> queue = new Queue<IntLL>();
            Queue<IntLL> newQueue = new Queue<IntLL>();
            IntLL first = new IntLL(key * Int64.Parse(lines[0]));
            IntLL current = first;
            queue.Enqueue(first);
            int count = 1;
            for (int i = 1; i < lines.Length; i++)
            {
                IntLL prev = current;
                current = new IntLL(key * Int64.Parse(lines[i]));
                queue.Enqueue(current);
                prev.Next = current;
                current.Prev = prev;
                count++;
            }
            current.Next = first;
            first.Prev = current;

            for (int mixCount = 0; mixCount < 10; mixCount++)
            {
                while (queue.Count > 0)
                {

                    IntLL item = queue.Dequeue();
                    newQueue.Enqueue(item);
                    long value = item.Value;
                    IntLL next = item.Next;
                    IntLL prev = item.Prev;
                    // Stitch together the neighbours
                    next.Prev = prev;
                    prev.Next = next;

                    // go forwards (or backwards) value entries
                    bool negative = value < 0;
                    value = Math.Abs(value);
                    for (int i = 0; i < value % (count - 1); i++)
                    {
                        if (negative)
                        {
                            next = prev;
                            prev = next.Prev;
                        }
                        else
                        {
                            prev = next;
                            next = next.Next;
                        }
                    }

                    // insert the item into this position
                    prev.Next = item;
                    next.Prev = item;
                    item.Next = next;
                    item.Prev = prev;
                }
                queue = newQueue;
                newQueue = new Queue<IntLL>();
            }

            // Find the zero
            current = first;
            while (current.Value != 0)
            {
                current = current.Next;
            }

            long total = 0;
            for (int threeCount = 0; threeCount < 3; threeCount++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    // Go forwards  1000
                    current = current.Next;

                }
                total = total + current.Value;
            }
            return total.ToString();

        }
    }

    internal class IntLL
    {
        IntLL prev;
        IntLL next;
        long value;

        public long Value { get => value; }
        internal IntLL Prev { get => prev; set => prev = value; }
        internal IntLL Next { get => next; set => next = value; }

        public IntLL(long value)
        {
            this.value = value;
            prev = null;
            next = null;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}
