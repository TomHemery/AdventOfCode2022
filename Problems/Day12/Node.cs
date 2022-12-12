namespace AdventOfCode2022
{

    class Node
    {
        public List<Node> neighbours = new();
        public (int x, int y) position;
        public int height;

        public int distFromStart = int.MaxValue;
        public bool visited = false;

        public Node((int x, int y) position, int height) {
            this.position = position;
            this.height = height;
        }

        public void FindNeighbours(Dictionary<(int x, int y), Node> allNodes) {
            if (allNodes.ContainsKey((position.x - 1, position.y)) && height + 1 >= allNodes[(position.x - 1, position.y)].height) {
                neighbours.Add(allNodes[(position.x - 1, position.y)]);
            }
            if (allNodes.ContainsKey((position.x + 1, position.y)) && height + 1 >= allNodes[(position.x + 1, position.y)].height) {
                neighbours.Add(allNodes[(position.x + 1, position.y)]);
            }
            if (allNodes.ContainsKey((position.x, position.y - 1)) && height + 1 >= allNodes[(position.x, position.y - 1)].height) {
                neighbours.Add(allNodes[(position.x, position.y - 1)]);
            }
            if (allNodes.ContainsKey((position.x, position.y + 1)) && height + 1 >= allNodes[(position.x, position.y + 1)].height) {
                neighbours.Add(allNodes[(position.x, position.y + 1)]);
            }
        }
    }
}