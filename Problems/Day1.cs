using System.Linq;

namespace AdventOfCode2022
{
    public class Day1 : Problem
    {
        protected IEnumerable<int> calories;

        public Day1(string inputPath) : base(inputPath)
        {
            calories = rawPuzzleInput
                .Split("\n\n")
                .Select(x => x.Split("\n").Sum(x => {return Convert.ToInt32(x);}))
                .OrderByDescending(x => x).ToList();
        }

        public override string Part1()
        {
            return "Max: " + calories.First();
        }

        public override string Part2()
        {
            return "Max 3: " + (calories.Take(3).Sum());
        }
    }
}