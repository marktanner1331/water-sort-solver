using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace water_sort_solver
{
    internal class Program
    {
        static List<ConsoleColor>[] InitialTubes;
        static Random Random = new Random();

        static void Main(string[] args)
        {
            InitialTubes = CreateInitialTubes();
            PrintTubes(InitialTubes);

            Console.ReadLine();

            List<Tuple<int, int>> path = null;
            for (int i = 0; i < 10000; i++)
            {
                path = Solve(InitialTubes);
                if(path != null)
                {
                    break;
                }
            }

            if(path == null)
            {
                Console.WriteLine("Couldn't Solve");
                return;
            }

            foreach(var move in path)
            {
                Console.Clear();

                ApplyMove(InitialTubes, move);

                PrintTubes(InitialTubes);
                Console.WriteLine($"{move.Item1 + 1}, {move.Item2 + 1}");

                if(IsComplete(InitialTubes))
                {
                    Console.WriteLine("Complete!");
                    break;
                }

                Console.ReadLine();
            }
        }

        static List<Tuple<int, int>> Solve(List<ConsoleColor>[] tubes)
        {
            List<Tuple<int, int>> path = new List<Tuple<int, int>>();
            HashSet<string> explored = new HashSet<string>();

            while (true)
            {
                if(IsComplete(tubes))
                {
                    return path;
                }

                var moves = GetPossibleMoves(tubes).ToList();

                while (true)
                {
                    if (moves.Count == 0)
                    {
                        return null;
                    }

                    int randomIndex = Random.Next(0, moves.Count);
                    var randomMove = moves[randomIndex];

                    List<ConsoleColor>[] tempTubes = Clone(tubes);
                    ApplyMove(tempTubes, randomMove);

                    var tempTubesHash = TubesToString(tempTubes);
                    if(explored.Contains(tempTubesHash))
                    {
                        moves.RemoveAt(randomIndex);
                    }
                    else
                    {
                        tubes = tempTubes;
                        explored.Add(tempTubesHash);
                        path.Add(randomMove);
                        break;
                    }
                }
            }
        }

        static string TubesToString(List<ConsoleColor>[] tubes)
        {
            return string.Concat(tubes.Select(x => string.Concat(x) + "\n"));
        }

        static List<ConsoleColor>[] Clone(List<ConsoleColor>[] tubes)
        {
            return tubes.Select(x => x.ToList()).ToArray();
        }

        static bool IsComplete(List<ConsoleColor>[] tubes)
        {
            return tubes
                .All(x => x.Count == 0 || (x.Count == 4 && x.Distinct().Count() == 1));
        }

        static void ApplyMove(List<ConsoleColor>[] tubes, Tuple<int, int> move)
        {
            List<ConsoleColor> tube = tubes[move.Item1];

            ConsoleColor topColor = tube.Last();
            int numColors = Enumerable.Reverse(tube).Skip(1).TakeWhile(x => x == topColor).Count() + 1;
            int numDestGaps = 4 - tubes[move.Item2].Count;

            numColors = Math.Min(numColors, numDestGaps);

            tube.RemoveRange(tube.Count - numColors, numColors);
            tubes[move.Item2].AddRange(Enumerable.Repeat(topColor, numColors));
        }

        static IEnumerable<Tuple<int, int>> GetPossibleMoves(List<ConsoleColor>[] tubes)
        {
            for (int i = 0; i < tubes.Length; i++)
            {
                if(tubes[i].Count == 0)
                {
                    continue;
                }

                ConsoleColor topColor = tubes[i].Last();

                for (int j = 0; j < tubes.Length; j++)
                {
                    if(i == j)
                    {
                        continue;
                    }

                    if(tubes[j].Count == 4)
                    {
                        continue;
                    }

                    if(tubes[i].Distinct().Count() == 1 && tubes[j].Count == 0)
                    {
                        continue;
                    }

                    if(tubes[j].Count == 0)
                    {
                        yield return new Tuple<int, int>(i, j);
                    }
                    else
                    {
                        if(tubes[j].Last() == topColor)
                        {
                            yield return new Tuple<int, int>(i, j);
                        }
                    }
                }
            }
        }

        static void PrintTubes(List<ConsoleColor>[] tubes)
        {
            for (int j = 3; j >= 0; j--)
            {
                for (int i = 0; i < tubes.Length; i++)
                {
                    Console.Write("|");

                    if (tubes[i].Count > j)
                    {
                        Console.BackgroundColor = tubes[i][j];
                    }

                    Console.Write("  ");

                    Console.ResetColor();
                    Console.Write("|");
                    Console.Write(" ");
                }

                Console.WriteLine();
            }

            Console.Write(" ");
            for (int i = 0; i < tubes.Length; i++)
            {
                Console.Write("--");
                Console.Write("   ");
            }

            Console.WriteLine();
        }

        private static List<ConsoleColor>[] CreateInitialTubes()
        {
            List<ConsoleColor>[] tubes = Enumerable.Range(0, 14).Select(x => new List<ConsoleColor>()).ToArray();
            tubes[0].Add(ConsoleColor.Magenta);
            tubes[0].Add(ConsoleColor.Blue);
            tubes[0].Add(ConsoleColor.DarkBlue);
            tubes[0].Add(ConsoleColor.Green);
            
            tubes[1].Add(ConsoleColor.Magenta);
            tubes[1].Add(ConsoleColor.Gray);
            tubes[1].Add(ConsoleColor.DarkGreen);
            tubes[1].Add(ConsoleColor.Red);

            tubes[2].Add(ConsoleColor.DarkYellow);
            tubes[2].Add(ConsoleColor.Gray);
            tubes[2].Add(ConsoleColor.Cyan);
            tubes[2].Add(ConsoleColor.DarkCyan);

            tubes[3].Add(ConsoleColor.DarkRed);
            tubes[3].Add(ConsoleColor.DarkGreen);
            tubes[3].Add(ConsoleColor.DarkCyan);
            tubes[3].Add(ConsoleColor.Blue);


            tubes[4].Add(ConsoleColor.DarkRed);
            tubes[4].Add(ConsoleColor.Cyan);
            tubes[4].Add(ConsoleColor.DarkBlue);
            tubes[4].Add(ConsoleColor.DarkBlue);

            tubes[5].Add(ConsoleColor.Red);
            tubes[5].Add(ConsoleColor.Gray);
            tubes[5].Add(ConsoleColor.Yellow);
            tubes[5].Add(ConsoleColor.DarkYellow);

            tubes[6].Add(ConsoleColor.Magenta);
            tubes[6].Add(ConsoleColor.Green);
            tubes[6].Add(ConsoleColor.Blue);
            tubes[6].Add(ConsoleColor.DarkCyan);

            tubes[7].Add(ConsoleColor.Red);
            tubes[7].Add(ConsoleColor.DarkRed);
            tubes[7].Add(ConsoleColor.Gray);
            tubes[7].Add(ConsoleColor.DarkGreen);

            tubes[8].Add(ConsoleColor.Cyan);//
            tubes[8].Add(ConsoleColor.Yellow);
            tubes[8].Add(ConsoleColor.DarkBlue);
            tubes[8].Add(ConsoleColor.Red);

            tubes[9].Add(ConsoleColor.DarkRed);
            tubes[9].Add(ConsoleColor.DarkYellow);
            tubes[9].Add(ConsoleColor.DarkGreen);
            tubes[9].Add(ConsoleColor.Green);


            tubes[10].Add(ConsoleColor.Green);
            tubes[10].Add(ConsoleColor.Cyan);
            tubes[10].Add(ConsoleColor.Blue);
            tubes[10].Add(ConsoleColor.Yellow);

            tubes[11].Add(ConsoleColor.DarkCyan);
            tubes[11].Add(ConsoleColor.DarkYellow);
            tubes[11].Add(ConsoleColor.Magenta);
            tubes[11].Add(ConsoleColor.Yellow);

            return tubes;
        }
    }
}
