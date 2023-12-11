internal class Program
{
    private static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();
        List<string> lines = [];
        Dictionary<int, (int, int)> galaxyLocations = [];
        var oneMillion = 1000000;
        while (line != null)
        {
            lines.Add(line);
            line = sr.ReadLine();
        }
        var galaxies = Galaxies(lines);

        // Part 1
        var totalDistanceBetweenGalaxiesPart1 = TotalDistanceBetweenGalaxies(galaxies, 2);
        Console.WriteLine($"Part 1 --> Total distance between the galaxies is: {totalDistanceBetweenGalaxiesPart1}.");

        // Part 2
        var totalDistanceBetweenGalaxiesPart2 = TotalDistanceBetweenGalaxies(galaxies, oneMillion);
        Console.WriteLine($"Part 2 --> Total distance between the galaxies is: {totalDistanceBetweenGalaxiesPart2}.");
    }

    /// <summary>
    /// Get the total distance between all galaxies based on the locations and expansion.
    /// </summary>
    /// <param name="galaxies"></param>
    /// <param name="expandingPower"></param>
    /// <returns></returns>
    private static long TotalDistanceBetweenGalaxies(Dictionary<int, (int, int)> galaxies, int expandingPower)
    {
        var expandedUniverse = ExpandUniverse(galaxies, expandingPower);
        var totalDistanceBetweenGalaxies = TotalDistanceBetweenPairs(expandedUniverse);
        return totalDistanceBetweenGalaxies;
    }

    /// <summary>
    /// Expand the coordinates for each galaxy accordingly to the expanding rules and power.
    /// </summary>
    /// <param name="galaxies"></param>
    /// <param name="expandingPower"></param>
    /// <returns></returns>
    private static Dictionary<int, (int, int)> ExpandUniverse(Dictionary<int, (int, int)> galaxies, int expandingPower)
    {
        Dictionary<int, (int, int)> newUniverse = new(galaxies);

        // Columns
        // Look for columns that contains no galaxies
        var column = 0;
        List<int> columnsToInsertIndexes = [];
        while (column < galaxies.Values.Select(v => v.Item1).Max())
        {
            if (!galaxies.Values.Any(x => x.Item1 == column))
            {
                columnsToInsertIndexes.Add(column);
            }
            column++;
        }

        // Rows
        // Look for rows that contains no galaxies
        var row = 0;
        List<int> rowsToInsertIndexes = [];
        while (row < galaxies.Values.Select(v => v.Item2).Max())
        {
            if (!galaxies.Values.Any(x => x.Item2 == row))
            {
                rowsToInsertIndexes.Add(row);
            }
            row++;
        }

        // Look for galaxies to update
        // Since it is a dictionary we only need to store the key and the expanding power
        // Galaxies further and further away will occur more because they are higher than several indexes
        List<(int, int)> galaxiesToUpdateX = [];
        List<(int, int)> galaxiesToUpdateY = [];
        foreach (var columnIndex in columnsToInsertIndexes)
        {
            foreach (var galaxy in galaxies)
            {
                if (galaxy.Value.Item1 > columnIndex)
                {
                    galaxiesToUpdateX.Add((galaxy.Key, expandingPower - 1));
                }
            }
        }
        foreach (var update in galaxiesToUpdateX)
        {
            var updateGalaxyLocation = newUniverse[update.Item1];
            updateGalaxyLocation = (updateGalaxyLocation.Item1 + update.Item2, updateGalaxyLocation.Item2);
            newUniverse[update.Item1] = updateGalaxyLocation;
        }

        foreach (var rowIndex in rowsToInsertIndexes)
        {
            foreach (var galaxy in galaxies)
            {
                if (galaxy.Value.Item2 > rowIndex)
                {
                    galaxiesToUpdateY.Add((galaxy.Key, expandingPower - 1));
                }
            }
        }
        foreach (var update in galaxiesToUpdateY)
        {
            var updateGalaxyLocation = newUniverse[update.Item1];
            updateGalaxyLocation = (updateGalaxyLocation.Item1, updateGalaxyLocation.Item2 + update.Item2);
            newUniverse[update.Item1] = updateGalaxyLocation;
        }

        return newUniverse;
    }

    /// <summary>
    /// Get all the galaxies with their location in the map.
    /// </summary>
    /// <param name="galaxy"></param>
    /// <returns></returns>
    private static Dictionary<int, (int, int)> Galaxies(List<string> galaxy)
    {
        Dictionary<int, (int, int)> galaxies = [];
        var galaxyNumber = 0;
        var galaxyY = 0;
        foreach (var row in galaxy)
        {
            var galaxyX = 0;
            foreach (char c in row)
            {
                if (c == '#')
                {
                    galaxyNumber++;
                    galaxies.Add(galaxyNumber, (galaxyX, galaxyY));
                }
                galaxyX++;
            }
            galaxyY++;
        }

        return galaxies;
    }

    /// <summary>
    /// Get the total distance between all galaxies.
    /// </summary>
    /// <param name="galaxyLocations"></param>
    /// <returns></returns>
    private static long TotalDistanceBetweenPairs(Dictionary<int, (int, int)> galaxyLocations)
    {
        long totalDistance = 0;
        var distancesMeasured = 0;
        List<(string, int)> distances = [];
        for (int i = 0; i < galaxyLocations.Keys.Max(); i++)
        {
            foreach (var location in galaxyLocations.Where(l => l.Key > i))
            {
                var nextLocation = galaxyLocations[i + 1];
                distancesMeasured++;
                totalDistance += GetDistance(location.Value, nextLocation);
            }
        }

        return totalDistance;
    }

    /// <summary>
    /// Get the distance between two galaxies.
    /// </summary>
    /// <param name="galaxy1"></param>
    /// <param name="galaxy2"></param>
    /// <returns></returns>
    private static int GetDistance((int, int) galaxy1, (int, int) galaxy2)
    {
        var distanceX = galaxy1.Item1 - galaxy2.Item1;
        var distanceY = galaxy1.Item2 - galaxy2.Item2;
        // Possible negative values, convert to positive
        distanceX = distanceX < 0 ? distanceX *= -1 : distanceX;
        distanceY = distanceY < 0 ? distanceY *= -1 : distanceY;
        return distanceX + distanceY;
    }
}

// Part 1 --> 9543156
// Part 2 --> 625243292686
