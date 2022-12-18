namespace AdventOfCode2022.Helpers
{
    public static class VectorMaths
    {
        public static (int x, int y) Sum((int x, int y) a, (int x, int y) b)
        {
            return (a.x + b.x, a.y + b.y);
        }

        public static (int x, int y, int z) Sum((int x, int y, int z) a, (int x, int y, int z) b)
        {
            return (a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static int ManhattanDist((int x, int y) a, (int x, int y) b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        public static int ManhattanDist((int x, int y, int z) a, (int x, int y, int z) b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);
        }
    }
}