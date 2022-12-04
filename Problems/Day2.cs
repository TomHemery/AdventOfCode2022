namespace AdventOfCode2022
{
    public class Day2 : Problem
    {
        public Day2(string inputPath) : base(inputPath)
        {
        }

        public override string Part1()
        {
            long score = 0;
            foreach(string line in puzzleInputLines) {
                int playerMove = line[2] - 87;
                int opponentMove = line[0] - 64;
                score += (playerMove);
                if (playerMove == opponentMove) { // DRAW
                    score += 3;
                } else if (((playerMove + 4) % 3) + 1 == opponentMove) { // WIN 
                    score += 6;
                }
            }
            return score.ToString();
        }

        public override string Part2()
        {
            long score = 0;
            foreach(string line in puzzleInputLines) {
                int playerGoal = line[2] - 89;
                int opponentMove = line[0] - 64;
                int playerMove = opponentMove + playerGoal;
                playerMove = playerMove == 0 ? 3 : playerMove == 4 ? 1 : playerMove;
                score += (playerMove);
                score += (playerGoal + 1) * 3;
            }
            return score.ToString();
        }
    }
}