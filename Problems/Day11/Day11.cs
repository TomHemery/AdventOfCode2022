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
                    monkey.TakeTurn((x => x / 3));
                }
            }

            IEnumerable<int> inspectionCounts = Monkey.allMonkeys.Select(x => x.timesInspected).OrderByDescending(x => x);
            return (inspectionCounts.First() * inspectionCounts.Skip(1).First()).ToString();
        }

        public override string Part2()
        {
            resetMonkeys();
            int lcm = Monkey.allMonkeys.Select(m => m.divisor).Aggregate((a, b) => a * b);

            for (int i = 0; i < 10000; i++) {
                foreach (Monkey monkey in Monkey.allMonkeys) {
                    monkey.TakeTurn((x => x % lcm));
                }
            }

            IEnumerable<int> inspectionCounts = Monkey.allMonkeys.Select(x => x.timesInspected).OrderByDescending(x => x);
            return (((long)inspectionCounts.First()) * ((long)inspectionCounts.Skip(1).First())).ToString();
        }
    }
}