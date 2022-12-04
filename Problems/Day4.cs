namespace AdventOfCode2022
{
    public class Day4 : Problem
    {
        public Day4(string inputPath) : base(inputPath)
        {
        }

        public override string Part1()
        {
            int fullyContained = 0;
            foreach (string line in puzzleInputLines) {
                string[] parts = line.Split(",");
                IEnumerable<int> sectionA = parts[0].Split("-").Select(x => int.Parse(x));
                IEnumerable<int> sectionB = parts[1].Split("-").Select(x => int.Parse(x));

                if (sectionA.First() >= sectionB.First() && sectionA.Last() <= sectionB.Last()) {
                    fullyContained++;
                } else if(sectionB.First() >= sectionA.First() && sectionB.Last() <= sectionA.Last()) {
                    fullyContained++;
                }
            }
            return fullyContained.ToString();
        }

        public override string Part2()
        {
            int overlap = 0;
            foreach (string line in puzzleInputLines) {
                string[] parts = line.Split(",");
                IEnumerable<int> sectionA = parts[0].Split("-").Select(x => int.Parse(x));
                IEnumerable<int> sectionB = parts[1].Split("-").Select(x => int.Parse(x));

                if (
                    sectionA.Last() >= sectionB.First() && sectionA.Last() <= sectionB.Last() || 
                    sectionB.Last() >= sectionA.First() && sectionB.Last() <= sectionA.Last()
                ) {
                    overlap++;
                }
            }
            return overlap.ToString();
        }
    }
}