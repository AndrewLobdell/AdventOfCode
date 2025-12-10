namespace AdventOfCode2025.Days;

public sealed class Day02 : IDay
{
    public string SolvePart1(string input)
    {
        long invalidSum = 0;
        string[] ranges = input.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var rangeStr in ranges)
        {
            var range = Range.Parse(rangeStr);
            range.GetInvalidInRange();
            invalidSum += range.SumInvalid();
        }

        return invalidSum.ToString();
    }

    public string SolvePart2(string input)
    {
        long invalidSum = 0;
        string[] ranges = input.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var rangeStr in ranges)
        {
            var range = Range.Parse(rangeStr);
            range.GetInvalidInRange(part: 2);
            invalidSum += range.SumInvalid();
        }

        return invalidSum.ToString();
    }

    public class Range
    {
        public required string Start { get; set; }
        public required string End { get; set; }
        public List<long> InvalidIds = [];

        public static Range Parse(string rangeStr)
        {
            var parts = rangeStr.Split('-', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                throw new FormatException($"Invalid range format: {rangeStr}");

            return new Range
            {
                Start = parts[0],
                End = parts[1]
            };
        }

        public void GetInvalidInRange(int part = 1)
        {
            for (long i = long.Parse(Start); i <= long.Parse(End); i++)
            {
                if (IsInvalid(i, part))
                {
                    InvalidIds.Add(i);
                }
            }
        }

        public long SumInvalid()
        {
            return InvalidIds.Sum();
        }

        private static bool IsInvalid(long id, int part)
        {
            var s = id.ToString();

            if (part == 1 && s.Length % 2 != 0) return false;
            int groupSize = part == 1 ? s.Length / 2 : 1;

            while (groupSize * 2 <= s.Length)
            {
                if (s.Length % groupSize != 0)
                {
                    groupSize++;
                    continue;
                }
                HashSet<string> set = SplitToSet(s, groupSize);
                if (set.Count == 1)
                {
                    return true;
                }
                groupSize++;
            }
            return false;
        }
    }

    private static HashSet<string> SplitToSet(string input, int size)
    {
        HashSet<string> set = [];
        for (int i = 0; i + size <= input.Length; i += size)
        {
            set.Add(input.Substring(i, size));
        }
        return set;
    }
}
