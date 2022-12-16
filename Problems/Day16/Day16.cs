using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    class Day16 : Problem
    {
        protected Valve[] allValves;

        protected static Stopwatch stopwatch = new Stopwatch();

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
            Valve? curr = Valve.lookup["AA"];
            return FindFlow(curr, usefulValves, new List<string>(), 30, 0).ToString();
        }

        protected int FindFlow(
            Valve curr, 
            List<string> closedValves, 
            List<string> openValves, 
            int timeRemaining, 
            int pressureRelease
        ) {
            if (curr.flowRate > 0) {
                timeRemaining -= 1;
                pressureRelease += openValves.Sum(x => Valve.lookup[x].flowRate);

                closedValves.Remove(curr.id);
                openValves.Add(curr.id);
            }

            if (closedValves.Count == 0 || timeRemaining == 0) {
                return pressureRelease + timeRemaining * openValves.Sum(x => Valve.lookup[x].flowRate);
            }

            int bestFuturePressureRelease = 0;
            string bestId = "";
            foreach (var valveId in closedValves) {
                if (timeRemaining - curr.distances[valveId] > 0) {
                    int pressureReleaseOnJourney = curr.distances[valveId] * openValves.Sum(x => Valve.lookup[x].flowRate);
                    int futurePressureRelease = FindFlow(
                        Valve.lookup[valveId], 
                        new(closedValves), 
                        new(openValves), 
                        timeRemaining - curr.distances[valveId], 
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

        public override string Part2()
        {
            List<string> usefulValves = allValves.Where(x => x.flowRate > 0).Select(x => x.id).ToList();
            Valve? curr = Valve.lookup["AA"];
            return FindFlow("AA", "AA", usefulValves, new List<string>(), 26).ToString();
        }

        protected int FindFlow(
            string curr, 
            string eleCurr,
            List<string> closed, 
            List<string> open, 
            int timer,
            int flow = 0,
            string target = "",
            int targetDist = 0,
            string eleTarget = "",
            int eleTargetDist = 0
        ) {
            if (timer == 26) {
                stopwatch.Restart();
            }
            if (timer == 25) { 
                stopwatch.Stop();
                Console.WriteLine(stopwatch.ElapsedMilliseconds + "ms, whole thing will take: " + stopwatch.ElapsedMilliseconds * 210 + "ms ish");
                Console.WriteLine(target + ", " + eleTarget);
                stopwatch.Restart();
            }
            UpdateClosedOpen(ref curr, ref target, targetDist, open, closed);
            UpdateClosedOpen(ref eleCurr, ref eleTarget, eleTargetDist, open, closed);

            if (closed.Count == 0 || timer == 0) { // Out of time or valves
                return flow + timer * open.Sum(x => Valve.lookup[x].flowRate);
            }

            int bestFlow = flow;
            if (target == "" && eleTarget == "" && closed.Count >= 2) { // We both need a target
                foreach (var (id, i) in closed.Select((id, i) => (id, i))) {
                    foreach (var eleId in closed.Where(x => x != id)) {
                        if (id == eleId) {
                            continue;
                        }
                        int testFlow = FindFlow(
                            curr, eleCurr, new(closed), new(open), timer - 1, flow + getFlow(open), id, Valve.lookup[curr].distances[id] - 1, eleId, Valve.lookup[eleCurr].distances[eleId] - 1
                        );
                        if (testFlow > bestFlow) {
                            bestFlow = testFlow;
                        }
                    }
                }
                return bestFlow;
            } else if (target == "" && (eleTarget != "" && closed.Count >= 2 || eleTarget == "" && closed.Count >= 1)) { // We need target and it can't be where ele is going
                foreach (string id in closed.Where(x => x != eleTarget)) {
                    int testFlow = FindFlow(
                        curr, eleCurr, new(closed), new(open), timer - 1, flow + getFlow(open), id, Valve.lookup[curr].distances[id] - 1, eleTarget, eleTargetDist - 1
                    );
                    if (testFlow > bestFlow) {
                        bestFlow = testFlow;
                    }
                }
                return bestFlow;
            } else if (eleTarget == "" && (target != "" && closed.Count >= 2 || target == "" && closed.Count >= 1)) { // Ele needs target and it can't be where we're going
                foreach (string id in closed.Where(x => x != target)) {
                    int testFlow = FindFlow(
                        curr, eleCurr, new(closed), new(open), timer - 1, flow + getFlow(open), target, targetDist - 1, id, Valve.lookup[eleCurr].distances[id] - 1
                    );
                    if (testFlow > bestFlow) {
                        bestFlow = testFlow;
                    }
                }
                return bestFlow;
            } 

            // Skip time where both parties are just walking. 1 time step always passes.
            int elapsed = Math.Max(1, Math.Min(targetDist, eleTargetDist));
            if (elapsed >= timer) elapsed = timer;
            targetDist -= elapsed;
            eleTargetDist -= elapsed;
            timer -= elapsed;
            return FindFlow(curr, eleCurr, closed, open, timer, flow + elapsed * getFlow(open), target, targetDist, eleTarget, eleTargetDist);
        }

        protected void UpdateClosedOpen(ref string curr, ref string target, int targetDist, List<string> open, List<string> closed) 
        {
            if (curr == target) { // We have spent a minute opening a valve
                target = "";
                closed.Remove(curr);
                open.Add(curr);
            } else if (target != "" && targetDist <= 0) { // We reached our destination
                curr = target;
            }
        }

        protected int getFlow(List<string> open)
        {
            return open.Sum(x => Valve.lookup[x].flowRate);
        }
    }
}