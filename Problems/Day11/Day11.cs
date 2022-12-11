using System.Text;
namespace AdventOfCode2022
{
    class Day11 : Problem
    {
        string[] monkeyDescriptions;

        public Day11(string inputPath) : base(inputPath)
        {
            monkeyDescriptions = rawPuzzleInput.Split("\n\n");
        }

        protected void resetMonkeys()
        {
            Monkey.ClearMonkeyList();
            
            foreach (string description in monkeyDescriptions) {
                string[] descriptionParts = description.Split("\n").Select(x => x.Trim()).Skip(1).ToArray();

                new Monkey(
                    descriptionParts[0],
                    descriptionParts[1],
                    descriptionParts[2],
                    descriptionParts[3],
                    descriptionParts[4]
                );
            }
        }

        public override string Part1()
        {
            resetMonkeys();
            for (int i = 0; i < 20; i++) {
                foreach (Monkey monkey in Monkey.allMonkeys) {
                    monkey.TakeTurn();
                }
            }

            IEnumerable<int> inspectionCounts = Monkey.allMonkeys.Select(x => x.timesInspected).OrderByDescending(x => x);
            return (inspectionCounts.First() * inspectionCounts.Skip(1).First()).ToString();
        }

        public override string Part2()
        {
            throw new NotImplementedException();
        }
    }
}