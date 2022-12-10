using System.Text;
namespace AdventOfCode2022
{
    class Day10 : Problem
    {

        public Day10(string inputPath) : base(inputPath)
        {
        }

        protected int calculateSignalStrength(int counter, int x) 
        {
            if ((counter - 20) % 40 == 0) {
                return counter * x;
            }
            return 0;
        }

        public override string Part1()
        {
            int signalStrength = 0;
            int register = 1;
            int cycleCounter = 1;
            foreach (string line in puzzleInputLines) {
                string instruction = line.Split(" ")[0];
                if (instruction == "addx") {
                    int value = int.Parse(line.Split(" ")[1]);
                    signalStrength += calculateSignalStrength(++cycleCounter, register);
                    register += value;
                }
                signalStrength += calculateSignalStrength(++cycleCounter, register);
            }
            return signalStrength.ToString();
        }

        protected void DrawNextPixel(int cycleCounter, int register, StringBuilder sb)
        {
            int row = (cycleCounter - 1) / 40;
            int col = (cycleCounter - 1) % 40;

            if (col == 0) {
                sb.Append("\n");
            }

            if (col == register - 1 || col == register || col == register + 1) {
                sb.Append("#");
            } else {
                sb.Append(" ");
            }
        }

        public override string Part2()
        {
            int register = 1;
            int cycleCounter = 0;
            StringBuilder crtStringBuilder = new();
            foreach (string line in puzzleInputLines) {
                DrawNextPixel(++cycleCounter, register, crtStringBuilder);
                string instruction = line.Split(" ")[0];
                if (instruction == "addx") {
                    int value = int.Parse(line.Split(" ")[1]);
                    DrawNextPixel(++cycleCounter, register, crtStringBuilder);
                    register += value;
                }
            }
            return crtStringBuilder.ToString();
        }
    }
}