using System.Net;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    class Day16 : Problem
    {
        protected Valve[] allValves;
        public Day16(string inputPath) : base(inputPath)
        {
            allValves = new Valve[puzzleInputLines.Length];
            foreach (var (line, i) in puzzleInputLines.Select((line, i) => ( line, i ))) {
                string[] parts = line.Replace("valve ", "valves ").Split("; ");
                string id = parts[0].Split(" ")[1];
                int flowRate = Regex.Matches(parts[0], @"[0-9\-]+").Select(x => int.Parse(x.Value)).First();
                string[] neighbourIds = parts[1].Split(" valves ")[1].Split(", ");
                allValves[i] = new Valve(id, flowRate, neighbourIds);
            }

            foreach (Valve v in allValves) {
                v.SaveNeighbours();
            }

            foreach (Valve v in allValves) {
                v.CalculateDistances();
            }
        }

        protected int AdvanceTime(ref int timeRemaining, int elapsed, List<Valve> openValves)
        {
            int pressureRelease = 0;
            for (; elapsed > 0; elapsed --) {
                timeRemaining--;
                foreach (Valve valve in openValves) {
                    pressureRelease += valve.flowRate;
                }
                Console.WriteLine(timeRemaining + ": " + pressureRelease);
            }
            return pressureRelease;
        }

        public override string Part1()
        {
            List<Valve> usefulValves = allValves.Where(x => x.flowRate > 0).ToList();
            List<Valve> openValves = new();
            Valve? curr = Valve.valveLookup["AA"];
            int timeRemaining = 30;
            int pressureRelease = 0;
            while (timeRemaining > 0 && usefulValves.Count > 0 && curr != null)
            {
                int bestValue = int.MinValue;
                Valve? bestValve = null;

                foreach (Valve other in usefulValves) {
                    // value of another valve is the time it has to vent pressure, t remaining minus time to get there minus 1 minute to open
                    int value = (timeRemaining - curr.distanceToOthers[other] - 1) * other.flowRate;
                    if (value > bestValue) {
                        bestValue = value;
                        bestValve = other;
                    }
                }
                Console.WriteLine(string.Format("At valve {0}, moving to valve {1}", curr.id, bestValve.id));
                pressureRelease += AdvanceTime(ref timeRemaining, curr.distanceToOthers[bestValve], openValves); // time to move to other valve
                usefulValves.Remove(bestValve);
                openValves.Add(bestValve);
                pressureRelease += AdvanceTime(ref timeRemaining, 1, openValves); // time to open valve
                curr = bestValve;
            }
            return pressureRelease.ToString();
        }

        public override string Part2()
        {
            throw new NotImplementedException();
        }
    }
}