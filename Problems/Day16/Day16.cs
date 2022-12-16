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

        public override string Part1()
        {
            List<string> usefulValves = allValves.Where(x => x.flowRate > 0).Select(x => x.id).ToList();
            Valve? curr = Valve.valveLookup["AA"];
            return TryPath(curr, usefulValves, new List<string>(), 30, 0).ToString();
        }

        public override string Part2()
        {
            throw new NotImplementedException();
        }

        public int TryPath(
            Valve curr, 
            List<string> closedValves, 
            List<string> openValves, 
            int timeRemaining, 
            int pressureRelease
        ) {
            if (curr.flowRate > 0) {
                timeRemaining -= 1;
                pressureRelease += openValves.Sum(x => Valve.valveLookup[x].flowRate);

                closedValves.Remove(curr.id);
                openValves.Add(curr.id);
            }

            if (closedValves.Count == 0 || timeRemaining == 0) {
                return pressureRelease + timeRemaining * openValves.Sum(x => Valve.valveLookup[x].flowRate);
            }

            int bestFuturePressureRelease = 0;
            string bestId = "";
            foreach (var valveId in closedValves) {
                Valve next = Valve.valveLookup[valveId];
                if (timeRemaining - curr.distanceToOthers[next] > 0) {
                    int pressureReleaseOnJourney = curr.distanceToOthers[next] * openValves.Sum(x => Valve.valveLookup[x].flowRate);
                    int futurePressureRelease = TryPath(
                        next, 
                        new(closedValves), 
                        new(openValves), 
                        timeRemaining - curr.distanceToOthers[next], 
                        pressureReleaseOnJourney
                    );
                    if (futurePressureRelease > bestFuturePressureRelease) {
                        bestFuturePressureRelease = futurePressureRelease;
                        bestId = valveId;
                    }
                }
            }

            return pressureRelease + bestFuturePressureRelease;
        }
    }
}