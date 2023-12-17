using _16;

internal class Program
{
    private static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();
        Dictionary<(int, int), char> grid = [];
        var row = 0;
        while (line != null)
        {
            var column = 0;
            foreach (char c in line)
            {
                grid.Add((column, row), c);
                column++;
            }
            row++;
            line = sr.ReadLine();
        }

        // Part 1
        (int, int, Direction) startingLocation = (0, 0, Direction.Right);
        Dictionary<(int, int, Direction), char> energizedTilesWithDirection = [];
        FollowBeam(startingLocation, grid, energizedTilesWithDirection);
        Dictionary<(int, int), char> energizedTiles = [];
        foreach (var tile in energizedTilesWithDirection)
        {
            if (!energizedTiles.ContainsKey((tile.Key.Item1, tile.Key.Item2)))
            {
                energizedTiles.Add((tile.Key.Item1, tile.Key.Item2), tile.Value);
            }
        }
        Console.WriteLine($"Part 1 --> There are {energizedTiles.Count} energized tiles.");

        // Part 2
        var endColumn = grid.Select(g => g.Key.Item1).Max();
        var endRow = grid.Select(g => g.Key.Item2).Max();
        List<(int, int, Direction)> startingLocations = FindAllStartingLocations(endColumn, endRow);
        int maxTilesEnergized = FindMostTilesEnergized(startingLocations, grid);
        Console.WriteLine($"Part 2 --> There are {maxTilesEnergized} energized tiles at the most.");
    }

    /// <summary>
    /// From a specified starting location and direction, follow the beam accordingly and update the energizedTiles-dictionary.
    /// </summary>
    /// <param name="location"></param>
    /// <param name="grid"></param>
    /// <param name="energizedTilesWithDirection"></param>
    private static void FollowBeam((int, int, Direction) location, Dictionary<(int, int), char> grid, Dictionary<(int, int, Direction), char> energizedTilesWithDirection)
    {
        List<Direction> directions = [];
        if (grid.TryGetValue((location.Item1, location.Item2), out char value))
        {
            if (!energizedTilesWithDirection.ContainsKey((location.Item1, location.Item2, location.Item3)))
            {
                energizedTilesWithDirection.TryAdd((location.Item1, location.Item2, location.Item3), value);

                // If we are still on the grid we continue, else we have traveled too far and are beyond the grid
                if (grid.ContainsKey((location.Item1, location.Item2)))
                {
                    var newLocations = NewLocations((location.Item1, location.Item2, location.Item3), grid[(location.Item1, location.Item2)]);
                    foreach (var newLocation in newLocations)
                    {
                        FollowBeam(newLocation, grid, energizedTilesWithDirection);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Based on location, direction and character we get the next location.
    /// </summary>
    /// <param name="location"></param>
    /// <param name="character"></param>
    /// <returns></returns>
    private static List<(int, int, Direction)> NewLocations((int, int, Direction) location, char character)
    {
        List<Direction> newDirections = NewDirections(location.Item3, character);
        List<(int, int, Direction)> newLocations = [];
        foreach (var newDirection in newDirections)
        {
            if (newDirection == Direction.Up)
            {
                newLocations.Add((location.Item1, location.Item2 - 1, newDirection));
            }
            else if (newDirection == Direction.Right)
            {
                newLocations.Add((location.Item1 + 1, location.Item2, newDirection));
            }
            else if (newDirection == Direction.Down)
            {
                newLocations.Add((location.Item1, location.Item2 + 1, newDirection));
            }
            else
            {
                newLocations.Add((location.Item1 - 1, location.Item2, newDirection));
            }
        }

        return newLocations;
    }

    /// <summary>
    /// Based on direction and current character, get new directions.
    /// </summary>
    /// <param name="oldDirection"></param>
    /// <param name="character"></param>
    /// <returns></returns>
    private static List<Direction> NewDirections(Direction oldDirection, char character)
    {
        List<Direction> newDirections = [];
        if (character == '\\')
        {
            switch (oldDirection)
            {
                case Direction.Up:
                    newDirections.Add(Direction.Left);
                    break;
                case Direction.Right:
                    newDirections.Add(Direction.Down);
                    break;
                case Direction.Down:
                    newDirections.Add(Direction.Right);
                    break;
                case Direction.Left:
                    newDirections.Add(Direction.Up);
                    break;
                default:
                    break;
            }
        }
        else if (character == '/')
        {
            switch (oldDirection)
            {
                case Direction.Up:
                    newDirections.Add(Direction.Right);
                    break;
                case Direction.Right:
                    newDirections.Add(Direction.Up);
                    break;
                case Direction.Down:
                    newDirections.Add(Direction.Left);
                    break;
                case Direction.Left:
                    newDirections.Add(Direction.Down);
                    break;
                default:
                    break;
            }
        }
        else if (character == '|')
        {
            switch (oldDirection)
            {
                case Direction.Up:
                    newDirections.Add(Direction.Up);
                    break;
                case Direction.Right:
                    newDirections.Add(Direction.Up);
                    newDirections.Add(Direction.Down);
                    break;
                case Direction.Down:
                    newDirections.Add(Direction.Down);
                    break;
                case Direction.Left:
                    newDirections.Add(Direction.Up);
                    newDirections.Add(Direction.Down);
                    break;
                default:
                    break;
            }
        }
        else if (character == '-')
        {
            switch (oldDirection)
            {
                case Direction.Up:
                    newDirections.Add(Direction.Left);
                    newDirections.Add(Direction.Right);
                    break;
                case Direction.Right:
                    newDirections.Add(Direction.Right);
                    break;
                case Direction.Down:
                    newDirections.Add(Direction.Left);
                    newDirections.Add(Direction.Right);
                    break;
                case Direction.Left:
                    newDirections.Add(Direction.Left);
                    break;
                default:
                    break;
            }
        }
        else
        {
            newDirections.Add(oldDirection);
        }

        return newDirections;
    }

    /// <summary>
    /// Find all the starting locations and their direction.
    /// </summary>
    /// <param name="endColumn"></param>
    /// <param name="endRow"></param>
    /// <returns></returns>
    private static List<(int, int, Direction)> FindAllStartingLocations(int endColumn, int endRow)
    {
        List<(int, int, Direction)> startingLocations = [];
        for (int i = 0; i <= endColumn; i++)
        {
            for (int j = 0; j <= endRow; j++)
            {
                if (i == 0 || i == 109 || j == 0 || j == 109)
                {
                    if (i == 0)
                    {
                        startingLocations.Add((i, j, Direction.Right));
                    }
                    if (i == 109)
                    {
                        startingLocations.Add((i, j, Direction.Left));
                    }
                    if (j == 0)
                    {
                        startingLocations.Add((i, j, Direction.Down));
                    }
                    if (j == 109)
                    {
                        startingLocations.Add((i, j, Direction.Up));
                    }
                }
            }
        }

        return startingLocations;
    }

    /// <summary>
    /// From a list of starting locations and directions, find the amount of most energized tiles possible.
    /// </summary>
    /// <param name="startingLocations"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    private static int FindMostTilesEnergized(List<(int, int, Direction)> startingLocations, Dictionary<(int, int), char> grid)
    {
        List<int> differentStartingPointsEnergizedTiles = [];
        foreach (var start in startingLocations)
        {
            Dictionary<(int, int, Direction), char> anotherStartingPointEnergizedTilesWithDirection = [];
            FollowBeam(start, grid, anotherStartingPointEnergizedTilesWithDirection);
            Dictionary<(int, int), char> anotherStartingPointEnergizedTiles = [];
            foreach (var tile in anotherStartingPointEnergizedTilesWithDirection)
            {
                if (!anotherStartingPointEnergizedTiles.ContainsKey((tile.Key.Item1, tile.Key.Item2)))
                {
                    anotherStartingPointEnergizedTiles.Add((tile.Key.Item1, tile.Key.Item2), tile.Value);
                }
            }
            differentStartingPointsEnergizedTiles.Add(anotherStartingPointEnergizedTiles.Count);
        }
        var maximizedEnergizedTitles = differentStartingPointsEnergizedTiles.Max();

        return differentStartingPointsEnergizedTiles.Max();
    }
}

// Part 1 --> 7623
// Part 2 --> 8244
