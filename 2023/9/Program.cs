internal class Program
{
    private static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();
        List<int> predictedEndValues = [];
        List<int> predictedStartValues = [];
        while (line != null)
        {
            (int, int) nextPreviousValues = PredictValues(line);
            // Get the next value for the sequence and save to list
            predictedEndValues.Add(nextPreviousValues.Item1);
            // Get the previous value for the sequence and save to list
            predictedStartValues.Add(nextPreviousValues.Item2);

            line = sr.ReadLine();
        }

        // Part 1
        // Sum for all next values in the sequences
        var predictedEndValuesTotal = predictedEndValues.Sum();
        Console.WriteLine($"Part 1--> Sum of the predicted next values is: {predictedEndValuesTotal}.");

        // Part 2
        // Sum for all previous values in the sequences
        var predictedStartValuesTotal = predictedStartValues.Sum();
        Console.WriteLine($"Part 2--> Sum of the predicted previous values is: {predictedStartValuesTotal}.");
    }

    /// <summary>
    /// Predict the end and previous values for a sequence.
    /// </summary>
    /// <param name="line"></param>
    /// <returns>(endNumber, startNumber)</returns>
    private static (int, int) PredictValues(string line)
    {
        var values = line.Split(' ').Select(int.Parse).ToList();
        List<List<int>> lines = [];
        // Loop through values until there are only a list of zeros
        while (values.Any(v => !v.Equals(0)))
        {
            List<int> newValues = [];
            for (int i = 0; i < values.Count(); i++)
            {
                if (i + 1 < values.Count)
                {
                    newValues.Add(values[i + 1] - values[i]);
                }
            }
            // Add the line to list for keeping
            lines.Add(values);
            // Update the values to the new values
            values = newValues;
        }

        var endNumber = PredictEndNumber(lines);
        var startNumber = PredictStartNumber(lines);
        return (endNumber, startNumber);
    }

    /// <summary>
    /// Predict the next number in the sequence
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    private static int PredictEndNumber(List<List<int>> lines)
    {
        List<int> endNumbers = [];
        for (int i = lines.Count - 1; i >= 0; i--)
        {
            List<int> line = lines[i];
            // If we are at the last line we know the difference is 0
            // and no need to add that to the last line
            if (i + 1 < lines.Count)
            {
                // Add a new last number for the line
                // The number is the sum of last number for next row and the last number in current line
                var differencePreviousRow = lines[i + 1].Last();
                line.Add(line.Last() + differencePreviousRow);
            }
        }

        // The next number is the last number for the first line
        return lines.First().Last();
    }

    /// <summary>
    /// Predict the previous number in the sequence
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    private static int PredictStartNumber(List<List<int>> lines)
    {
        List<int> startNumbers = [];
        for (int i = lines.Count - 1; i >= 0; i--)
        {
            List<int> line = lines[i];
            // If we are at the last line we know the difference is 0
            // and no need to add that to the last line
            if (i + 1 < lines.Count)
            {
                // Insert a new first number for the line
                // The number is the difference between first number for next row and the first number in current line
                var differencePreviousRow = lines[i + 1].First();
                line.Insert(0, line.First() - differencePreviousRow);
            }
        }

        // The previous number is the first number for the first line
        return lines.First().First();
    }
}

// Part 1 --> 1972648895
// Part 2 --> 919
