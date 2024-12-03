using System.Text.RegularExpressions;

namespace _3
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            var sr = new StreamReader("../../../input.txt");
            var line = sr.ReadLine();
            List<string> instructions = [];
            while (line != null)
            {
                var instruction = line.Split("mul").ToList();
                instructions.AddRange(instruction);
                line = sr.ReadLine();
            }

            // Part 1
            var part1Multiplers = GetMultiplers(instructions);
            var part1Result = Multiply(part1Multiplers).Sum();
            Console.WriteLine("Part 1 total is: " + part1Result);

            // Part 2
            var filteredInstructions = FilterInstructions(instructions);
            var part2Multiplers = GetMultiplers(filteredInstructions);
            var part2Result = Multiply(part2Multiplers).Sum();
            Console.WriteLine("Part 2 total is: " + part2Result);
        }

        /// <summary>
        /// From list of instructions, get the multipliers as list of (int, int)
        /// </summary>
        /// <param name="multipliers"></param>
        /// <returns></returns>
        private static List<(int, int)> GetMultiplers(List<string> multipliers)
        {
            var multiplersFound = new List<(int, int)>();
            Regex filter = FindNumbers();
            foreach (var multiplier in multipliers)
            {
                var match = filter.Match(multiplier);
                if (match.Success)
                {
                    // Check for end of multipler element
                    var endIndex = multiplier.IndexOf(')') - 1;
                    if (endIndex > 0)
                    {
                        // Take the numbers sperated by comma
                        var elements = multiplier.Substring(1, endIndex).Split(',');
                        if (elements.Length == 2)
                        {
                            if (int.TryParse(elements[0], out int first) && int.TryParse(elements[1], out int second))
                            {
                                multiplersFound.Add((first, second));
                            }
                        }
                    }
                }
            }

            return multiplersFound;
        }

        /// <summary>
        /// Multiply each list item and return as list of results.
        /// </summary>
        /// <param name="multipliers"></param>
        /// <returns></returns>
        private static List<int> Multiply(List<(int, int)> multipliers)
        {
            var result = new List<int>();
            foreach (var multiplier in multipliers)
            {
                var multiplied = multiplier.Item1 * multiplier.Item2;
                result.Add(multiplied);
            }

            return result;
        }

        /// <summary>
        /// Remove the list items followed by "don't()".
        /// </summary>
        /// <param name="multiplers"></param>
        /// <returns></returns>
        private static List<string> FilterInstructions(List<string> multiplers)
        {
            List<string> result = [];
            bool add = true;
            foreach (var multipler in multiplers)
            {
                if (add)
                {
                    result.Add(multipler);
                }
                int doIndex = multipler.LastIndexOf("do()");
                if (doIndex > -1)
                {
                    add = true;
                }
                int dontIndex = multipler.LastIndexOf("don't");
                if (dontIndex > -1)
                {
                    add = false;
                }
            }

            return result;
        }

        [GeneratedRegex("(\\d+,\\d+)")]
        private static partial Regex FindNumbers();
    }
}
