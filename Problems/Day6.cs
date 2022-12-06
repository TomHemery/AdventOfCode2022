namespace AdventOfCode2022
{
    class Day6 : Problem
    {
        public Day6(string inputPath) : base(inputPath)
        {
        }

        protected int findDistinctSubstring(int length)
        {
           for (int i = length - 1; i <= rawPuzzleInput.Length; i++) {
                string checkString = rawPuzzleInput.Substring(i - (length - 1), length);
                if (checkString.Distinct().Count() == length) {
                    return i + 1;
                }
            } 
            throw new Exception(string.Format("Couldn't find substring of length {0}", length));
        }

        public override string Part1()
        {
            return findDistinctSubstring(4).ToString();
        }

        public override string Part2()
        {
            return findDistinctSubstring(14).ToString();
        }
    }
}