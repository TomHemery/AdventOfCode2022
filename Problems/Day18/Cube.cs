namespace AdventOfCode2022
{
    class Cube
    {
        public (int x, int y, int z) position {get; protected set;}
        public List<Cube> neighbours {get; protected set;} = new();

        public Cube((int x, int y, int z) position)
        {
            this.position = position;
        }

        public Cube(int x, int y, int z)
        {
            position = (x, y, z);
        }

        public void FindNeighboursIn(Dictionary<(int x, int y, int z), Cube> allCubes)
        {
            if (allCubes.ContainsKey((position.x - 1, position.y, position.z))) {
                neighbours.Add(allCubes[(position.x - 1, position.y, position.z)]);
            }
            if (allCubes.ContainsKey((position.x + 1, position.y, position.z))) {
                neighbours.Add(allCubes[(position.x + 1, position.y, position.z)]);
            }
            if (allCubes.ContainsKey((position.x, position.y - 1, position.z))) {
                neighbours.Add(allCubes[(position.x, position.y - 1, position.z)]);
            }
            if (allCubes.ContainsKey((position.x, position.y + 1, position.z))) {
                neighbours.Add(allCubes[(position.x, position.y + 1, position.z)]);
            }
            if (allCubes.ContainsKey((position.x, position.y, position.z - 1))) {
                neighbours.Add(allCubes[(position.x, position.y, position.z - 1)]);
            }
            if (allCubes.ContainsKey((position.x, position.y, position.z + 1))) {
                neighbours.Add(allCubes[(position.x, position.y, position.z + 1)]);
            }
        }
    }
}