using AdventOfCode2022.Helpers;

namespace AdventOfCode2022
{
    public class Rock
    {
        protected readonly char[][,] shapes = {
            new char [,] {{'#', '#', '#', '#'}},
            new char [,] {{'.', '#', '.'}, {'#', '#', '#'}, {'.', '#', '.'}},
            new char [,] {{'#', '#', '#'}, {'.', '.', '#'}, {'.', '.', '#'}},
            new char [,] {{'#'}, {'#'}, {'#'}, {'#'}},
            new char [,] {{'#', '#'}, {'#', '#'}}
        };

        protected static Dictionary<(int, string), int>[] allHistorys = {
            new (),
            new(),
            new(),
            new(),
            new()
        };

        protected char[,] shape;
        protected Dictionary<(int index, string topRow), int> history;
        protected (int x, int y) position; // bottom left corner
        

        public Rock(int index, int yPosition) 
        {
            shape = shapes[index % shapes.Length];
            history = allHistorys[index % shapes.Length];
            this.position = (2, yPosition);
        }

        public int Fall(string jetDirs, ref int index, Dictionary<(int x, int y), char> cave, int prevHighestY)
        {
            while (Update(jetDirs[index], cave)) {
                index = (index + 1) % jetDirs.Length;
            }
            index = (index + 1) % jetDirs.Length;
            for (int y = 0; y < shape.GetLength(0); y++) {
                for (int x = 0; x < shape.GetLength(1); x++) {
                    if (shape[y, x] == '#') {
                        cave[VectorMaths.Sum(position, (x, y))] = '#';
                    }
                }
            }

            return position.y + shape.GetLength(0) - 1;
        }

        protected bool Update(char jetDir, Dictionary<(int x, int y), char> cave)
        {
            (int x, int y) oldPos = position;
            ApplyJet(jetDir, cave);
            return MoveDown(cave);
        }

        protected void ApplyJet(char jetDir, Dictionary<(int x, int y), char> cave) 
        {
            (int x, int y) jet = jetDir == '>' ? (1, 0) : (-1, 0);
            (int x, int y) newPosition = VectorMaths.Sum(position, jet);

            if (jetDir == '<' && newPosition.x < 0) { // hit cave wall
                return;
            } else if (jetDir == '>' && newPosition.x + shape.GetLength(1) - 1 >= 7) { // hit cave wall
                return;
            }

            for (int x = 0; x < shape.GetLength(1); x++) {
                for (int y = 0; y < shape.GetLength(0); y++) { // Rock on rock collision
                    if (
                        shape[y, x] == '#' && 
                        cave.ContainsKey((newPosition.x + x, newPosition.y + y)) && cave[(newPosition.x + x, newPosition.y + y)] == '#'
                    ) {
                        return;
                    }
                }
            }

            position = newPosition;
        }

        protected bool MoveDown(Dictionary<(int x, int y), char> cave)
        {
            (int x, int y) newPosition = VectorMaths.Sum(position, (0, -1));

            if (newPosition.y <= 0) { // hit cave floor
                return false;
            }

            for (int x = 0; x < shape.GetLength(1); x ++) { // Rock on rock collision
                for (int y = 0; y < shape.GetLength(0); y++) {
                    if (
                        shape[y, x] == '#' &&
                        cave.ContainsKey((newPosition.x + x, newPosition.y + y))
                    ) {
                        return false;
                    }         
                }              
            }

            position = newPosition;
            return true;
        }

        public void DrawSelf()
        {
            for (int y = shape.GetLength(0) - 1; y >= 0; y --) {
                for (int x = 0; x < shape.GetLength(1); x++) {
                    Console.Write(shape[y, x]);
                }
                Console.WriteLine();
            }
        }
    }
}