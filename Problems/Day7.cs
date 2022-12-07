namespace AdventOfCode2022
{
    class Day7 : Problem
    {

        protected List<int> dirSizes = new();


        public Day7(string inputPath) : base(inputPath)
        {
        }


        protected int calculateDirectorySize(ref int curr)
        {
            int dirSize = 0;
            do {
                if (!puzzleInputLines[curr].StartsWith("dir") && !puzzleInputLines[curr].StartsWith("$")) {
                    dirSize += int.Parse(puzzleInputLines[curr].Split(" ")[0]);
                    curr ++; // To next line
                } else if (puzzleInputLines[curr].StartsWith("$ cd") && !puzzleInputLines[curr].Contains("..")) {
                    curr ++; // Step over cd into dir
                    dirSize += calculateDirectorySize(ref curr);
                } else {
                    curr ++; // To next line
                }
            } while (curr < puzzleInputLines.Length && !puzzleInputLines[curr].StartsWith("$ cd .."));
            curr ++; // Step over cd ..
            dirSizes.Add(dirSize);
            return dirSize;
        }

        public override string Part1()
        {
            dirSizes.Clear();
            int start = 1; // skip the cd /
            calculateDirectorySize(ref start);
            int totalSize = 0;
            foreach (int dirSize in dirSizes.Where(x => x <= 100000)) {
                totalSize += dirSize;
            }
            return totalSize.ToString();
        }

        public override string Part2()
        {
            dirSizes.Clear();
            int start = 1; // skip the cd /
            int fileSystemSize = calculateDirectorySize(ref start);

            int spaceToDelete = 30000000 - (70000000 - fileSystemSize);
            var largeDirs = dirSizes.Where(x => x >= spaceToDelete).ToList();
            largeDirs.Sort();
            
            return largeDirs[0].ToString();
        }
    }
}