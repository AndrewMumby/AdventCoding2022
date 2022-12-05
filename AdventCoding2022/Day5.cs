using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day5
    {
        public static string A(string input)
        {
            string[] inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // Split up the initial state and the instructions
            List<string> initialState = new List<string>();
            List<string> instructions = new List<string>();
            int i = 0;
            string line = inputList[0];
            do
            {
                initialState.Add(line);
                i++;
                line = inputList[i];
            } while (line != "");
            i++;
            do
            {
                instructions.Add(inputList[i]);
                i++;
            } while (i < inputList.Length);

            List<Stack<char>> stacks = ParseInitialState(initialState);

            foreach (string instruction in instructions)
            {
                // move 1 from 2 to 1
                int[] instructionParts = instruction.Split(new string[] { "move ", " from ", " to " }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToArray();
                for (int repeatCount = 0; repeatCount < instructionParts[0]; repeatCount++)
                {
                    stacks[instructionParts[2] - 1].Push(stacks[instructionParts[1] - 1].Pop());
                }
            }
            string returnValue = "";
            foreach (Stack<char> stack in stacks)
            {
                returnValue += stack.First();
            }

            return returnValue;
        }

        public static string B(string input)
        {
            string[] inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // Split up the initial state and the instructions
            List<string> initialState = new List<string>();
            List<string> instructions = new List<string>();
            int i = 0;
            string line = inputList[0];
            do
            {
                initialState.Add(line);
                i++;
                line = inputList[i];
            } while (line != "");
            i++;
            do
            {
                instructions.Add(inputList[i]);
                i++;
            } while (i < inputList.Length);

            List<Stack<char>> stacks = ParseInitialState(initialState);

            foreach (string instruction in instructions)
            {
                Stack<char> tempStack = new Stack<char>();
                // move 1 from 2 to 1
                int[] instructionParts = instruction.Split(new string[] { "move ", " from ", " to " }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToArray();
                for (int repeatCount = 0; repeatCount < instructionParts[0]; repeatCount++)
                {
                    tempStack.Push(stacks[instructionParts[1] - 1].Pop());
                }
                while (tempStack.Count > 0)
                {
                    stacks[instructionParts[2]-1].Push(tempStack.Pop());
                }
            }
            string returnValue = "";
            foreach (Stack<char> stack in stacks)
            {
                returnValue += stack.First();
            }

            return returnValue;
        }


        private static List<Stack<char>> ParseInitialState(List<string> initialState)
        {
            List<Stack<char>> state = new List<Stack<char>>();
            // flip it on its head
            initialState.Reverse();
            // The first line gives us the number of stacks
            for (int i = 0; i < initialState[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length; i++)
            {
                state.Add(new Stack<char>());
            }

            for (int line = 1; line < initialState.Count; line++)
            {
                //[Z] [M] [P]
                //0123456789

                // first one is at 1. next is at 4(n-1)+2
                int stackNo = 0;
                int index = 1;
                while (index < initialState[line].Length)
                {
                    if (initialState[line][index] != ' ')
                    {
                        state[stackNo].Push(initialState[line][index]);
                    }
                    stackNo++;
                    index += 4;
                }
            }

            return state;
        }
    }
}
