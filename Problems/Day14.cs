using AdventOfCode2022.Helpers;

namespace AdventOfCode2022
{
    class Day14 : Problem
    {

        Dictionary<(int x, int y), char> caveMap = new();
        protected int minX = int.MaxValue;
        protected int maxX = int.MinValue;
        protected int maxY = int.MinValue;

        protected int floorHeight = 0;

        public Day14(string inputPath) : base(inputPath)
        {
        }

        public override string Part1()
        {
            ResetCaveMap();
            int sandParticlesAtRest = 0;
            while (SpawnSandFrom((500, 0), false)){
                sandParticlesAtRest++;
            }
            return sandParticlesAtRest.ToString();
        }

        public override string Part2()
        {
            ResetCaveMap();
            int sandParticlesAtRest = 0;
            while (!caveMap.ContainsKey((500, 0))){
                SpawnSandFrom((500, 0), true);
                sandParticlesAtRest++;
            }
            return sandParticlesAtRest.ToString();
        }

        protected bool SpawnSandFrom((int x, int y) startPoint, bool useFloor)
        {
            (int x, int y) sandPosition = startPoint;
            while (useFloor ? sandPosition.y < floorHeight - 1: sandPosition.y <= maxY) {
                (int x, int y) next = VectorMaths.Sum(sandPosition, (0, 1));
                if (!caveMap.ContainsKey(next)) {
                    sandPosition = next;
                    continue;
                } 
                next.x -= 1;
                if (!caveMap.ContainsKey(next)) {
                    sandPosition = next;
                    continue;
                } 
                next.x += 2;
                if (!caveMap.ContainsKey(next)) {
                    sandPosition = next;
                    continue;
                }

                SettleSandAt(sandPosition);
                return true;
            }
            if (useFloor) {
                SettleSandAt(sandPosition);
                return true;
            }
            return false;
        }

        protected void SettleSandAt((int x, int y) pos)
        {
            if (pos.x < minX) {
                minX = pos.x; 
            } else if (pos.x > maxX) {
                maxX = pos.x;
            }
            caveMap[pos] = 'o';
        }

        protected void PrintCaveMap(bool useFloor)
        {
            for (int y = 0; y <= (useFloor ? floorHeight : maxY); y++) {
                for (int x = minX; x <= maxX; x++) {
                    if (y == floorHeight) {
                        Console.Write('#');
                    } else if(caveMap.ContainsKey((x, y))) {
                        Console.Write(caveMap[(x, y)]);
                    } else {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
        }

        protected void ResetCaveMap()
        {
            caveMap.Clear();

            foreach(string line in puzzleInputLines) {
                (int x, int y)[] points = line.Split(" -> ").Select(x => {
                    var parts = x.Split(","); return (int.Parse(parts[0]), int.Parse(parts[1]));
                }).ToArray();

                for (int i = 0; i < points.Length - 1; i++) {
                    CreateRockLine(points[i], points[i + 1]);
                }

                int lineMinX = points.Select(point => point.x).Min();
                int lineMaxX = points.Select(point => point.x).Max();
                int lineMaxY = points.Select(point => point.y).Max();

                minX = minX < lineMinX ? minX : lineMinX;
                maxX = maxX > lineMaxX ? maxX : lineMaxX;
                maxY = maxY > lineMaxY ? maxY : lineMaxY;
                floorHeight = maxY + 2;
            }
        }

        protected void CreateRockLine((int x, int y) start, (int x, int y) end) 
        {
            (int x, int y) step = start.x != end.x ? 
                (Math.Sign(end.x - start.x), 0):
                (0, Math.Sign(end.y - start.y));
            caveMap[start] = '#';
            while (start != end) {
                start = VectorMaths.Sum(start, step);
                caveMap[start] = '#';
            }
        }
    }
}