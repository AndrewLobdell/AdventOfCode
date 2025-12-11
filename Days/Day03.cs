namespace AdventOfCode2025.Days;

public sealed class Day03 : IDay
{
    public string SolvePart1(string input)
    {
        int bankSize = 2;
        List<long> joltages = ParseInputForJoltage(input, bankSize);
        return joltages.Sum().ToString();
    }

    public static List<long> ParseInputForJoltage(string input, int bankSize)
    {
        string[] banks = input.Split("\n");
        List<long> joltages = [];

        foreach (string bank in banks)
        {
            if (!string.IsNullOrWhiteSpace(bank))
            {
                var joltage = GetPeakJoltage(bank, bankSize);
                joltages.Add(long.Parse(string.Concat(joltage)));
            }
        }

        return joltages;
    }

    public static List<long> GetPeakJoltage(string bank, int bankSize = 2)
    {
        List<long> batteries = [.. bank.Select(c => (long)(c - '0'))];
        List<long> joltageList = batteries.PopRange(bankSize);

        long heldBattery;
        while (batteries.Count > 0)
        {
            heldBattery = batteries.Pop();
            for (int i = 0; i < joltageList.Count; i++)
            {
                if (joltageList[i] < heldBattery)
                {
                    (joltageList[i], heldBattery) = (heldBattery, joltageList[i]);
                }
                else if (joltageList[i] > heldBattery)
                {
                    break;
                }
            }
        }

        return joltageList;
    }


    public string SolvePart2(string input)
    {
        int bankSize = 12;
        List<long> joltages = ParseInputForJoltage(input, bankSize);
        return joltages.Sum().ToString();
    }
}

public static class ListExtensions
{
    public static T Pop<T>(this List<T> list)
    {
        var value = list[^1];
        list.RemoveAt(list.Count - 1);
        return value;
    }

    public static List<T> PopRange<T>(this List<T> list, int count)
    {
        var slice = list.GetRange(list.Count - count, count);
        list.RemoveRange(list.Count - count, count);
        return slice;
    }
}
