namespace AdventOfCode2022
{
    class Day12 : Problem
    {
        protected Dictionary<(int x, int y), Node> allNodes = new();
        protected Node? part1StartNode;
        protected Node? destNode;

        public Day12(string inputPath) : base(inputPath)
        {
            foreach (var (line, y) in puzzleInputLines.Select((value, y) => (value, y))) {
                foreach (var (character, x) in line.Select((value, x) => (value, x))) {
                    if (character == 'S') {
                        part1StartNode = new Node((x, y), 0);
                        allNodes[(x, y)] = (part1StartNode);
                    } else if (character == 'E') {
                        destNode = new Node((x, y), 25);
                        allNodes[(x, y)] = (destNode);
                    } else {
                        allNodes[(x, y)] = (new Node((x, y), character - 'a'));
                    }
                }
            }

            foreach (Node node in allNodes.Values) {
                node.FindNeighbours(allNodes);
            }
        }

        public override string Part1()
        {
            if (part1StartNode == null || destNode == null) {
                return "Missing start or destination node";
            }

            return ShortestPathBetween(part1StartNode, destNode).ToString();
        }

        public override string Part2()
        {
            if (destNode == null) {
                return "Missing destination node";
            }

            int shortestPath = int.MaxValue;
            var startingPoints = allNodes.Values.Where(x => x.height == 0);
            foreach (Node startNode in startingPoints) {
                try {
                    int pathFromHere = ShortestPathBetween(startNode, destNode);
                    if (pathFromHere <= shortestPath) {
                        shortestPath = pathFromHere;
                    }
                } catch (NoPathException) { // no path exists
                    continue;
                }
            }
            return shortestPath.ToString();
        }

        protected int ShortestPathBetween(Node startNode, Node destNode) {
            ResetNodes();
            startNode.distFromStart = 0;

            PriorityQueue<Node, int> nodeQueue = new PriorityQueue<Node, int>();
            nodeQueue.Enqueue(startNode, 0);

            while (nodeQueue.Count > 0) {
                Node curr = nodeQueue.Dequeue();

                if (curr.visited) { // skip - already been here with a better dist (because p queue has no adjust priority method :( )
                    continue;
                }

                if (curr == destNode) {
                    return curr.distFromStart;
                }
                
                curr.visited = true;
                foreach (Node neighbour in curr.neighbours.Where(x => !x.visited)) {
                    int dist = curr.distFromStart + 1;
                    if (dist <= neighbour.distFromStart) {
                        neighbour.distFromStart = dist;
                        nodeQueue.Enqueue(neighbour, dist);
                    }
                }
            }

            throw new NoPathException();
        }

        protected void ResetNodes()
        {
            foreach (Node n in allNodes.Values) {
                n.distFromStart = int.MaxValue;
                n.visited = false;
            }
        }
    }

    [System.Serializable]
    public class NoPathException : System.Exception {}
}