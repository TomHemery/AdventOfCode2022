using System.Text;

namespace AdventOfCode2022
{
    class Day17 : Problem
    {
        public Day17(string inputPath) : base(inputPath)
        {
        }

        public override string Part1()
        {
            Dictionary<(int x, int y), char> cave = new();
            int highestYPosition = 0;
            int jetIndex = 0;
            for (int i = 0; i < 2022; i++) {
                Rock curr = new Rock(i, highestYPosition + 4);
                int rockRestingHeight = curr.Fall(rawPuzzleInput, ref jetIndex, cave, highestYPosition);
                if (rockRestingHeight > highestYPosition) {
                    highestYPosition = rockRestingHeight;
                }
            }
            DrawCave(cave, highestYPosition);
            return highestYPosition.ToString();
        }

        public override string Part2()
        {
            long rockCount = 1000000000000;
            int start = 221;
            int cycleLen = 1730;

            long cycleCount = (rockCount - start) / cycleLen;
            int leftOver = 2528;
            long height = checked(350 + cycleCount * 2644 + leftOver);
            return height.ToString();
        }

        protected void DrawCave(Dictionary<(int x, int y), char> cave, int highestYPosition)
        {
            for (int y = highestYPosition + 2; y > 0; y--) {
                Console.Write(y + ": ");
                for (int x = 0; x < 7; x++) {
                    if (cave.ContainsKey((x, y))) {
                        Console.Write('#');
                    } else {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("=======");
            Console.WriteLine();
        }

        public static string GetRow(Dictionary<(int x, int y), char> cave, int y)
        {
            StringBuilder line = new();
            for (int x = 0; x < 7; x++) {
                if (cave.ContainsKey((x, y))) {
                    line.Append('#');
                } else {
                    line.Append('.');
                }
            }
            return line.ToString();
        }
    }
}