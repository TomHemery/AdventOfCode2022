namespace AdventOfCode2022
{
    class Day12 : Problem
    {
        protected Dictionary<(int x, int y), Node> allNodes = new();
        protected Node? part1StartNode;
        protected Node? highPoint;

        public Day12(string inputPath) : base(inputPath)
        {
            foreach (var (line, y) in puzzleInputLines.Select((value, y) => (value, y))) {
                foreach (var (character, x) in line.Select((value, x) => (value, x))) {
                    if (character == 'S') {
                        part1StartNode = new Node((x, y), 0);
                        allNodes[(x, y)] = (part1StartNode);
                    } else if (character == 'E') {
                        highPoint = new Node((x, y), 25);
                        allNodes[(x, y)] = (highPoint);
                    } else {
                        allNodes[(x, y)] = (new Node((x, y), character - 'a'));
                    }
                }
            }
        }

        public override string Part1()
        {
            if (part1StartNode == null || highPoint == null) {
                return "Missing start or destination node";
            }

            return ShortestPath((h1, h2) => h1 + 1 >= h2, part1StartNode, highPoint).ToString();
        }

        public override string Part2()
        {
            if (highPoint == null) {
                return "Missing high point node";
            }

            int shortestPath = int.MaxValue;
            var lowPoints = allNodes.Values.Where(x => x.height == 0);
            ShortestPath((h1, h2) => h1 <= h2 + 1, highPoint);

            foreach (Node node in lowPoints) {
                if (shortestPath > node.distFromStart) {
                    shortestPath = node.distFromStart;
                }
            }

            return shortestPath.ToString();
        }

        // Djikstra with optional destination node, validNeighbour funcion takes heights and tells us if 2 nodes can be neighbours
        protected int ShortestPath(Func<int, int, bool> validNeighbour, Node startNode, Node ?destNode = null) {
            ResetNodes(validNeighbour);
            startNode.distFromStart = 0;

            PriorityQueue<Node, int> nodeQueue = new PriorityQueue<Node, int>();
            nodeQueue.Enqueue(startNode, 0);

            while (nodeQueue.Count > 0) {
                Node curr = nodeQueue.Dequeue();

                if (curr.visited) { // skip - already been here with a better dist (because p queue has no adjust priority method :( )
                    continue;
                }

                if (destNode != null && curr == destNode) {
                    return curr.distFromStart;
                }
                
                curr.visited = true;
                int dist = curr.distFromStart + 1;
                foreach (Node neighbour in curr.neighbours.Where(x => !x.visited)) {
                    if (dist <= neighbour.distFromStart) {
                        neighbour.distFromStart = dist;
                        nodeQueue.Enqueue(neighbour, dist);
                    }
                }
            }

            return 0;
        }

        protected void ResetNodes(Func<int, int, bool> validNeighbour)
        {
            foreach (Node n in allNodes.Values) {
                n.distFromStart = int.MaxValue;
                n.visited = false;
                n.FindNeighbours(allNodes, validNeighbour);
            }
        }
    }
}