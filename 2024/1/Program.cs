internal class Program
{
    private static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();
        List<int> leftListLocations = [];
        List<int> rightListLocations = [];
        while (line != null)
        {
            var values = line.Split(' ').Where(l => l != string.Empty).ToList();
            leftListLocations.Add(int.Parse(values[0]));
            rightListLocations.Add(int.Parse(values[1]));
            line = sr.ReadLine();
        }
        leftListLocations = [.. leftListLocations.OrderBy(l => l)];
        rightListLocations = [.. rightListLocations.OrderBy(l => l)];

        // Part 1
        int distance = GetDistance(leftListLocations, rightListLocations);
        Console.WriteLine(distance);

        // Part 2
        int similarityScore = GetSimilarityScore(leftListLocations, rightListLocations);
        Console.WriteLine(similarityScore);
    }

    /// <summary>
    /// Get the sum of distances of ordered left and right locations.
    /// </summary>
    /// <param name="leftListLocations"></param>
    /// <param name="rightListLocations"></param>
    /// <returns></returns>
    private static int GetDistance(List<int> leftListLocations, List<int> rightListLocations)
    {
        List<int> distances = [];
        for (int i = 0; i < leftListLocations.Count; i++)
        {
            distances.Add(Math.Abs(leftListLocations[i] - rightListLocations[i]));
        }

        int distance = distances.Sum();
        return distance;
    }

    /// <summary>
    /// Get the similarity score of the left and right locations.
    /// </summary>
    /// <param name="leftListLocations"></param>
    /// <param name="rightListLocations"></param>
    /// <returns></returns>
    private static int GetSimilarityScore(List<int> leftListLocations, List<int> rightListLocations)
    {
        int score = 0;
        List<int> scores = [];
        foreach (int leftLocation in leftListLocations)
        {
            var numberOfOccurences = rightListLocations.Where(rightLocation => rightLocation == leftLocation).Count();
            scores.Add(leftLocation * numberOfOccurences);
        }

        score = scores.Sum();
        return score;
    }
}
