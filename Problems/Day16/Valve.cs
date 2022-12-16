using System.Text;
namespace AdventOfCode2022 {
    class Valve
    {
        public static Dictionary<string, Valve> valveLookup = new();
        public Dictionary<Valve, int> distanceToOthers = new();
        public int flowRate;
        public List<Valve> neighbours = new();
        public string id {get; protected set;}
        protected string[] neighbourIds;

        // Shortest Path Helper Vars
        public bool visited = false;
        public int distFromStart = int.MaxValue;

        public Valve(string id, int flowRate, string[] neighbourIds)
        {
            this.id = id;
            valveLookup[id] = this;
            this.neighbourIds = neighbourIds;
            this.flowRate = flowRate;
        }

        public void SaveNeighbours()
        {
            foreach (string id in neighbourIds) {
                this.neighbours.Add(valveLookup[id]);
            }
        }

        public void CalculateDistances() // hello day 12
        {
            foreach (Valve v in valveLookup.Values) {
                v.visited = false;
                v.distFromStart = int.MaxValue;
            }

            PriorityQueue<Valve, int> valveQueue = new PriorityQueue<Valve, int>();
            this.distFromStart = 0;
            valveQueue.Enqueue(this, 0);

            while (valveQueue.Count > 0) { //Djikstra
                Valve curr = valveQueue.Dequeue();

                if (curr.visited) {
                    continue;
                }

                curr.visited = true;
                int dist = curr.distFromStart + 1;
                foreach (Valve neighbour in curr.neighbours.Where(x => !x.visited)) {
                    if (dist <= neighbour.distFromStart) {
                        neighbour.distFromStart = dist;
                        valveQueue.Enqueue(neighbour, dist);
                    }
                }
            }

            foreach (Valve v in valveLookup.Values) {
                this.distanceToOthers[v] = v.distFromStart;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("Valve {0} with flowRate {1}\n", this.id, this.flowRate));
            foreach (var kvp in distanceToOthers)
            {
                builder.Append(String.Format("\tValve {0} is {1} away\n", kvp.Key.id, kvp.Value));
            }
            return builder.ToString();
        }
    }
}