namespace AdventOfCode2022
{
    class Day20 : Problem
    {
        public Day20(string inputPath) : base(inputPath)
        {
        }

        public override string Part1()
        {
            List<(long val, int salt)> numbers = puzzleInputLines.Select((x, i) => (long.Parse(x), i)).ToList();
            numbers = Mix(numbers);
            int zeroIndex = numbers.IndexOf(numbers.Find(x => x.val == 0));
            return (
                GetAtCircular(numbers, 1000 + zeroIndex) + 
                GetAtCircular(numbers, 2000 + zeroIndex) + 
                GetAtCircular(numbers, 3000 + zeroIndex)
            ).ToString();
        }

        public override string Part2()
        {
            List<(long val, int salt)> numbers = puzzleInputLines.Select((x, i) => (long.Parse(x) * 811589153, i)).ToList();
            List<(long val, int salt)> mixedNumbers = new(numbers);
            for (int i = 0; i < 10; i++) {
                mixedNumbers = Mix(numbers, mixedNumbers);
            }
            int zeroIndex = mixedNumbers.IndexOf(mixedNumbers.Find(x => x.val == 0));
            return (
                GetAtCircular(mixedNumbers, 1000 + zeroIndex) + 
                GetAtCircular(mixedNumbers, 2000 + zeroIndex) + 
                GetAtCircular(mixedNumbers, 3000 + zeroIndex)
            ).ToString();
        }

        protected List<(long val, int salt)> Mix(List<(long val, int i)> startNumbers, List<(long val, int i)>? result = null)
        {
            result = result == null ? new(startNumbers) : result;
            foreach (var number in startNumbers) {
                int oldIndex = result.IndexOf(number);
                if (oldIndex == -1) {
                    Console.WriteLine("Couldn't find: " + number);
                }
                int newIndex = (int)((oldIndex + number.val) % (result.Count - 1));
                if (newIndex <= 0 && oldIndex + number.val != 0) newIndex = result.Count - 1 + newIndex;
                result.RemoveAt(oldIndex);
                result.Insert(newIndex, number);
            }
            return result;
        }

        protected long GetAtCircular(List<(long val, int salt)> list, int index)
        {
            return list[index % list.Count].val;
        }
    }
}