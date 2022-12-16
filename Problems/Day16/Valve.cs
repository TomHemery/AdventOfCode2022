using System.Text;
namespace AdventOfCode2022 {
    class Valve
    {
        public static Dictionary<string, Valve> lookup = new();
        public Dictionary<Valve, int> distances = new();
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
            lookup[id] = this;
            this.neighbourIds = neighbourIds;
            this.flowRate = flowRate;
        }

        public void SaveNeighbours()
        {
            foreach (string id in neighbourIds) {
                this.neighbours.Add(lookup[id]);
            }
        }

        public void CalculateDistances() // hello day 12
        {
            foreach (Valve v in lookup.Values) {
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

            foreach (Valve v in lookup.Values) {
                this.distances[v] = v.distFromStart;
            }
        }

        public override string ToString()
        {
            return String.Format("Valve {0} with flowRate {1}");
        }
    }
}