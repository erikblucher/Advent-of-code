using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();
        int lineCounter = 0;
        // List<(number, x-start, y-start, x-end, y-end)>
        List<(string, int, int, int, int)> numbers = new();
        // List<(symbol, x, y)>
        List<(string, int, int)> symbols = new();

        while (line != null)
        {
            lineCounter++;
            var charCounter = 0;
            var currentNumber = string.Empty;
            var numberStart = 0;
            var numberEnd = 0;
            foreach (char c in line)
            {
                charCounter++;
                numberEnd = charCounter - 1;
                if (IsDigit(c))
                {
                    if (currentNumber.Length == 0)
                    {
                        numberStart = charCounter;
                    }
                    currentNumber = currentNumber + c;

                    // If is end of line.
                    // Every line in input is 140 characters.
                    if (charCounter == 140)
                    {
                        numbers.Add((currentNumber, numberStart, lineCounter, numberEnd, lineCounter));
                    }
                }
                else if (IsSymbol(c))
                {
                    symbols.Add((c.ToString(), charCounter, lineCounter));
                    if (currentNumber.Length > 0)
                    {
                        numbers.Add((currentNumber, numberStart, lineCounter, numberEnd, lineCounter));
                        currentNumber = string.Empty;
                    }
                }
                else
                {
                    if (currentNumber.Length > 0)
                    {
                        numbers.Add((currentNumber, numberStart, lineCounter, numberEnd, lineCounter));
                        currentNumber = string.Empty;
                    }
                }
            }

            line = sr.ReadLine();
        }

        // Part 1
        var sumPartNumbers = numbers.Where(n => IsNextToSymbol(n, symbols)).Select(n => int.Parse(n.Item1)).Sum();
        Console.WriteLine($"Part 1 --> Sum for numbers is: {sumPartNumbers}");

        // Part 2
        var gearRatios = GearRatios(numbers, symbols);
        Console.WriteLine($"Part 2 --> Sum of gear ratios is: {gearRatios.Sum()}");
    }

    /// <summary>
    /// Is the character a digit?
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    private static bool IsDigit(char c)
    {
        Regex isDigit = new Regex("[0-9]");
        return isDigit.IsMatch(c.ToString());
    }

    /// <summary>
    /// Is the character a symbol other than a . and a digit?
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    private static bool IsSymbol(char c)
    {
        Regex isSymbol = new Regex("[^.0-9]");
        return isSymbol.IsMatch(c.ToString());
    }

    /// <summary>
    /// Look for part numbers adjacent to symbols.
    /// </summary>
    /// <param name="number"></param>
    /// <param name="symbols"></param>
    /// <returns></returns>
    private static bool IsNextToSymbol((string, int, int, int, int) number, List<(string, int, int)> symbols)
    {
        // Is any symbol adjacent the provided number; return true.
        foreach (var symbol in symbols)
        {
            if (
                symbol.Item2 >= number.Item2 - 1 &&
                symbol.Item2 <= number.Item4 + 1 &&
                symbol.Item3 >= number.Item3 - 1 &&
                symbol.Item3 <= number.Item5 + 1
                )
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Look for gear ratios and return list of all gear ratios.
    /// </summary>
    /// <param name="numbers"></param>
    /// <param name="symbols"></param>
    /// <returns></returns>
    private static List<int> GearRatios(List<(string, int, int, int, int)> numbers, List<(string, int, int)> symbols)
    {
        List<int> gearRatios = new();
        foreach (var symbol in symbols)
        {
            // Gear ratio only for symbols as *
            if (symbol.Item1 == "*")
            {
                List<string> gears = new();
                // Look for gears adjacent to the symbol and add the gear to list of gears
                foreach (var number in numbers)
                {
                    if (symbol.Item2 >= number.Item2 - 1 && symbol.Item2 <= number.Item4 + 1 && symbol.Item3 >= number.Item3 - 1 && symbol.Item3 <= number.Item5 + 1)
                    {
                        gears.Add(number.Item1);
                    }
                }

                // Gear ratio only applicable where there are two gears to a *
                if (gears.Count == 2)
                {
                    var gearRatio = int.Parse(gears[0]) * int.Parse(gears[1]);
                    gearRatios.Add(gearRatio);
                }
            }
        }

        return gearRatios;
    }
}

// Part 1 --> 540131
// Part 2 --> 86879020
