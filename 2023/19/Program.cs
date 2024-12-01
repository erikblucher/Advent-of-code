using System.Text.RegularExpressions;

internal partial class Program
{
    private static void Main(string[] args)
    {
        var sr = new StreamReader("../../../input.txt");
        var line = sr.ReadLine();
        Dictionary<string, List<string>> workflows = [];
        List<Dictionary<string, string>> ratings = [];
        var isRating = false;
        while (line != null)
        {
            if (line.Length == 0)
            {
                isRating = !isRating;
                line = sr.ReadLine();
                continue;
            }
            // Create rating
            if (isRating)
            {
                var values = line.Remove(line.Length - 1, 1).Remove(0, 1).Split(',').ToList();
                Dictionary<string, string> dict = [];
                foreach (var value in values)
                {
                    var keyValue = value.Split('=');
                    dict.Add(keyValue[0], keyValue[1]);
                }
                ratings.Add(dict);
            }
            // Create workflow
            else
            {
                var workflow = line;
                var key = line.Split('{')[0];
                var value = line.Split('{')[1].Remove(line.Split('{')[1].IndexOf('}')).Split(',').ToList();
                workflows.Add(key, value);
            }

            line = sr.ReadLine();
        }

        // Part 1
        var acceptedPartsTotal = 0;
        foreach (var xmas in ratings)
        {
            if (IsAccepted(xmas, workflows))
            {
                var sum = xmas.Select(x => int.Parse(x.Value)).Sum();
                Console.WriteLine(sum);
                acceptedPartsTotal += sum;
            }
        }
        Console.WriteLine($"Part 1 --> The total sum for the parts is: {acceptedPartsTotal}.");

        // Part 2
    }

    /// <summary>
    /// If we end on the letter 'A' we have an accepted rating.
    /// </summary>
    /// <param name="xmas"></param>
    /// <param name="workflows"></param>
    /// <returns></returns>
    private static bool IsAccepted(Dictionary<string, string> xmas, Dictionary<string, List<string>> workflows)
    {
        string end = FindEnd(xmas, workflows, "in");
        return end == "A";
    }

    /// <summary>
    /// Go through the workflows until we hit 'A' or 'R'.
    /// </summary>
    /// <param name="xmas"></param>
    /// <param name="workflows"></param>
    /// <param name="current"></param>
    /// <returns></returns>
    private static string FindEnd(Dictionary<string, string> xmas, Dictionary<string, List<string>> workflows, string current)
    {
        Console.WriteLine(current);
        var end = string.Empty;
        var workflow = workflows[current];
        List<List<string>> conditions = GetConditions(workflow);
        var searching = true;
        foreach (var condition in conditions)
        {
            if (searching && condition.Count == 1)
            {
                if (condition[0] == "A" || condition[0] == "R")
                {
                    end = condition[0];
                    return end;
                }
                return FindEnd(xmas, workflows, condition[0]);
            }
            if (searching && CheckCondition(condition, xmas))
            {
                if (condition[3] == "A" || condition[3] == "R")
                {
                    end = condition[3];
                    searching = !searching;
                }
                else
                {
                    return FindEnd(xmas, workflows, condition[3]);
                }
            }
        }

        return end;
    }

    /// <summary>
    /// List all the conditions.
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    private static List<List<string>> GetConditions(List<string> workflow)
    {
        List<List<string>> conditions = [];
        foreach (string parts in workflow)
        {
            List<string> condition = [];
            // If only letters we are at the last condition
            // where we hit if every condition prior this is false
            if (OnlyLetters(parts))
            {
                condition.Add(parts);
            }
            else
            {
                // Key: x, m, a or s
                condition.Add(parts[0].ToString());
                // Operator: < or >
                condition.Add(parts[1].ToString());
                // Next workflow if condition is true
                condition.Add(parts[2..].Split(':')[0]);
                // Next workflow if condition is false
                condition.Add(parts[2..].Split(':')[1]);
            }
            conditions.Add(condition);
        }

        return conditions;
    }

    /// <summary>
    /// Evaluate condition.
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="xmas"></param>
    /// <returns></returns>
    private static bool CheckCondition(List<string> condition, Dictionary<string, string> xmas)
    {
        var xmasValue = xmas[condition[0]];
        if (condition[1] == ">")
        {
            return int.Parse(xmasValue) > int.Parse(condition[2]);
        }
        return int.Parse(xmasValue) < int.Parse(condition[2]);
    }

    private static bool OnlyLetters(string input)
    {
        return !OnlyLetters().Match(input).Success;
    }

    [GeneratedRegex(@"[^\sa-zA-Z]")]
    private static partial Regex OnlyLetters();
}

// Part 1 --> 480738
// Part 2 --> 
