using System.Xml.Schema;
using System.Reflection.PortableExecutable;
namespace AdventOfCode2022
{
    class Day8 : Problem
    {
        int[][] trees;

        public Day8(string inputPath) : base(inputPath)
        {
            trees = new int[puzzleInputLines.Length][];
            for (int y = 0; y < puzzleInputLines.Length; y++) {
                for (int x = 0; x < puzzleInputLines[y].Length; x++) {
                    trees[x] = trees[x] == null ? new int[puzzleInputLines.Length] : trees[x];
                    trees[x][y] = puzzleInputLines[y][x] - '0';
                }
            }
        }

        public override string Part1()
        {
            Dictionary<(int, int), bool> visibleTrees = new();
            for (int x = 0; x < trees.Length; x ++) {
                int curr = -1;
                for (int y = 0; y < trees[x].Length; y++) { // Columns, top to bottom
                    if (trees[x][y] > curr) {
                        visibleTrees[(x, y)] = true;
                        curr = trees[x][y];
                    }
                }
                curr = -1;
                for (int y = trees[x].Length - 1; y >= 0; y--) { // Columns, bottom to top
                    if (trees[x][y] > curr) {
                        visibleTrees[(x, y)] = true;
                        curr = trees[x][y];
                    }
                }
            }

            for (int y = 0; y < trees[0].Length; y++ ) {
                int curr = -1;
                for (int x = 0; x < trees.Length; x++) { // Rows, left to right
                    if (trees[x][y] > curr) {
                        visibleTrees[(x, y)] = true;
                        curr = trees[x][y];
                    }
                }
                curr = -1;
                for (int x = trees.Length - 1; x >= 0; x--) { // Rows, right to left
                    if (trees[x][y] > curr) {
                        visibleTrees[(x, y)] = true;
                        curr = trees[x][y];
                    }
                }
            }

            return visibleTrees.Values.Count.ToString();
        }

        public override string Part2()
        {
            int maxScenicScore = 0;
            for (int x = 0; x < trees.Length; x++) {
                for (int y = 0; y < trees[x].Length; y++) {
                    int score = getScenicScore(x, y);
                    if (score >= maxScenicScore) {
                        maxScenicScore = score;
                    }
                }
            }
            return maxScenicScore.ToString();
        }

        protected int getScenicScore(int posX, int posY)
        {
            int upScore = 0;
            int downScore = 0;
            int leftScore = 0;
            int rightScore = 0;
            int treeHeight = trees[posX][posY];

            for(int x = posX - 1; x >= 0; x--) {
                leftScore ++;
                if (trees[x][posY] >= treeHeight) {
                    break;
                } 
            }

            for(int x = posX + 1; x < trees.Length; x++) {
                rightScore ++;
                if (trees[x][posY] >= treeHeight) {
                    break;
                } 
            }

            for(int y = posY - 1; y >= 0; y--) {
                upScore ++;
                if (trees[posX][y] >= treeHeight) {
                    break;
                } 
            }

            for(int y = posY + 1; y < trees.Length; y++) {
                downScore ++;
                if (trees[posX][y] >= treeHeight) {
                    break;
                } 
            }

            return upScore * downScore * leftScore * rightScore;
        }
    }
}