using System.Diagnostics;

namespace AdventOfCode2022
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();
            Problem p = new Day22("PuzzleInputs/day22.txt");
            watch.Stop();
            
            Console.WriteLine("Constructed puzzle in: " + (watch.ElapsedTicks / 10) + "μs");
            Console.WriteLine();
            
            watch.Restart();
            string result = p.Part1();
            watch.Stop();

            Console.WriteLine("Part 1 (Executed in "+ (watch.ElapsedTicks / 10) + "μs):");
            Console.WriteLine(result);
            Console.WriteLine();

            watch.Restart();
            result = p.Part2();
            watch.Stop();

            Console.WriteLine("Part 2 (Executed in "+ (watch.ElapsedTicks / 10) + "μs):");
            Console.WriteLine(result);
            Console.WriteLine();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}