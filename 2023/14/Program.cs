internal class Program
{
    static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();
        List<string> lines = [];
        while (line != null)
        {
            lines.Add(line);
            line = sr.ReadLine();
        }

        // Part 1
        var gridPart1 = MapToGrid(lines);
        TiltNorth(gridPart1);
        int load = GetLoad(gridPart1);
        Console.WriteLine($"Part 1--> The total load is: {load}.");

        // Part 2
        var gridPart2 = MapToGrid(lines);
        List<List<char>> uniqueCycles = [];
        var currentCycle = 1;
        var cycles = 1000000000;
        HashSet<int> loads = [];
        while (currentCycle <= cycles)
        {
            TiltNorth(gridPart2);
            TiltWest(gridPart2);
            TiltSouth(gridPart2);
            TiltEast(gridPart2);
            var currentLoad = GetLoad(gridPart2);
            if (!loads.Add(currentLoad))
            {
                break;
            }
            currentCycle++;
        }
        var cycleLoad = GetLoad(gridPart2);
        Console.WriteLine($"Part 2 --> The total load after {currentCycle - 1} cycles is {cycleLoad}.");

    }

    /// <summary>
    /// Build a dictionary with key (x, y)-coordinates.
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    private static Dictionary<(int, int), char> MapToGrid(List<string> lines)
    {
        Dictionary<(int, int), char> grid = [];
        for (int i = 0; i < lines.First().Length; i++)
        {
            var y = 0;
            foreach (string line in lines)
            {
                grid.Add((i, y), line[i]);
                y++;
            }
        }

        return grid;
    }

    /// <summary>
    /// Tilt the platform north to roll the rocks.
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    private static void TiltNorth(Dictionary<(int, int), char> grid)
    {
        for (int i = 0; i <= grid.Keys.Max(k => k.Item1); i++)
        {
            var topPosition = 0;
            for (int j = 0; j <= grid.Keys.Max(k => k.Item2); j++)
            {
                if (grid[(i, j)] == 'O')
                {
                    grid[(i, topPosition)] = 'O';
                    if ((i, topPosition) != (i, j))
                    {
                        grid[(i, j)] = '.';
                    }
                    topPosition++;
                }
                else if (grid[(i, j)] == '#')
                {
                    topPosition = j + 1;
                }
            }
        }
    }

    /// <summary>
    /// Tilt the platform west to roll the rocks.
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    private static void TiltWest(Dictionary<(int, int), char> grid)
    {
        for (int j = 0; j <= grid.Keys.Max(k => k.Item2); j++)
        {
            var leftPosition = 0;
            for (int i = 0; i <= grid.Keys.Max(k => k.Item1); i++)
            {
                if (grid[(i, j)] == 'O')
                {
                    grid[(leftPosition, j)] = 'O';
                    if ((leftPosition, j) != (i, j))
                    {
                        grid[(i, j)] = '.';
                    }
                    leftPosition++;
                }
                else if (grid[(i, j)] == '#')
                {
                    leftPosition = i + 1;
                }
            }
        }
    }

    /// <summary>
    /// Tilt the platform south to roll the rocks.
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    private static void TiltSouth(Dictionary<(int, int), char> grid)
    {
        for (int i = 0; i <= grid.Keys.Max(k => k.Item1); i++)
        {
            var bottomPosition = grid.Keys.Max(k => k.Item1);
            for (int j = grid.Keys.Max(k => k.Item2); j >= 0; j--)
            {
                if (grid[(i, j)] == 'O')
                {
                    grid[(i, bottomPosition)] = 'O';
                    if ((i, bottomPosition) != (i, j))
                    {
                        grid[(i, j)] = '.';
                    }
                    bottomPosition--;
                }
                else if (grid[(i, j)] == '#')
                {
                    bottomPosition = j - 1;
                }
            }
        }
    }

    /// <summary>
    /// Tilt the platform east to roll the rocks.
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    private static void TiltEast(Dictionary<(int, int), char> grid)
    {
        for (int j = 0; j <= grid.Keys.Max(k => k.Item2); j++)
        {
            var rightPosition = grid.Keys.Max(k => k.Item2);
            for (int i = grid.Keys.Max(k => k.Item1); i >= 0; i--)
            {
                if (grid[(i, j)] == 'O')
                {
                    grid[(rightPosition, j)] = 'O';
                    if ((rightPosition, j) != (i, j))
                    {
                        grid[(i, j)] = '.';
                    }
                    rightPosition--;
                }
                else if (grid[(i, j)] == '#')
                {
                    rightPosition = i - 1;
                }
            }
        }
    }

    /// <summary>
    /// Calculate the load.
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static int GetLoad(Dictionary<(int, int), char> grid)
    {
        var load = 0;
        var rows = grid.Keys.Max(k => k.Item2) + 1;
        foreach (var item in grid)
        {
            if (item.Value == 'O')
            {
                load += rows - item.Key.Item2; 
            }
        }

        return load;
    }

    private static void Print(Dictionary<(int, int), char> grid)
    {
        for (int j = 0; j < grid.Keys.Max(k => k.Item2) + 1; j++)
        {
            for (int i = 0; i < grid.Keys.Max(k => k.Item1) + 1; i++)
            {
                Console.Write(grid[(i, j)]);
            }

            Console.WriteLine("");
        }
        Console.WriteLine();
    }
}

// Part 1 --> 113078
// Part 2 --> 94255
