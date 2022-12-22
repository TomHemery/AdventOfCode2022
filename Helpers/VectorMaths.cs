namespace AdventOfCode2022.Helpers
{
    public static class VectorMaths
    {
        public static (int x, int y) Add((int x, int y) a, (int x, int y) b)
        {
            return (a.x + b.x, a.y + b.y);
        }

        public static (int x, int y, int z) Add((int x, int y, int z) a, (int x, int y, int z) b)
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

        public static (int x, int y) Mult((int x, int y) a, int operand)
        {
            return (a.x * operand, a.y * operand);
        }

        public static (int x, int y) RotateClockwise90((int x, int y) a)
        {
            return (a.y, a.x * -1);
        }

        public static (int x, int y) RotateAntiClockwise90((int x, int y) a) 
        {
            return (a.y * -1, a.x);
        }
    }
}