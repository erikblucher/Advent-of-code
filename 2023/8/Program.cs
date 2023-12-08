internal class Program
{
    private static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();
        // The first line is instructions on Left or Right
        var instructions = line ?? string.Empty;
        line = sr.ReadLine();
        List<(string, string, string)> maps = [];
        while (line != null)
        {
            if (string.IsNullOrEmpty(line))
            {
                line = sr.ReadLine();
                continue;
            }
            (string, string, string) map;
            // Just take the nodes for every line
            map.Item1 = line[..3];
            map.Item2 = line.Substring(7, 3);
            map.Item3 = line.Substring(12, 3);
            maps.Add(map);
            line = sr.ReadLine();
        }

        // Part 1
        // Find the starting map
        var currentMap = maps.Single(m => m.Item1 == "AAA");
        var stepsToFindZZZ = 0;
        while (currentMap.Item1 != "ZZZ")
        {
            foreach (var instruction in instructions)
            {
                stepsToFindZZZ++;
                var nextLocation = NextLocation(currentMap, instruction);
                currentMap = maps.Single(m => m.Item1 == nextLocation);
            }
        }
        Console.WriteLine($"Part 1 --> Steps to find ZZZ is: {stepsToFindZZZ}.");

        // Part 2
        // Find the starting maps; the nodes ending with 'A'
        var currentMaps = maps.Where(m => m.Item1.EndsWith('A')).ToList();
        List<int> allStepsToFindEndZ = [];
        // Look for the number of steps to find location ending with 'Z'
        // Loop through all maps to find the steps required for every map
        for (int i = 0; i < currentMaps.Count; i++)
        {
            var currentMapByIndex = currentMaps[i];
            var stepsToFindEndZ = 0;
            while (!currentMapByIndex.Item1.EndsWith('Z'))
            {
                foreach (var instruction in instructions)
                {
                    stepsToFindEndZ++;
                    var nextLocation = NextLocation(currentMapByIndex, instruction);
                    currentMapByIndex = maps.Single(m => m.Item1 == nextLocation);
                }
            }
            allStepsToFindEndZ.Add(stepsToFindEndZ);
        }
        // The solution where all ends with 'Z' at the same time is the least common multiple
        var lcm = FindLcm(allStepsToFindEndZ);
        Console.Write($"Part 2 --> Steps to find Z at the end simultaneously: {lcm}");
    }

    /// <summary>
    /// Determine based on instruction L or R which route to take.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="instruction"></param>
    /// <returns></returns>
    private static string NextLocation((string, string, string) map, char instruction)
    {
        if (instruction == 'L')
        {
            return map.Item2;
        }
        return map.Item3;
    }

    /// <summary>
    /// Find the least common multiple for list of numbers.
    /// Stolen from: https://www.geeksforgeeks.org/lcm-of-given-array-elements/. Thank you!
    /// </summary>
    /// <param name="numbers"></param>
    /// <returns></returns>
    private static long FindLcm(List<int> numbers)
    {
        long lcm = 1;
        int divisor = 2;

        while (true)
        {
            int counter = 0;
            bool divisible = false;
            for (int i = 0; i < numbers.Count; i++)
            {
                if (numbers[i] == 0)
                {
                    return 0;
                }
                else if (numbers[i] < 0)
                {
                    numbers[i] = numbers[i] * (-1);
                }
                if (numbers[i] == 1)
                {
                    counter++;
                }

                if (numbers[i] % divisor == 0)
                {
                    divisible = true;
                    numbers[i] = numbers[i] / divisor;
                }
            }

            if (divisible)
            {
                lcm *= divisor;
            }
            else
            {
                divisor++;
            }

            if (counter == numbers.Count)
            {
                return lcm;
            }
        }
    }
}

// Part 1 --> 20093
// Part 2 --> 22103062509257
