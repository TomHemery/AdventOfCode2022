using System.Runtime.CompilerServices;

namespace AdventOfCode2022
{
    class Day9 : Problem
    {
        public Day9(string inputPath) : base(inputPath)
        {
        }

        public void MoveHead(ref (int x, int y) head, char dir) 
        {
            switch (dir) {
                case 'D':
                    head.y -= 1;
                    break;
                case 'U':
                    head.y += 1;
                    break;
                case 'L':
                    head.x -= 1;
                    break;
                case 'R':
                    head.x += 1;
                    break;
            }
        }

        public void MoveTail((int x, int y) headPos, ref (int x, int y) tailPos)
        {
            if (Math.Abs(headPos.x - tailPos.x) == 2) {
                tailPos.x += Math.Sign(headPos.x - tailPos.x);
                if (tailPos.y != headPos.y) {
                    tailPos.y += Math.Sign(headPos.y - tailPos.y);
                }
            } else if (Math.Abs(headPos.y - tailPos.y) == 2) {
                tailPos.y += Math.Sign(headPos.y - tailPos.y);
                if (tailPos.x != headPos.x) {
                    tailPos.x += Math.Sign(headPos.x - tailPos.x);
                }
            } 
        }

        public override string Part1()
        {
            Dictionary<(int, int), bool> visitedPositions = new();
            (int x, int y) headPos = (0, 0);
            (int x, int y) tailPos = (0, 0);

            visitedPositions[tailPos] = true;

            foreach(string line in puzzleInputLines) {
                char dir = line.Split(' ')[0][0];
                int steps = int.Parse(line.Split(' ')[1]);

                for (; steps > 0; steps --) {
                    MoveHead(ref headPos, dir);
                    MoveTail(headPos, ref tailPos);
                    visitedPositions[tailPos] = true;
                }
            }

            return visitedPositions.Values.Count().ToString();
        }

        public override string Part2()
        {
            Dictionary<(int, int), bool> visitedPositions = new();

            (int x, int y)[] positions = new (int x, int y)[10];
            for (int i = 0; i < positions.Length; i++) positions[i] = (0, 0);

            visitedPositions[positions[9]] = true;

            foreach(string line in puzzleInputLines) {
                char dir = line.Split(' ')[0][0];
                int steps = int.Parse(line.Split(' ')[1]);

                for (; steps > 0; steps --) {
                    MoveHead(ref positions[0], dir);
                    for (int i = 1; i < positions.Length; i++) {
                        MoveTail(positions[i - 1], ref positions[i]);
                    }
                    visitedPositions[positions[9]] = true;
                }
            }

            return visitedPositions.Values.Count().ToString();
        }
    }
}