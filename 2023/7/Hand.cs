namespace _7
{
    public class Hand
    {
        private List<char> _cards = [];
        private int _weight;
        private int _jokerWeight;

        /// <summary>
        /// The cards.
        /// Using the cards we calculate the strength and the weight of the hand.
        /// </summary>
        public List<char> Cards
        {
            get
            {
                return _cards;
            }
            set
            {
                _cards = value;

                // Find which unique cards there are
                var uniqueCards = new HashSet<char>();
                foreach (var card in Cards)
                {
                    uniqueCards.Add(card);
                }

                // Find the number of cards of each card present
                List<(char, int)> occurencesOfCards = [];
                foreach (var card in uniqueCards)
                {
                    (char, int) cardCount = (card, Cards.Where(c => c == card).Count());
                    occurencesOfCards.Add(cardCount);
                }

                // Set the strength of the hand
                int handStrength = 0;
                int jokerHandStrength = 0;
                // Count jokers
                int jokers = occurencesOfCards.Where(c => c.Item1 == 'J').FirstOrDefault().Item2;
                foreach (var occurence in occurencesOfCards)
                {
                    // Get the hand strength and set if highest
                    var strength = HandStrength(occurencesOfCards);
                    if (strength > handStrength)
                    {
                        handStrength = strength;
                    }

                    // Get the hand strength for jokers and set if highest
                    var jokerStrength = HandStrength(occurencesOfCards, jokers);
                    if (jokerStrength > jokerHandStrength)
                    {
                        jokerHandStrength = jokerStrength;
                    }
                }
                Strength = handStrength;
                JokerStrength = jokerHandStrength;

                // Set the weight of the cards
                var weightDigit = 5;
                for (int i = 0; i < Cards.Count; i++)
                {
                    var card = CardWeight(Cards[i]);
                    var jokerCard = CardWeight(Cards[i], true);
                    // Count down weight digit to go down from most significant to least significant
                    weightDigit--;
                    _weight += card * int.Parse(Math.Pow(16, weightDigit).ToString());
                    _jokerWeight += jokerCard * int.Parse(Math.Pow(16, weightDigit).ToString());
                }
                Weight = _weight;
                JokerWeight = _jokerWeight;
            }
        }

        /// <summary>
        /// The bid for the hand.
        /// </summary>
        public int Bid { get; set; }

        /// <summary>
        /// The strength of the hand based on five of the same card, four of the same,
        /// full house, three of the same, two pairs or one pair.
        /// </summary>
        public int Strength { get; set; }

        /// <summary>
        /// The same as Strength but with Jokers instead of Jacks.
        /// </summary>
        public int JokerStrength { get; set; }

        /// <summary>
        /// The weight is a number to determine the order of cards.
        /// Counting the first card as the most significant one and then down one by one.
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// The same as Weight but Jacks are Jokers and the least valuable card.
        /// </summary>
        public int JokerWeight { get; set; }

        /// <summary>
        /// The rank based on Strength and then Weight.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// The rank but is based on JokerStrength and JokerWeight.
        /// </summary>
        public int JokerRank { get; set; }

        /// <summary>
        /// Find the total strength of the hand based on number of jokers.
        /// </summary>
        /// <param name="occurencesOfCards"></param>
        /// <param name="jokers"></param>
        /// <returns></returns>
        private static int HandStrength(List<(char, int)> occurencesOfCards, int jokers = 0)
        {
            List<(char, int)> occurencesCopy = new(occurencesOfCards.OrderBy(c => c.Item2).Reverse());
            if (jokers > 0)
            {
                var bestMatchIndex = 0;
                for (int i = 0; i < occurencesCopy.Count; i++)
                {
                    if (occurencesCopy[i].Item1 != 'J')
                    {
                        bestMatchIndex = i;
                        break;
                    }
                }
                if (occurencesCopy.Any(o => o.Item1 != 'J'))
                {
                    var bestMatch = occurencesCopy.Where(o => o.Item1 != 'J').ToList()[0];
                    for (int i = 0; i < jokers; i++)
                    {
                        bestMatch.Item2++;
                    }
                    occurencesCopy[bestMatchIndex] = bestMatch;
                    var jokerCard = occurencesCopy.Single(c => c.Item1 == 'J');
                    jokerCard.Item2 = 0;
                    var jokerIndex = 0;
                    for (int i = 0; i < occurencesCopy.Count; i++)
                    {
                        if (occurencesCopy[i].Item1 == 'J')
                        {
                            jokerIndex = i;
                        }
                    }
                    occurencesCopy[jokerIndex] = jokerCard;
                }
                
            }
            var strength = 0;
            // Five of a kind
            if (occurencesCopy.Any(o => o.Item2 == 5))
            {
                strength = 6;
            }
            // Four of a kind
            else if (occurencesCopy.Any(o => o.Item2 == 4))
            {
                strength = 5;
            }
            // Full house
            else if (occurencesCopy.Any(o => o.Item2 == 3) && occurencesCopy.Any(o => o.Item2 == 2))
            {
                strength = 4;
            }
            // Three of a kind
            else if (occurencesCopy.Any(o => o.Item2 == 3))
            {
                strength = 3;
            }
            // Two pairs
            else if (occurencesCopy.Where(o => o.Item2 == 2).Count() > 1)
            {
                strength = 2;
            }
            // One pair
            else if (occurencesCopy.Any(o => o.Item2 == 2))
            {
                strength = 1;
            }

            return strength;
        }

        /// <summary>
        /// Return the weight of the card.
        /// </summary>
        /// <param name="card"></param>
        /// <param name="useJoker"></param>
        /// <returns></returns>
        private static int CardWeight(char card, bool useJoker = false)
        {
            int cardWeight = 0;
            switch (card)
            {
                case '2':
                    cardWeight = int.Parse(card.ToString());
                    break;
                case '3':
                    cardWeight = int.Parse(card.ToString());
                    break;
                case '4':
                    cardWeight = int.Parse(card.ToString());
                    break;
                case '5':
                    cardWeight = int.Parse(card.ToString());
                    break;
                case '6':
                    cardWeight = int.Parse(card.ToString());
                    break;
                case '7':
                    cardWeight = int.Parse(card.ToString());
                    break;
                case '8':
                    cardWeight = int.Parse(card.ToString());
                    break;
                case '9':
                    cardWeight = int.Parse(card.ToString());
                    break;
                case 'T':
                    cardWeight = 10;
                    break;
                case 'J':
                    // If treated as joker the J-card has the lowest value
                    if (useJoker)
                    {
                        cardWeight = 1;
                    }
                    else
                    {
                        cardWeight = 11;
                    }
                    break;
                case 'Q':
                    cardWeight = 12;
                    break;
                case 'K':
                    cardWeight = 13;
                    break;
                case 'A':
                    cardWeight = 14;
                    break;
                default:
                    break;

            }

            return cardWeight;
        }
    }
}
