using AdventOfCode2025.Days;

namespace AdventOfCode2025.Tests;

public class Day02Tests
{
    private static string ReadTestData(string fileName)
    {
        // Starting from bin/Debug/netX/, go up to the repo root.
        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        var path = Path.Combine(root, "AdventOfCode2025.Tests", "TestData", "Day02", fileName);
        return File.ReadAllText(path);
    }

    [Fact()]
    public void Part1_Example()
    {
        var input = ReadTestData("part1.example.input.txt");
        var expected = ReadTestData("part1.example.expected.txt");
        var day = new Day02();

        var result = day.SolvePart1(input);

        Assert.Equal(expected, result);
    }

    [Fact()]
    public void Part2_Example()
    {
        var input = ReadTestData("part2.example.input.txt");
        var expected = ReadTestData("part2.example.expected.txt");
        var day = new Day02();

        var result = day.SolvePart2(input);

        Assert.Equal(expected, result);
    }

    [Fact()]
    public void ParsesCorrectRange()
    {
        var rangeStr = "100-200";
        var range = Day02.Range.Parse(rangeStr);

        Assert.Equal("100", range.Start);
        Assert.Equal("200", range.End);
    }

    [Theory]
    [MemberData(nameof(InvalidIdTestData))]
    public void IdentifiesInvalidIdsInRange(string rangeStr, List<long> expectedInvalidIds, List<long> expectedInvalidIdsPart2)
    {
        foreach (var part in new[] { 1, 2 })
        {
            var range = Day02.Range.Parse(rangeStr);
            range.GetInvalidInRange(part: part);
            var expected = part == 1 ? expectedInvalidIds : expectedInvalidIdsPart2;
            Assert.Equal(expected, range.InvalidIds);
        }
    }
    public static TheoryData<string, List<long>, List<long>> InvalidIdTestData => new()
    {
        { "11-22", new List<long> { 11, 22}, new List<long> {11, 22} },
        { "95-115", new List<long> {99} , new List<long> {99, 111} },
        { "998-1012", new List<long> {1010}, new List<long> {999, 1010} },
        { "1188511880-1188511890", new List<long> {1188511885}, new List<long> {1188511885} },
        { "222220-222224", new List<long> {222222}, new List<long> {222222} },
        { "1698522-1698528", new List<long> {} , new List<long> {} },
        { "446443-446449", new List<long> {446446}, new List<long> {446446} },
        { "38593856-38593862", new List<long> {38593859}, new List<long> {38593859} },
        {"565653-565659", new List<long> {}, new List<long> {565656} },
        {"824824821-824824827", new List<long> {}, new List<long> {824824824} },
        {"2121212118-2121212124", new List<long> {}, new List<long> {2121212121} },
    };
}