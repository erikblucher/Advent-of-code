internal class Program
{
    private static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();
        // List<(myNymbers, winningNumbers, occurences-of-card)>
        List<(List<int>, List<int>, int)> cards = new();
        List<double> gamePoints = new();
        while (line != null)
        {
            var game = line.Split(':');
            var numbers = game[1].Trim().Split('|');
            var winningNumbers = numbers[0].Trim().Split(' ').Where(n => n != string.Empty).Select(int.Parse).ToList();
            var myNumbers = numbers[1].Trim().Split(' ').Where(n => n != string.Empty).Select(int.Parse).ToList();
            cards.Add((myNumbers, winningNumbers, 1));
            line = sr.ReadLine();
        }

        // Part 1
        foreach (var card in cards)
        {
            var matchingNumbers = FindMatchingNumbers(card.Item1, card.Item2);
            if (matchingNumbers > 0)
            {
                gamePoints.Add(Math.Pow(2, matchingNumbers - 1));
            }
        }
        var totalPoints = gamePoints.Sum();
        Console.WriteLine($"Part 1 --> The sum for the winning cards is: {totalPoints}.");

        // Part 2
        SetOccurences(cards);
        var totalCards = cards.Select(c => c.Item3).Sum();
        Console.WriteLine($"Part 2 --> The amount of cards at the end of the game is: {totalCards}.");
    }

    /// <summary>
    /// Find number of matching numbers from my numbers and winning numbers.
    /// </summary>
    /// <param name="numbers"></param>
    /// <param name="winningNumbers"></param>
    /// <returns></returns>
    private static int FindMatchingNumbers(List<int> numbers, List<int> winningNumbers)
    {
        int matchingNumbers = 0;
        foreach (var number in winningNumbers)
        {
            if (numbers.Contains(number))
            {
                matchingNumbers++;
            };
        }

        return matchingNumbers;
    }

    /// <summary>
    /// Set occurences for cards based on number of matching numbers.
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    private static List<(List<int>, List<int>, int)> SetOccurences(List<(List<int>, List<int>, int)> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            var matchingNumbers = FindMatchingNumbers(card.Item1, card.Item2);
            for(int j = 0;j < matchingNumbers; j++)
            {
                // Take the following card in the list for every matching number
                var manipulateOccurencesCard = cards[i + j + 1];
                // The number of occurences the card already has
                var occurences = cards[i].Item3;
                manipulateOccurencesCard.Item3 += occurences;
                // Update the card with the new manipulated card with updated occurences
                cards[i + j + 1] = manipulateOccurencesCard;
            }
        }

        return cards;
    }
}

// Part 1 --> 24175
// Part 2 --> 18846301
