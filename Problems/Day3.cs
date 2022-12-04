namespace AdventOfCode2022
{
    public class Day3 : Problem
    {
        public Day3(string inputPath) : base(inputPath)
        {
        }

        public override string Part1()
        {
            int prioritySum = 0;
            foreach (string line in puzzleInputLines) {
                string rucksackA = line.Substring(0, line.Length/2);
                string rucksackB = line.Substring((int)(line.Length / 2), (int)(line.Length / 2));
                foreach (char item in rucksackB) {
                    if (rucksackA.Contains(item)) {
                        int priority = item - 96;
                        if (priority < 0) priority += 58;
                        prioritySum += priority;
                        break;
                    }
                }
            }
            return prioritySum.ToString();
        }

        public override string Part2()
        {
            int prioritySum = 0;
            for (int i = 0; i < puzzleInputLines.Length; i += 3) {
                foreach (char item in puzzleInputLines[i]) {
                    if (puzzleInputLines[i + 1].Contains(item) && puzzleInputLines[i + 2].Contains(item)) {
                        int priority = item - 96;
                        if (priority < 0) priority += 58;
                        prioritySum += priority;
                        break;
                    }
                }   
            }
            return prioritySum.ToString();
        }
    }
}