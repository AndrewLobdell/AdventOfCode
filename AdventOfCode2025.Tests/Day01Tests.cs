using AdventOfCode2025.Days;

namespace AdventOfCode2025.Tests;

public class Day01Tests
{
    private static string ReadTestData(string fileName)
    {
        // Starting from bin/Debug/netX/, go up to the repo root.
        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        var path = Path.Combine(root, "AdventOfCode2025.Tests", "TestData", "Day01", fileName);
        return File.ReadAllText(path);
    }

    [Fact()]
    public void Part1_Example()
    {
        var input = ReadTestData("part1.example.input.txt");
        var expected = ReadTestData("part1.example.expected.txt");
        var day = new Day01();

        var result = day.SolvePart1(input);

        Assert.Equal(expected, result);
    }

    [Fact()]
    public void Part2_Example()
    {
        var input = ReadTestData("part2.example.input.txt");
        var expected = ReadTestData("part2.example.expected.txt");
        var day = new Day01();

        var result = day.SolvePart2(input);

        Assert.Equal(expected, result);
    }
}
