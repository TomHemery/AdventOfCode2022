namespace AdventOfCode2022.Helpers
{
    public static class VectorMaths
    {
        public static (int x, int y) Sum((int x, int y) a, (int x, int y) b)
        {
            return (a.x + b.x, a.y + b.y);
        }
    }
}