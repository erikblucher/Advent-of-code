using _2;

internal class Program
{
    private static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();
        var games = new List<(int, List<Colors>)>();
        while (line != null)
        {
            var game = line.Split(':');
            int gameId = int.Parse(game[0].Split(' ')[1]);
            var rounds = new List<Colors>();
            foreach (var round in game[1].Split(';'))
            {
                var colors = new Colors();
                var currentRound = round.Split(',');
                foreach (var color in currentRound)
                {
                    var colorTrimmed = color.Trim();
                    var currentColor = colorTrimmed.Split(" ");
                    if (currentColor[1] == "red")
                    {
                        colors.Red = int.Parse(currentColor[0]);
                    }
                    else if (currentColor[1] == "green")
                    {
                        colors.Green = int.Parse(currentColor[0]);
                    }
                    else if (currentColor[1] == "blue")
                    {
                        colors.Blue = int.Parse(currentColor[0]);
                    }
                }
                rounds.Add(colors);
            }

            games.Add((gameId, rounds));
            line = sr.ReadLine();
        }

        // Part 1

        int sumOfIds = 0;
        foreach (var game in games)
        {
            if (GamePossible(game))
            {
                sumOfIds += game.Item1;
            }
        }

        Console.WriteLine($"Part 1 --> Sum of id:s for possible games is: {sumOfIds}");

        // Part 2

        int sumOfPowers = 0;
        foreach (var game in games.Select(g => g.Item2))
        {
            sumOfPowers += PowerOfMinimumSetOfCubesRequired(game);
        }

        Console.WriteLine($"Part 2 --> Sum of powers of minimum required cubes is: {sumOfPowers}.");
    }

    /// <summary>
    /// Is the game possible with a pre-defined set of cubes of each color?
    /// </summary>
    /// <param name="game"></param>
    /// <returns></returns>
    private static bool GamePossible((int, List<Colors>) game)
    {
        var possibleGame = true;
        int maxRed = 12;
        int maxGreen = 13;
        int maxBlue = 14;

        foreach (var round in game.Item2)
        {
            if (round.Red > maxRed)
            {
                possibleGame = false;
                break;
            }
            if (round.Green > maxGreen)
            {
                possibleGame = false;
                break;
            }
            if (round.Blue > maxBlue)
            {
                possibleGame = false;
                break;
            }
        }

        return possibleGame;
    }

    /// <summary>
    /// Find the minimum required amount of cubes of each color
    /// and multiply the result together to get the "power".
    /// </summary>
    /// <param name="game"></param>
    /// <returns></returns>
    private static int PowerOfMinimumSetOfCubesRequired(List<Colors> game)
    {
        int maxRed = 0;
        int maxGreen = 0;
        int maxBlue = 0;

        foreach (var round in game)
        {
            if (round.Red > maxRed)
            {
                maxRed = round.Red;
            }
            if (round.Green > maxGreen)
            {
                maxGreen = round.Green;
            }
            if (round.Blue > maxBlue)
            {
                maxBlue = round.Blue;
            }
        }

        return maxRed * maxGreen * maxBlue;
    }
}

// Part 1 --> 2879
// Part 2 --> 2879
