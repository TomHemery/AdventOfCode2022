using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2022.Helpers;

namespace AdventOfCode2022
{
    class Day22 : Problem
    {
        Dictionary<(int x, int y), char> map = new Dictionary<(int x, int y), char>();
        (int x, int y) position;
        (int x, int y) heading = (1, 0);
        (int dist, char? dir)[] moves;

        int maxX = 0;
        int maxY;

        Dictionary<(int x, int y), (int x, int y)> history = new();

        public Day22(string inputPath) : base(inputPath)
        {
            string[] boardLines = rawPuzzleInput.Split("\n\n")[0].Split("\n");
            maxY = boardLines[1].Length - 1;
            for (int y = 0; y < boardLines.Length; y++) {
                maxX = maxX < boardLines[y].Length - 1 ? boardLines[y].Length - 1: maxX;
                for (int x = 0; x < boardLines[y].Length; x++) {
                    map[(x, y)] = boardLines[y][x];
                }
            }

            string[] moveStrings = Regex.Matches(rawPuzzleInput.Split("\n\n")[1], "[0-9]+[LR]?").Select(x => x.ToString()).ToArray();
            moves = moveStrings.Select(x => CreateMove(x)).ToArray();
        }

        protected (int dist, char? dir) CreateMove(string moveDef)
        {
            int dist = int.Parse(Regex.Match(moveDef, "[0-9]+").ToString());
            if (moveDef.Contains('R')) {
                return (dist, 'R');
            } else if (moveDef.Contains('L')) {
                return (dist, 'L');
            }
            return (dist, null);
        }

        protected (int x, int y) GetNewHeading((int x, int y) heading, (int dist, char? dir) move)
        {
            if (move.dir != null) {
                // y is inverted (1 is down), go R by rotating anti clockwise
                heading = move.dir == 'R' ? VectorMaths.RotateAntiClockwise90(heading) : VectorMaths.RotateClockwise90(heading);
            }
            return heading;
        }

        protected void Wrap(ref (int x, int y) position, (int x, int y) heading) 
        {
            // Constrain to map dimensions
            position.x = position.x > maxX ? 0 : position.x < 0 ? maxX : position.x;
            position.y = position.y > maxY ? 0 : position.y < 0 ? maxY : position.y; 

            // Constrain to mapped area
            while(!map.ContainsKey(position) || map[position] == ' ') {
                position = VectorMaths.Add(position, heading);

                // Reconstrain to map dimensions
                position.x = position.x > maxX ? 0 : position.x < 0 ? maxX : position.x;
                position.y = position.y > maxY ? 0 : position.y < 0 ? maxY : position.y; 
            }
        }

        protected int GetFacingValue((int x, int y) heading) {
            if (heading.x == 1) {
                return 0;
            } else if (heading.x == -1) {
                return 2;
            } else if (heading.y == 1) {
                return 1;
            } else if (heading.y == -1) {
                return 3;
            }
            throw new Exception("Invalid heading " + heading);
        }

        public override string Part1()
        {
            Wrap(ref position, heading);

            // DEBUG
            history[position] = heading;
            PrintMap(map, history, position);
            // DEBUG

            foreach (var move in moves) {
                // DEBUG
                Console.WriteLine(position + " :: " + heading + " ==> " + move);
                // DEBUG

                for (int i = 0; i < move.dist; i++) {
                    (int x, int y) next = VectorMaths.Add(position, heading);
                    Wrap(ref next, heading);
                    if (map[next] == '.') {
                        position = next;
                    } else if (map[next] == '#') {
                        Console.WriteLine("Hit a wall at " + next);
                        break;
                    }
                    history[position] = heading;
                }
                heading = GetNewHeading(heading, move);

                // DEBUG
                history[position] = heading;
                PrintMap(map, history, position);
                Console.WriteLine(position + " :: " + heading);
                Console.ReadKey();
                // DEBUG
            }
            
            // DEBUG
            Console.WriteLine(position + " :: " + heading);
            // DEBUG

            return (1000 * (position.y + 1) + 4 * (position.x + 1) + GetFacingValue(heading)).ToString();
        }

        public override string Part2()
        {
            throw new NotImplementedException();
        }

        protected void PrintMap(Dictionary<(int x, int y), char> map, Dictionary<(int x, int y), (int x, int y)> history, (int x, int y) lastPos)
        {
            for (int i = 0; i < maxX; i++) Console.Write("=");
            Console.WriteLine();
            for (int y = 0; y <= maxY; y++) {
                for (int x = 0; x <= maxX; x++) {
                    if ((x, y) == lastPos) {
                        ConsoleColor def = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("!");
                        Console.ForegroundColor = def;
                    } else if (history.ContainsKey((x, y))) {
                        switch (history[(x, y)]) {
                            case (1, 0):
                                Console.Write(">");
                                break;
                            case (-1, 0):
                                Console.Write("<");
                                break;
                            case (0, 1):
                                Console.Write("v");
                                break;
                            case (0, -1):
                                Console.Write("^");
                                break;
                        }
                    } else {
                        if (map.ContainsKey((x, y))) {
                            Console.Write(map[(x, y)]);
                        } else {
                            Console.Write(' ');
                        }
                    }
                }
                Console.WriteLine();
            }
            for (int i = 0; i < maxX; i++) Console.Write("=");
            Console.WriteLine();
        }
    }
}