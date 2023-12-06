using _5;

internal class Program
{
    private static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();
        var seedNumbers = line.Split(':')[1].Trim();
        List<long> seeds = seedNumbers.Split(' ').Select(long.Parse).ToList();
        line = sr.ReadLine();
        // Placeholder for current mapping, will update through iteration
        var currentMap = new Mapping();
        // All mappings in a list for iteration
        var mappings = new List<Mapping>();
        while (line != null)
        {
            if (line != string.Empty)
            {
                var mapSpecification = line.Split(' ');
                // If destination range start, source range start and range
                if (mapSpecification.Length == 3)
                {
                    currentMap.Ranges.Add((long.Parse(mapSpecification[0]), long.Parse(mapSpecification[1]), long.Parse(mapSpecification[2])));
                }
                // Else it is a name/header for the mapping
                else
                {
                    currentMap = new Mapping { MapName = line };
                }
            }
            // If the line is empty the collection of mappings is done, add the currentMap to mappings.
            else
            {
                if (currentMap.MapName != null)
                {
                    mappings.Add(currentMap);
                }
            }
            line = sr.ReadLine();
        }
        // Do not miss the last "currentMap" after iteration
        mappings.Add(currentMap);

        // Part 1
        List<long> locations = [];
        foreach (var seed in seeds)
        {
            locations.Add(FindLocation(seed, mappings));
        }
        var shortestLocationPart1 = locations.Min();
        Console.WriteLine($"Part 1 --> Lowest location is: {shortestLocationPart1}.");

        // Part 2 || Slow! ~10 min.
        long shortestLocationPart2 = FindPart2Location(seeds, mappings);
        Console.WriteLine($"Part 2 --> Lowest location is: {shortestLocationPart2}.");
    }

    /// <summary>
    /// Find the end location for a seed based on all mappings.
    /// </summary>
    /// <param name="currentLocation"></param>
    /// <param name="mappings"></param>
    /// <returns></returns>
    private static long FindLocation(long currentLocation, List<Mapping> mappings)
    {
        foreach (var mapping in mappings)
        {
            currentLocation = NextLocation(currentLocation, mapping);
        }

        return currentLocation;
    }

    /// <summary>
    /// Find the next location from the current location and mapping.
    /// </summary>
    /// <param name="currentLocation"></param>
    /// <param name="mapping"></param>
    /// <returns></returns>
    private static long NextLocation(long currentLocation, Mapping mapping)
    {
        foreach (var range in mapping.Ranges)
        {
            if (currentLocation >= range.Item2 && currentLocation <= range.Item2 + range.Item3)
            {
                var difference = range.Item1 - range.Item2;
                currentLocation += difference;
                break;
            }
        }

        return currentLocation;
    }

    /// <summary>
    /// Slow!!!
    /// With the seeds list provided loop through all seeds and find location,
    /// return the shortest location.
    /// </summary>
    /// <param name="seeds"></param>
    /// <param name="mappings"></param>
    /// <returns></returns>
    private static long FindPart2Location(List<long> seeds, List<Mapping> mappings)
    {
        var seedCouples = new List<Tuple<long, long>>();
        // Every second (1,3,5...) seed number is the start of a range of seed numbers.
        // Every second (2,4,6...) seed number is now the size of the range of seed numbers
        // following the first seed number.
        for (int i = 0; i < seeds.Count; i += 2)
        {
            var seed = new Tuple<long, long>(seeds[i], seeds[i + 1]);
            seedCouples.Add(seed);
        }

        List<long> shortestLocationsPerSeedCouple = [];
        Parallel.ForEach(seedCouples, seedCouple =>
        {
            long shortestLocationPerSeedCouple = FindShortestLocationPerSeedCouple(seedCouple, mappings);
            shortestLocationsPerSeedCouple.Add(shortestLocationPerSeedCouple);
        });

        return shortestLocationsPerSeedCouple.Min();
    }

    /// <summary>
    /// Find the shortest location for a seed couple through all mapping.
    /// </summary>
    /// <param name="seedCouple"></param>
    /// <param name="mappings"></param>
    /// <returns></returns>
    private static long FindShortestLocationPerSeedCouple(Tuple<long, long> seedCouple, List<Mapping> mappings)
    {
        long shortestLocation = long.MaxValue;

        // Generate the seed number based on initial seed number
        // added by every number in the range.
        for (int i = 0; i < seedCouple.Item2; i++)
        {
            long seed = seedCouple.Item1 + i;
            long location = long.MaxValue;
            // Find the location of the generated seed
            location = FindLocation(seed, mappings);
            // If the location is shorter than the already shortest location, take that
            if (location < shortestLocation)
            {
                shortestLocation = location;
            }
        }

        return shortestLocation;
    }
}

// Part 1 --> 57075758
// Part 2 --> 31161857
