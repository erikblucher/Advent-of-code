using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();
        List<int> times = [];
        List<int> distances = [];
        while (line != null)
        {
            if (line.StartsWith("Time:"))
            {
                // Add times to list
                times.AddRange(line.Split(' ').Where(IsDigit).Select(int.Parse));
            }
            else if (line.StartsWith("Distance:"))
            {
                // Add distances to list
                distances.AddRange(line.Split(' ').Where(IsDigit).Select(int.Parse));
            }
            line = sr.ReadLine();
        }
        var pairs = GetPairs(times, distances);

        // Part 1
        List<long> numberOfWaysToWinMultipleRaces = [];
        foreach (var pair in pairs)
        {
            var waysToWin = NumberDifferentOfHoldTimesToWin(pair.Item1, pair.Item2);
            numberOfWaysToWinMultipleRaces.Add(waysToWin);
        }
        long total = 1;
        foreach (var number in numberOfWaysToWinMultipleRaces)
        {
            total *= number;
        }
        Console.WriteLine($"Part 1 --> Number of ways to win power is: {total}.");

        // Part 2
        var noSpaceTimeString = string.Empty;
        var noSpaceDistanceTaveledString = string.Empty;
        foreach (var pair in pairs)
        {
            // Add the numbers together as a string
            noSpaceTimeString += pair.Item1;
            noSpaceDistanceTaveledString += pair.Item2;
        }
        long numberOfWaysToSingleRace = NumberDifferentOfHoldTimesToWin(long.Parse(noSpaceTimeString), long.Parse(noSpaceDistanceTaveledString));
        Console.WriteLine($"Part 2 --> Number of ways to win the single race: {numberOfWaysToSingleRace}.");
    }

    /// <summary>
    /// Is digit?
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private static bool IsDigit(string s)
    {
        Regex digit = new("[\\d]");
        return digit.IsMatch(s);
    }

    /// <summary>
    /// Pair item x of time with item x of distance.
    /// </summary>
    /// <param name="times"></param>
    /// <param name="distances"></param>
    /// <returns></returns>
    private static List<(int, int)> GetPairs(List<int> times, List<int> distances)
    {
        List<(int, int)> pairs = [];
        for (int i = 0; i < times.Count; i++)
        {
            (int, int) pair = (times[i], distances[i]);
            pairs.Add(pair);
        }
        return pairs;
    }

    /// <summary>
    /// Find the amount of ways to win from specified time and return
    /// the amount of ways it succeeds the distance.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    private static long NumberDifferentOfHoldTimesToWin(long time, long distance)
    {
        long numberOfHoldsToWin = 0;
        for (int i = 0; i < time; i++)
        {
            var speed = i;
            var timeTraveled = time - i;
            var distanceTraveled = speed * timeTraveled;
            if (distanceTraveled > distance)
            {
                numberOfHoldsToWin++;
            }
        }

        return numberOfHoldsToWin;        
    }
}

// Part 1 --> 440000
// Part 2 --> 26187338
