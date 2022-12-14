using System.Text;
namespace AdventOfCode2022
{
    class Day13 : Problem
    {
        public Day13(string inputPath) : base(inputPath) {}

        // Returns true if in order, false if out of order and null if no clear result
        protected int Compare(string[] left, string[] right) 
        {
            for (int index = 0; index < left.Length; index ++) {
                if (index >= right.Length) {
                    return -1;
                } else if (left[index].StartsWith('[') || right[index].StartsWith('[')) {
                    string[] leftList = left[index].StartsWith('[') ? 
                        GetSymbolArray(left[index].Substring(1, left[index].Length - 2)) : 
                        new string[] {left[index]};
                    string[] rightList = right[index].StartsWith('[') ? 
                        GetSymbolArray(right[index].Substring(1, right[index].Length - 2)) : 
                        new string[] {right[index]};
                    int result = Compare(leftList, rightList);
                    if (result != 0) return result;
                } else {
                    int leftInt = int.Parse(left[index]);
                    int rightInt = int.Parse(right[index]);

                    if (leftInt < rightInt) return 1;
                    else if (leftInt > rightInt) return -1;
                }
            }
            if (left.Length < right.Length) return 1;
            return 0;
        }

        protected string[] GetSymbolArray(string input) 
        {
            if (input == "") {
                return new string[0];
            } else if (!input.Contains("[")) {
                return input.Split(',');
            }

            List<string> result = new List<string>();
            for (int i = 0; i < input.Length; i++) {
                string symbol = GetNextSymbol(input, ref i);
                result.Add(symbol);
            }
            return result.ToArray();
        }

        protected string GetNextSymbol(string input, ref int index) 
        {
            int bracketCount = 0;
            StringBuilder symbol = new();
            for (; index < input.Length; index ++) {
                if (input[index] == ',' && bracketCount == 0) {
                    return symbol.ToString();
                } else if (input[index] == '[') {
                    bracketCount++;
                } else if (input[index] == ']') {
                    bracketCount--;
                }
                symbol.Append(input[index]);
            }
            return symbol.ToString();
        }

        public override string Part1()
        {
            string[][] pairs = rawPuzzleInput
                .Split("\n\n")
                .Select(x => x.Split("\n").ToArray())
                .ToArray();

            int sum = 0;
            foreach (var (pair, i) in pairs.Select((value, i) => ( value, i ))) {
                int ordered = Compare(
                    GetSymbolArray(pair[0].Substring(1, pair[0].Length - 2)), 
                    GetSymbolArray(pair[1].Substring(1, pair[1].Length - 2))
                );
                if (ordered == 1) {
                    sum += (i + 1);
                }
            }

            return sum.ToString();
        }

        public override string Part2()
        {
            List<string> allPackets = puzzleInputLines.Where(x => x.Trim() != "").Select(x => x.Substring(1, x.Length - 2)).ToList();
            allPackets.Add("[2]");
            allPackets.Add("[6]");
            allPackets.Sort(delegate(string left, string right) {return Compare(GetSymbolArray(right), GetSymbolArray(left));});
            return ((allPackets.IndexOf("[2]") + 1) * (allPackets.IndexOf("[6]") + 1)).ToString();
        }
    }
}