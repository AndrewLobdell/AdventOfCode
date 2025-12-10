using AdventOfCode2025.Days;

namespace AdventOfCode2025;

public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: dotnet run <day> <part> [example]");
            Console.WriteLine("  day:    1-25");
            Console.WriteLine("  part:   1 | 2");
            Console.WriteLine("  example: optional; if provided, uses example input instead of puzzle input");
            return 1;
        }

        if (!int.TryParse(args[0], out var day) || day is < 1 or > 25)
        {
            Console.WriteLine("Day must be an integer between 1 and 25.");
            return 1;
        }

        var part = args[1] switch
        {
            "1" => 1,
            "2" => 2,
            _ => 0
        };

        if (part == 0)
        {
            Console.WriteLine("Part must be 1 or 2.");
            return 1;
        }

        var useExample = args.Length >= 3 &&
                         args[2].Equals("example", StringComparison.OrdinalIgnoreCase);

        var inputPath = Path.Combine("Inputs", $"Day{day:00}", useExample ? "example.txt" : "input.txt");
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return 1;
        }

        var input = File.ReadAllText(inputPath);
        IDay solver = day switch
        {
            1 => new Day01(),
            2 => new Day02(),
            // NEW_DAY_MARKER
            _ => throw new NotImplementedException($"Day {day:00} has not been implemented yet."),
        };

        var result = part == 1
            ? solver.SolvePart1(input)
            : solver.SolvePart2(input);

        Console.WriteLine(result);
        return 0;
    }
}
