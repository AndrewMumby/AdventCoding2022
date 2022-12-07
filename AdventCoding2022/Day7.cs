using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2022
{
    internal class Day7
    {
        public static string A(string input)
        {
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Queue<string> inputQueue = new Queue<string>();
            foreach (string line in inputLines)
            {
                inputQueue.Enqueue(line);
            }
            ElFSDirectory root = new ElFSDirectory("/");
            inputQueue.Dequeue();
            root.ProcessData(inputQueue);
            if (inputQueue.Count > 0)
            {
                throw (new Exception("Data remains"));
            }

            // now find the right-sized directories
            List<int> directorySizes = root.GenerateDirectorySizeList();

            int total = 0;
            foreach (int directorySize in directorySizes)
            {
                if (directorySize <= 100000)
                {
                    total += directorySize;
                }
            }
            return total.ToString();
        }


        public static string B(string input)
        {
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Queue<string> inputQueue = new Queue<string>();
            foreach (string line in inputLines)
            {
                inputQueue.Enqueue(line);
            }
            ElFSDirectory root = new ElFSDirectory("/");
            inputQueue.Dequeue();
            root.ProcessData(inputQueue);
            if (inputQueue.Count > 0)
            {
                throw (new Exception("Data remains"));
            }
            int spaceRequired = 30000000 - (70000000 - root.Size());

            // now find the right-sized directory
            List<int> directorySizes = root.GenerateDirectorySizeList();

            directorySizes.Sort();
            foreach (int directorySize in directorySizes)
            {
                if (directorySize > spaceRequired)
                {
                    return directorySize.ToString(); 
                }
            }
            return "";
        }



        abstract internal class ElFSNode
        {
            abstract public int Size();
            abstract public string Name();
        }

        internal class ElFSDirectory : ElFSNode
        {
            private string name;
            private List<ElFSNode> contents;

            public ElFSDirectory(string name)
            {
                this.name = name;
                contents = new List<ElFSNode>();
            }
            public override string Name()
            {
                return name;
            }
            override public int Size()
            {
                int size = 0;
                foreach (ElFSNode node in contents)
                {
                    size = size + node.Size();
                }

                return size;
            }

            public void ProcessData(Queue<string> data)
            {
                /*
                    $ ls
                    dir a
                    14848514 b.txt
                    8504156 c.dat
                    dir d
                    $ cd a
                    $ ls
                    dir e
                    29116 f
                    2557 g
                    62596 h.lst
                    $ cd e
                    $ ls
                    584 i
                    $ cd ..
                    $ cd ..
                    $ cd d
                    $ ls
                    4060174 j
                    8033020 d.log
                    5626152 d.ext
                    7214296 k
                */
                string line = data.Dequeue();
                while (data.Count > 0)
                {
                    if (line == "$ ls")
                    {
                        if (data.Count > 0)
                        {
                            line = data.Dequeue();
                        }
                        else
                        {
                            return;
                        }
                        while (!line.StartsWith("$"))
                        {
                            string[] lineParts = line.Split(new char[] { ' ' });
                            if (lineParts[0] == "dir")
                            {
                                // It's a directory
                                contents.Add(new ElFSDirectory(lineParts[1]));
                            }
                            else
                            {
                                // It's a file
                                contents.Add(new ElFSFile(lineParts[1], lineParts[0]));
                            }

                            if (data.Count > 0)
                            {
                                line = data.Dequeue();
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    else if (line == "$ cd ..")
                    {
                        return;
                    }
                    else
                    {
                        // Going deeper
                        // Find the correct directory
                        string[] lineParts = line.Split(' ');
                        foreach (ElFSNode node in contents)
                        {
                            if (node is ElFSDirectory && node.Name() == lineParts[2])
                            {
                                ElFSDirectory dir = (ElFSDirectory)node;
                                dir.ProcessData(data);
                                break;
                            }
                        }
                        if (data.Count > 0)
                        {
                            line = data.Dequeue();
                        }
                        else
                        {
                            return;
                        }
                    }
                }


            }

            internal List<int> GenerateDirectorySizeList()
            {
                List<int> result = new List<int>();
                foreach (ElFSNode node in contents)
                {
                    if (node is ElFSDirectory)
                    {
                        ElFSDirectory dir = (ElFSDirectory)node;
                        result.AddRange(dir.GenerateDirectorySizeList());
                    }
                }
                result.Add(Size());
                return result;
            }
        }

        internal class ElFSFile : ElFSNode
        {
            string fileName;
            string extension;
            int size;

            public ElFSFile(string name, string sizeString)
            {
                if (name.Contains("."))
                {
                    string[] nameParts = name.Split(new char[] { '.' });
                    fileName = nameParts[0];
                    extension = nameParts[1];
                }
                else
                {
                    fileName = name;
                }

                size = Int32.Parse(sizeString);

            }
            public override string Name()
            {
                if (extension != null)
                {
                    return fileName + "." + extension;
                }
                else
                {
                    return fileName;
                }
            }
            override public int Size()
            {
                return size;
            }


        }
    }
}
