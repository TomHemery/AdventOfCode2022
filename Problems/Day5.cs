using System.Text;
namespace AdventOfCode2022
{
    public class Day5 : Problem
    {
        protected IEnumerable<string[]> moves;
        protected string[] parts;

        public Day5(string inputPath) : base(inputPath)
        {
            parts = rawPuzzleInput.Split("\n\n");
            moves = parts[1].Split("\n").Select(x => x.Split(' '));
        }

        public override string Part1()
        {
            Dictionary<int, Stack<char>> stacks = ParseStacks(parts[0]);
            
            foreach (var move in moves) {
                int count = int.Parse(move[1]);
                int sourceIndex = int.Parse(move[3]);
                int destIndex = int.Parse(move[5]);

                for (; count > 0; count --) {
                    char c = stacks[sourceIndex].Pop();
                    stacks[destIndex].Push(c);
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= stacks.Count; i++) {
                sb.Append(stacks[i].Peek());
            }
            return sb.ToString();
        }

        public override string Part2()
        {
            Dictionary<int, Stack<char>> stacks = ParseStacks(parts[0]);
            Stack<char> cratesToMove = new();

            foreach (var move in moves) {
                int count = int.Parse(move[1]);
                int sourceIndex = int.Parse(move[3]);
                int destIndex = int.Parse(move[5]);

                for (; count > 0; count --) {
                    char c = stacks[sourceIndex].Pop();
                    cratesToMove.Push(c);
                }
                while (cratesToMove.Count > 0) {
                    stacks[destIndex].Push(cratesToMove.Pop());
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= stacks.Count; i++) {
                sb.Append(stacks[i].Peek());
            }
            return sb.ToString();
        }

        protected Dictionary<int, Stack<char>> ParseStacks(string stackDescription)
        {
            Dictionary<int, Stack<char>> stacks = new();
            IEnumerable<string> rows = stackDescription
                .Split("\n")
                .SkipLast(1);
            foreach (string row in rows) {
                for (int i = 0; i < row.Length; i++) {
                    if (row[i] != '[' && row[i] != ']' && row[i] != ' ') {
                        int stackIndex = i / 4 + 1;
                        
                        if (!stacks.ContainsKey(stackIndex)) {
                            stacks[stackIndex] = new();
                        }
                        
                        stacks[stackIndex].Push(row[i]);
                    }
                }
            }

            foreach (KeyValuePair<int, Stack<char>> kvp in stacks) {
                stacks[kvp.Key] = new Stack<char>(kvp.Value);
            }

            return stacks;
        }
    }
}