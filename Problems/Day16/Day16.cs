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
            Valve? curr = Valve.lookup["AA"];
            return TryPath(curr, usefulValves, new List<string>(), 30, 0).ToString();
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
                Valve next = Valve.lookup[valveId];
                if (timeRemaining - curr.distances[next] > 0) {
                    int pressureReleaseOnJourney = curr.distances[next] * openValves.Sum(x => Valve.lookup[x].flowRate);
                    int futurePressureRelease = TryPath(
                        next, 
                        new(closedValves), 
                        new(openValves), 
                        timeRemaining - curr.distances[next], 
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
            return TryPathDuo(curr, curr, usefulValves, new List<string>(), 27).ToString();
        }

        public int TryPathDuo(
            Valve curr, 
            Valve eleCurr,
            List<string> closed, 
            List<string> open, 
            int timer,
            int flow = 0,
            Valve? target = null,
            int targetDist = 0,
            Valve? eleTarget = null,
            int eleTargetDist = 0
        ) {
            timer--; //time ticks down every iteration
            flow += open.Sum(x => Valve.lookup[x].flowRate); //pressure releases every iteration

            if (curr == target) { // We have spent a minute opening a valve
                target = null;
                closed.Remove(curr.id);
                open.Add(curr.id);
            } else if (target != null && --targetDist == 0) { // We reached our destination
                curr = target;
            }
            
            if (eleCurr == eleTarget) { // Ele has spent a minute opening a valve
                eleTarget = null;
                closed.Remove(eleCurr.id);
                open.Add(eleCurr.id);
            } else if (eleTarget != null && --eleTargetDist == 0) { // Ele reached their destination
                eleCurr = eleTarget;
            }

            if (closed.Count == 0 || timer == 0) { // Out of time or valves
                return flow + timer * open.Sum(x => Valve.lookup[x].flowRate);
            }

            int bestFlow = flow;
            if (target == null && eleTarget == null) { // We both need a target
                foreach (string id in closed) {
                    foreach (string eleId in closed) {
                        if (id != eleId) {
                            int testFlow = TryPathDuo(
                                curr, eleCurr, new(closed), new(open), timer, flow, Valve.lookup[id], curr.distances[Valve.lookup[id]], Valve.lookup[eleId], eleCurr.distances[Valve.lookup[eleId]]
                            );
                            if (testFlow > bestFlow) {
                                bestFlow = testFlow;
                            }
                        }
                    }
                }
            } else if (eleTarget != null && target == null) { // We need target, ele moving
                foreach (string id in closed.Where(x => x != eleTarget.id)) {
                    int testFlow = TryPathDuo(
                        curr, eleCurr, new(closed), new(open), timer, flow, Valve.lookup[id], curr.distances[Valve.lookup[id]], eleTarget, eleTargetDist
                    );
                    if (testFlow > bestFlow) {
                        bestFlow = testFlow;
                    }
                }
            } else if (target != null && eleTarget == null) { // Ele needs target, we are moving
                foreach (string id in closed.Where(x => x != target.id)) {
                    int testFlow = TryPathDuo(
                        curr, eleCurr, new(closed), new(open), timer, flow, target, targetDist, Valve.lookup[id], eleCurr.distances[Valve.lookup[id]]
                    );
                    if (testFlow > bestFlow) {
                        bestFlow = testFlow;
                    }
                }
            } else { // Both moving
                int movementFlow = 0;
                if (targetDist > 0 && eleTargetDist > 0) {
                    // Skip time where both parties are just walking, leaving 1 walk duration left on the shorter path
                    int elapsed = Math.Min(targetDist, eleTargetDist) - 1;
                    targetDist -= elapsed;
                    eleTargetDist -= elapsed;
                    timer -= elapsed;
                    movementFlow = elapsed * open.Sum(x => Valve.lookup[x].flowRate);
                }
                bestFlow = TryPathDuo(curr, eleCurr, new(closed), new(open), timer, flow + movementFlow, target, targetDist, eleTarget, eleTargetDist);
            }
            return bestFlow;
        }
    }
}