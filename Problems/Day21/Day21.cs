namespace AdventOfCode2022
{
    class Day21 : Problem
    {
        MathMonkey? rootMonkey;
        public Day21(string inputPath) : base(inputPath)
        {
            foreach (string line in puzzleInputLines) {
                MathMonkey newMonkey;
                string[] parts = line.Split(": ");
                string name = parts[0];
                string[] right = parts[1].Split(" ");

                if (right.Length == 1) {
                    newMonkey = new(name, int.Parse(right[0]));
                } else {
                    newMonkey = new(name, right[0], right[2], right[1][0]);
                }

                if (name == "root") {
                    rootMonkey = newMonkey;
                }
            }
        }

        public override string Part1()
        {
            if (rootMonkey == null) {
                throw new Exception("No Root Monkey");
            }
            return rootMonkey.GetValue().ToString();
        }

        public override string Part2()
        {
            if (rootMonkey == null) {
                throw new Exception("No Root Monkey");
            }

            bool humanLeft = rootMonkey.HasLeftDescendent("humn");
            Decimal min = -long.MaxValue;
            Decimal max = long.MaxValue;
            MathMonkey human = MathMonkey.monkeyLookup["humn"];

            human.value = 1;
            Decimal first = humanLeft ? rootMonkey.GetLeftValue() : rootMonkey.GetRightValue();
            human.value = 100;
            Decimal second = humanLeft ? rootMonkey.GetLeftValue() : rootMonkey.GetRightValue();
            bool increaseToIncrease = second > first;

            human.value = min + (max - min) / 2;
            Decimal left = rootMonkey.GetLeftValue();
            Decimal right = rootMonkey.GetRightValue();
            while (left != right) {
                Decimal humanSide = humanLeft ? left : right;
                Decimal other = humanLeft ? right : left;
                if (humanSide > other) { // too big
                    if (increaseToIncrease) {
                        max = min + (max - min) / 2;
                    } else {
                        min = min + (max - min) / 2;
                    }
                } else { // too small
                    if (increaseToIncrease) {
                        min = min + (max - min) / 2;
                    } else {
                        max = min + (max - min) / 2;
                    }
                }
                human.value = min + (max - min) / 2;
                left = rootMonkey.GetLeftValue();
                right = rootMonkey.GetRightValue();
            }
            
            return Math.Round(human.value).ToString();
        }


    }
}