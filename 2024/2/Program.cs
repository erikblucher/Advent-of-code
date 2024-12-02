namespace _2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sr = new StreamReader("../../../input.txt");
            var line = sr.ReadLine();
            List<List<int>> reports = [];
            while (line != null)
            {
                var levels = line.Split(' ').Select(int.Parse).ToList();
                reports.Add(levels);
                line = sr.ReadLine();
            }

            // Part 1
            int safeReports = 0;
            foreach (var report in reports)
            {
                if (IsSafe(report))
                {
                    safeReports++;
                }
            }
            Console.WriteLine("Safe reports: " + safeReports);

            // Part 2
            int safeReportsWithRemovingALevel = 0;
            foreach (var report in reports)
            {
                if (IsSafeWithRemovingALevel(report))
                {
                    safeReportsWithRemovingALevel++;
                }
            }
            Console.WriteLine("Safe reports with removing a level: " + safeReportsWithRemovingALevel);
        }

        /// <summary>
        /// Check if a report is safe based on its levels.
        /// </summary>
        /// <param name="levels"></param>
        /// <returns></returns>
        private static bool IsSafe(List<int> levels)
        {
            List<int> levelDifferences = [];
            for (int i = 0; i < levels.Count; i++)
            {
                if (i > 0)
                {
                    int levelDifference = levels[i] - levels[i - 1];
                    levelDifferences.Add(levelDifference);
                }
            }

            // Check if any level increase, or decrease, is 1, 2 or 3
            if (levelDifferences.Any(d => Math.Abs(d) < 1) || levelDifferences.Any(d => Math.Abs(d) > 3))
            {
                return false;
            }

            // Check if all levels are increasing or decreasing
            if (levelDifferences.All(d => d > 0) || levelDifferences.All(d => d < 0))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Allow removal of a level if it´s unsafe and check if a report is safe based on its levels.
        /// </summary>
        /// <param name="levels"></param>
        /// <returns></returns>
        private static bool IsSafeWithRemovingALevel(List<int> levels)
        {
            bool safe = IsSafe(levels);
            // If unsafe, remove each level individually and check again if safe
            if (!safe)
            {
                for (int i = 0; i < levels.Count; i++)
                {
                    List<int> levelsCopy = [];
                    levelsCopy.AddRange(levels);
                    levelsCopy.RemoveAt(i);
                    safe = IsSafe(levelsCopy);
                    // Safe report found, then we know we can break to return a safe result
                    if (safe)
                    {
                        break;
                    }
                }
            }

            return safe;
        }
    }
}
