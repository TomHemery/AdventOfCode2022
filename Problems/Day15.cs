using System.Text.RegularExpressions;
using AdventOfCode2022.Helpers;

namespace AdventOfCode2022 
{
    class Day15 : Problem
    {
        ((int x, int y) sensor, (int x, int y) beacon, int dist)[] sensorBeaconPairs;
        int minX;
        int maxX;
        int minY;
        int maxY;
        int maxDist = 0;

        public Day15(string inputPath) : base(inputPath) 
        {
            sensorBeaconPairs = new ((int x, int y) sensor, (int x, int y) beacon, int dist)[puzzleInputLines.Length];
            foreach (var (line, i) in puzzleInputLines.Select((line, i) => ( line, i ))) {
                var values = Regex.Matches(line, @"[0-9\-]+").Select(x => int.Parse(x.Value)).ToArray();
                (int, int) sensorPos = (values[0], values[1]);
                (int, int) beaconPos = (values[2], values[3]);
                int dist = VectorMaths.ManhattanDist(sensorPos, beaconPos);
                sensorBeaconPairs[i] = (sensorPos, beaconPos, dist);

                // uff
                if (dist > maxDist) maxDist = dist;
                if (values[0] < minX) minX = values[0];
                else if (values[0] > maxX) maxX = values[0];
                if (values[2] < minX) minX = values[2];
                else if (values[2] > maxX) maxX = values[2];
                if (values[1] < minY) minY = values[1];
                else if (values[1] > maxY) maxY = values[1];
                if (values[3] < minY) minY = values[3];
                else if (values[3] > maxY) maxY = values[3];
            }
        }

        public override string Part1()
        {
            long impossibleCount = 0;
            int y = 2000000;
            for (int x = minX - maxDist; x <= maxX + maxDist; x++) {
                foreach (var pair in sensorBeaconPairs) 
                {
                    if (pair.sensor == (x, y) || pair.beacon == (x, y)) {
                        break; // beacon or sensor already here, doesn't count
                    }
                    if (VectorMaths.ManhattanDist(pair.sensor, (x, y)) <= pair.dist) {
                        impossibleCount++;
                        break;
                    }
                }
            }
            return impossibleCount.ToString();
        }

        public override string Part2()
        {
            var pos = GetDistressSignalPos();
            long tuningFreq = ((long)pos.x * 4000000) + ((long)pos.y);
            return tuningFreq.ToString();
        }

        protected (int x, int y) GetDistressSignalPos()
        {
            for (int y = 0; y <= 4000000; y++) {
                for (int x = 0; x <= 4000000; x++) {
                    bool impossible = false;
                    foreach (var pair in sensorBeaconPairs) {
                        if (VectorMaths.ManhattanDist(pair.sensor, (x, y)) <= pair.dist) {
                            impossible = true;
                            // skip sensor range
                            x = pair.sensor.x + pair.dist - Math.Abs(pair.sensor.y - y);
                            break;
                        }
                    }
                    if (!impossible) return (x, y);
                }
            }
            throw new Exception("No distress signal position found");
        }
    }
}