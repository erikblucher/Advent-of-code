using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();

        List<string> textToNumbersAndNumbers = new();
        List<string> onlyNumbers = new();
        Regex filterNumbers = new Regex("[a-zA-Z]");
        while (line != null)
        {
            onlyNumbers.Add(filterNumbers.Replace(line, ""));
            var convertedToNumbers = ConvertTextToOnlyNumbers(line);
            textToNumbersAndNumbers.Add(filterNumbers.Replace(convertedToNumbers, ""));
            line = sr.ReadLine();
        }

        // Part 1
        var sumOnlyDigits = SumNumbers(onlyNumbers);
        Console.WriteLine($"Part 1 --> Sum for first and last digits is: {sumOnlyDigits}");

        // Part 2
        var sumTextToNumbers = SumNumbers(textToNumbersAndNumbers);
        Console.WriteLine($"Part 2 --> Sum when text is converted to numbers is: {sumTextToNumbers}");
    }

    /// <summary>
    /// Convert text to numbers.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private static string ConvertTextToOnlyNumbers(string text)
    {
        // Find indexes for numbers, both in text as "one" and "1"
        // Use tuples to keep track of index where which number goes
        (int, string) indexOneFirst = (text.IndexOf("one"), "1");
        (int, string) indexOneLast = (text.LastIndexOf("one"), "1");
        (int, string) indexTwoFirst = (text.IndexOf("two"), "2");
        (int, string) indexTwoLast = (text.LastIndexOf("two"), "2");
        (int, string) indexThreeFirst = (text.IndexOf("three"), "3");
        (int, string) indexThreeLast = (text.LastIndexOf("three"), "3");
        (int, string) indexFourFirst = (text.IndexOf("four"), "4");
        (int, string) indexFourLast = (text.LastIndexOf("four"), "4");
        (int, string) indexFiveFirst = (text.IndexOf("five"), "5");
        (int, string) indexFiveLast = (text.LastIndexOf("five"), "5");
        (int, string) indexSixFirst = (text.IndexOf("six"), "6");
        (int, string) indexSixLast = (text.LastIndexOf("six"), "6");
        (int, string) indexSevenFirst = (text.IndexOf("seven"), "7");
        (int, string) indexSevenLast = (text.LastIndexOf("seven"), "7");
        (int, string) indexEightFirst = (text.IndexOf("eight"), "8");
        (int, string) indexEightLast = (text.LastIndexOf("eight"), "8");
        (int, string) indexNineFirst = (text.IndexOf("nine"), "9");
        (int, string) indexNineLast = (text.LastIndexOf("nine"), "9");
        (int, string) index1First = (text.IndexOf("1"), "1");
        (int, string) index1Last = (text.LastIndexOf("1"), "1");
        (int, string) index2First = (text.IndexOf("2"), "2");
        (int, string) index2Last = (text.LastIndexOf("2"), "2");
        (int, string) index3First = (text.IndexOf("3"), "3");
        (int, string) index3Last = (text.LastIndexOf("3"), "3");
        (int, string) index4First = (text.IndexOf("4"), "4");
        (int, string) index4Last = (text.LastIndexOf("4"), "4");
        (int, string) index5First = (text.IndexOf("5"), "5");
        (int, string) index5Last = (text.LastIndexOf("5"), "5");
        (int, string) index6First = (text.IndexOf("6"), "6");
        (int, string) index6Last = (text.LastIndexOf("6"), "6");
        (int, string) index7First = (text.IndexOf("7"), "7");
        (int, string) index7Last = (text.LastIndexOf("7"), "7");
        (int, string) index8First = (text.IndexOf("8"), "8");
        (int, string) index8Last = (text.LastIndexOf("8"), "8");
        (int, string) index9First = (text.IndexOf("9"), "9");
        (int, string) index9Last = (text.LastIndexOf("9"), "9");

        // Use SortedSet to sort by index to keep order and removes duplicates
        // if there's only 1 occurence and the first and last numbers are the same
        SortedSet<(int, string)> numbersOrderedByIndex =
        [
            indexOneFirst,
            indexOneLast,
            indexTwoFirst,
            indexTwoLast,
            indexThreeFirst,
            indexThreeLast,
            indexFourFirst,
            indexFourLast,
            indexFiveFirst,
            indexFiveLast,
            indexSixFirst,
            indexSixLast,
            indexSevenFirst,
            indexSevenLast,
            indexEightFirst,
            indexEightLast,
            indexNineFirst,
            indexNineLast,
            index1First,
            index1Last,
            index2First,
            index2Last,
            index3First,
            index3Last,
            index4First,
            index4Last,
            index5First,
            index5Last,
            index6First,
            index6Last,
            index7First,
            index7Last,
            index8First,
            index8Last,
            index9First,
            index9Last,
        ];

        // Remove indexes that are -1, i.e not found
        numbersOrderedByIndex.RemoveWhere(x => x.Item1 < 0);

        // Create string of the ordered numbers
        string converted = "";
        foreach (var number in numbersOrderedByIndex)
        {
            converted += number.Item2;
        }

        return converted;
    }

    /// <summary>
    /// Take first and last characters in each item and merge together
    /// and parse as int. Return the sum of all items.
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    private static int SumNumbers(List<string> lines)
    {
        var sum = 0;
        foreach (var number in lines)
        {
            var firstAndLastDigit = number.Substring(0, 1) + number.Substring(number.Length - 1, 1);
            sum += int.Parse(firstAndLastDigit);
        }

        return sum;
    }
}

// Part 1 --> 54450
// Part 2 --> 54265
