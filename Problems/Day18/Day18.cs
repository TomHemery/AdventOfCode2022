using AdventOfCode2022.Helpers;

namespace AdventOfCode2022
{
    public class Day18 : Problem
    {
        Dictionary<(int x, int y, int z), Cube> allCubes = new();
        int minDim = -1;
        int maxDim = 22;

        (int x, int y, int z)[] offsetPoints = {
            (-1, 0, 0),
            (1, 0, 0),
            (0, -1, 0),
            (0, 1, 0),
            (0, 0, -1),
            (0, 0, 1)
        };

        public Day18(string inputPath) : base(inputPath)
        {
            foreach (string line in puzzleInputLines) {
                int[] coords = line.Split(",").Select(x => int.Parse(x)).ToArray();
                allCubes[(coords[0], coords[1], coords[2])] = new Cube(coords[0], coords[1], coords[2]);
            }

            foreach (Cube cube in allCubes.Values) {
                cube.FindNeighboursIn(allCubes);
            }
        }

        public override string Part1()
        {
            return GetExposedFaces().ToString();
        }

        public override string Part2()
        {
            int interiorFaces = 0;
            var reachable = GetReachableCubes((-1, -1, -1));
            for (int x = minDim; x <= maxDim; x++) {
                for (int y = minDim; y <= maxDim; y++) {
                    for (int z = minDim; z <= maxDim; z++) {
                        if (!allCubes.ContainsKey((x, y, z)) && !reachable.ContainsKey((x, y, z))) {
                            Cube airCube = new Cube(x, y, z);
                            airCube.FindNeighboursIn(allCubes);
                            interiorFaces += airCube.neighbours.Count();
                        }
                    }
                }
            }

            return (GetExposedFaces() - interiorFaces).ToString();
        }

        protected int GetExposedFaces()
        {
            return allCubes.Values.Sum(x => 6 - x.neighbours.Count);
        }

        protected Dictionary<(int x, int y, int z), bool> GetReachableCubes((int x, int y, int z) start)
        {
            Dictionary<(int x, int y, int z), bool> reachableSpaces = new();
            Queue<(int x, int y, int z)> openSet = new();
            openSet.Enqueue(start);

            Dictionary<(int x, int y, int z), bool> visited = new();
            visited[start] = true;

            while (openSet.Count > 0) {
                (int x, int y, int z) curr = openSet.Dequeue();

                foreach((int x, int y, int z) offset in offsetPoints) {
                    (int x, int y, int z) neighbour = VectorMaths.Add(curr, offset);
                    if (allCubes.ContainsKey(neighbour)) { // cube here
                        continue;
                    }
                    if (neighbour.x < minDim || neighbour.y < minDim || neighbour.z < minDim) { // out of bounds
                        continue;
                    } else if (neighbour.x > maxDim || neighbour.y > maxDim || neighbour.z > maxDim) { // out of bounds
                        continue;
                    }
                    if (!reachableSpaces.ContainsKey(neighbour)) {
                        reachableSpaces[neighbour] = true;
                        if(!openSet.Contains(neighbour)) openSet.Enqueue(neighbour);
                    }
                }
            }

            return reachableSpaces;
        }
    }
}