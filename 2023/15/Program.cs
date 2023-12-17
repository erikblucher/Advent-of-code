internal class Program
{
    private static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();
        List<string> sequence = line != null ? [.. line.Split(',')] : [];

        // Part 1
        var totalHashPower = 0;
        foreach (var code in sequence)
        {
            totalHashPower += HashPower(code);
        }
        Console.WriteLine($"Part 1 --> The sum of the hash algorithm is: {totalHashPower}.");

        // Part 2
        var totalFocalPower = FocusingPower(sequence);
        Console.WriteLine($"Part 2 --> The sum of all focusing powers is: {totalFocalPower}.");
    }

    /// <summary>
    /// Calculate the hashed power of a code.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    private static int HashPower(string code)
    {
        var currentValue = 0;
        foreach (var character in code)
        {
            currentValue += character;
            currentValue *= 17;
            currentValue %= 256;
        }

        return currentValue;
    }

    /// <summary>
    /// Sum of all the focusing powers.
    /// </summary>
    /// <param name="sequence"></param>
    /// <returns></returns>
    private static int FocusingPower(List<string> sequence)
    {
        // Use dictionary to hold boxes with a List of labels to keep order.
        Dictionary<int, List<(string, int)>> boxes = [];
        foreach (var code in sequence)
        {
            List<string> values = [];
            var addOperation = true;
            if (code.Contains('-'))
            {
                addOperation = false;
                values = [.. code.Split('-')];
            }
            else if (code.Contains('='))
            {
                values = [.. code.Split('=')];
            }

            var boxCode = values[0];
            var hasFocal = int.TryParse(values[1], out var focalLength);
            // Get the number for the box
            var boxNumber = HashPower(boxCode);
            // If the current box already exists, add or update the lens
            if (boxes.TryGetValue(boxNumber, out List<(string, int)>? value))
            {
                if (addOperation)
                {
                    var labels = value;
                    // If a lens with the same label already exists
                    if (labels.Any(c => c.Item1 == boxCode))
                    {
                        // If the label already has a focal length; let's remove
                        // the current lens and insert the new at the same place
                        if (hasFocal)
                        {
                            var currentFocalLength = labels.Single(c => c.Item1 == boxCode);
                            var index = labels.IndexOf(currentFocalLength);
                            currentFocalLength.Item2 = focalLength;
                            labels.RemoveAt(index);
                            labels.Insert(index, currentFocalLength);
                        }
                    }
                    // Add the label and the focal length to the box
                    else
                    {
                        labels.Add((boxCode, focalLength));
                    }
                }
                // Remove the lens if it is to be removed
                else
                {
                    var codes = value;
                    if (codes.Any(c => c.Item1 == boxCode))
                    {
                        var current = codes.Single(c => c.Item1 == boxCode);
                        codes.Remove(current);
                    }
                }
            }
            else
            {
                List<(string, int)> label = [];
                label.Add((boxCode, focalLength));
                boxes.Add(boxNumber, label);
            }
        }

        var totalFocalPower = 0;
        foreach (var box in boxes)
        {
            var boxNumber = box.Key;
            var labels = box.Value;
            var focals = labels.Select(l => l.Item2);
            var slot = 0;
            foreach (var focal in focals)
            {
                slot++;
                // Add the focusing power of each lense:
                // Boxnumber * Place in the list * Focal length
                totalFocalPower += (boxNumber + 1) * slot * focal;
            }
        }
        return totalFocalPower;
    }
}

// Part 1 --> 519041
// Part 2 --> 260530
