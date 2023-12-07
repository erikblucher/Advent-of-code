using _7;

internal class Program
{
    private static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();
        List<Hand> hands = [];
        while (line != null)
        {
            var values = line.Split(' ');
            List<char> cards = [];
            foreach (char card in values[0])
            {
                cards.Add(card);
            }
            var hand = new Hand
            {
                Cards = cards,
                Bid = int.Parse(values[1])
            };
            hands.Add(hand);
            line = sr.ReadLine();
        }

        // Part 1
        // Order by the strength of the hand and the weight
        hands = hands.OrderBy(h => h.Strength).ThenBy(h => h.Weight).ToList();
        var rank = 0;
        // Set the rank
        foreach (var hand in hands)
        {
            rank++;
            hand.Rank = rank;
        }
        int totalWinnings = TotalWinnings(hands);
        Console.WriteLine($"Part 1 --> Total winnings: {totalWinnings}.");

        // Part 2
        // Order by the strength when jacks are jokers
        var jokerHands = hands.OrderBy(h => h.JokerStrength).ThenBy(h => h.JokerWeight).ToList();
        int jokerRank = 0;
        // Set the joker rank
        foreach (var hand in jokerHands)
        {
            jokerRank++;
            hand.JokerRank = jokerRank;
        }
        int jokerTotalWinnings = TotalWinnings(hands, true);
        Console.WriteLine($"Part 2 --> Total winnings with jokers: {jokerTotalWinnings}.");
    }

    /// <summary>
    /// Calculate the total winnings by iterate through all bids multiplying
    /// with their corresponding rank and take the sum of all.
    /// Specify if use jacks as jokers or not.
    /// </summary>
    /// <param name="orderedHands"></param>
    /// <param name="useJoker"></param>
    /// <returns></returns>
    private static int TotalWinnings(List<Hand> orderedHands, bool useJoker = false)
    {
        var total = 0;
        foreach (var hand in orderedHands)
        {
            if (useJoker)
            {
                total += hand.Bid * hand.JokerRank;
            }
            else
            {
                total += hand.Bid * hand.Rank;
            }
        }

        return total;
    }
}

// Part 1 --> 250254244
// Part 2 --> 250087440.
