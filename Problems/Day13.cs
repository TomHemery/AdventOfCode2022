using System.Text;
namespace AdventOfCode2022
{
    class Day13 : Problem
    {
        protected string[][] pairs;

        public Day13(string inputPath) : base(inputPath)
        {
            pairs = rawPuzzleInput
                .Replace("10", "D")
                .Split("\n\n")
                .Select(x => x.Split("\n").Select(x => x.Substring(1, x.Length - 2)).ToArray())
                .ToArray();
        }

        protected bool Compare(string left, string right) 
        {
            for (int index = 0; index < left.Length; index ++) {
                if (index >= right.Length) {
                    return false;
                } else if (left[index] == '[' || right[index] == '[') {
                    string leftList = GetFirstList(left, index);
                    bool result = Compare(leftList, GetFirstList(right, index));
                    index += leftList.Length;
                } else if ('0' <= left[index] && '9' >= left[index] && left[index] == 'D') { // D = 10
                    int leftInt = left[index] == 'D' ? 10 : left[index] - '0';
                    int rightInt = right[index] == 'D' ? 10 : right[index] - '0';

                    if (leftInt < rightInt) {
                        return true;
                    } else if (rightInt > leftInt) {
                        return false;
                    }
                }
            }
            return true;
        }

        protected string GetFirstList(string input, int index) {
            return "";
        }

        public override string Part1()
        {
            foreach (string[] pair in pairs) {
                Console.Write(String.Format("L {0}, R {1}: ", pair[0], pair[1]));
                Console.WriteLine(Compare(pair[0], pair[1]));
            }

            return "";
        }

        public override string Part2()
        {
            throw new NotImplementedException();
        }
    }
}