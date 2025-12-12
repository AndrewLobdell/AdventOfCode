using AdventOfCode2025.Days;

namespace AdventOfCode2025.Tests;

public class Day05Tests
{
    private static string ReadTestData(string fileName)
    {
        // Starting from bin/Debug/netX/, go up to the repo root.
        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        var path = Path.Combine(root, "AdventOfCode2025.Tests", "TestData", "Day05", fileName);
        return File.ReadAllText(path);
    }

    [Fact(Skip = "Fill example input/expected files under AdventOfCode2025.Tests/TestData/DayXX before enabling this test.")]
    public void Part1_Example()
    {
        var input = ReadTestData("part1.example.input.txt");
        var expected = ReadTestData("part1.example.expected.txt");
        var day = new Day05();

        var result = day.SolvePart1(input);

        Assert.Equal(expected, result);
    }

    [Fact(Skip = "Fill example input/expected files under AdventOfCode2025.Tests/TestData/DayXX before enabling this test.")]
    public void Part2_Example()
    {
        var input = ReadTestData("part2.example.input.txt");
        var expected = ReadTestData("part2.example.expected.txt");
        var day = new Day05();

        var result = day.SolvePart2(input);

        Assert.Equal(expected, result);
    }
}
