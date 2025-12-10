# Advent of Code 2025 - C# Solution Template

A scaffolding tool for Advent of Code challenges with automatic puzzle downloading, test setup, and streamlined workflow.

## Prerequisites

- .NET SDK (compatible with the project)
- `curl` (for downloading puzzle inputs)
- `w3m` (optional, for automatic puzzle description extraction)
- Bash shell

## Initial Setup

### 1. Get Your Session Cookie

To automatically download puzzle inputs and descriptions, you need your Advent of Code session cookie:

1. Log in to [Advent of Code](https://adventofcode.com)
2. Open your browser's developer tools (F12)
3. Go to the **Application** or **Storage** tab
4. Under **Cookies**, find `adventofcode.com`
5. Copy the value of the `session` cookie

### 2. Set the Environment Variable

Add the session cookie to your environment:

```bash
# Add to your ~/.bashrc, ~/.zshrc, or similar
export AOC_SESSION="your_session_cookie_here"
```

Or set it temporarily in your current shell:

```bash
export AOC_SESSION="your_session_cookie_here"
```

**Important:** Keep your session cookie private! Don't commit it to version control.

## Creating a New Day

### Using new-day.sh

Run the scaffold script from the repository root:

```bash
./new-day.sh
```

This will:
1. Prompt you for a day number (1-25)
2. Create the following structure:
   ```
   Days/DayXX.cs                                    # Your solution code
   Inputs/DayXX/input.txt                           # Real puzzle input
   AdventOfCode2025.Tests/DayXXTests.cs             # Test file
   AdventOfCode2025.Tests/TestData/DayXX/           # Test data files
       part1.example.input.txt
       part1.example.expected.txt
       part2.example.input.txt
       part2.example.expected.txt
   Notes/DayXX/part1.txt                            # Puzzle description
   ```
3. If `AOC_SESSION` is set:
   - Automatically download the real puzzle input to `Inputs/DayXX/input.txt`
   - Download Part 1 puzzle description to `Notes/DayXX/part1.txt`
4. Update [Program.cs](Program.cs) to register the new day

### What Gets Created

**Day Class** ([Days/DayXX.cs](Days/))
```csharp
public sealed class DayXX : IDay
{
    public string SolvePart1(string input) { ... }
    public string SolvePart2(string input) { ... }
}
```

**Test Class** ([AdventOfCode2025.Tests/DayXXTests.cs](AdventOfCode2025.Tests/))
- Pre-configured with example tests for both parts
- Tests are initially skipped until you add example data

## Working with Test Data

### Setting Up Example Tests

After running `new-day.sh`, populate the test data files:

1. **Add example input** from the puzzle description:
   ```bash
   # Copy the example input from the puzzle page
   echo "example data here" > AdventOfCode2025.Tests/TestData/DayXX/part1.example.input.txt
   ```

2. **Add expected output** for the example:
   ```bash
   # The expected answer for the example
   echo "42" > AdventOfCode2025.Tests/TestData/DayXX/part1.example.expected.txt
   ```

3. **Enable the test** by removing the `Skip` attribute from [AdventOfCode2025.Tests/DayXXTests.cs](AdventOfCode2025.Tests/)

### Running Tests

Run all tests:
```bash
dotnet test
```

Run tests for a specific day:
```bash
dotnet test --filter "FullyQualifiedName~Day01Tests"
```

Run a specific test:
```bash
dotnet test --filter "FullyQualifiedName~Day01Tests.Part1_Example"
```

## Running Solutions

The main program accepts the following arguments:

```bash
dotnet run <day> <part> [example]
```

### Parameters

- `day`: Day number (1-25)
- `part`: Part number (1 or 2)
- `example` (optional): Use example input instead of real puzzle input

### Examples

Run Part 1 with real puzzle input:
```bash
dotnet run 1 1
```

Run Part 2 with real puzzle input:
```bash
dotnet run 1 2
```

Test with example input:
```bash
dotnet run 1 1 example
```

### Input File Resolution

The program looks for input files in this order:

1. With `example` flag: `Inputs/DayXX/example.txt`
2. Without flag: `Inputs/DayXX/input.txt`

**Note:** For testing with examples, you can create `Inputs/DayXX/example.txt` separately from the test data files.

## Refreshing Puzzle Notes (After Solving)

### Using refresh-notes.sh

After completing Part 1 and/or Part 2, use this script to:
- Submit your answers (optional)
- Download the complete puzzle description with Part 2 unlocked

```bash
./refresh-notes.sh
```

This will:
1. Prompt for the day number
2. Optionally submit your Part 1 answer
3. Optionally submit your Part 2 answer
4. Download the full puzzle description to `Notes/DayXX/full.txt`
5. Extract Part 2-specific content to `Notes/DayXX/part2.txt`

**Requirements:**
- Must have `AOC_SESSION` environment variable set
- Must have `w3m` installed

### Manual Submission

You can also submit answers through the Advent of Code website instead of using the script.

## Project Structure

```
AdventOfCode2025/
├── Days/                              # Solution implementations
│   ├── IDay.cs                       # Interface all days implement
│   ├── DayTemplate.cs                # Template for new days
│   └── DayXX.cs                      # Individual day solutions
├── Inputs/                            # Puzzle inputs
│   └── DayXX/
│       ├── input.txt                 # Real puzzle input
│       └── example.txt               # Optional example input
├── AdventOfCode2025.Tests/           # Test project
│   ├── TestData/
│   │   └── DayXX/
│   │       ├── part1.example.input.txt
│   │       ├── part1.example.expected.txt
│   │       ├── part2.example.input.txt
│   │       └── part2.example.expected.txt
│   ├── DayTemplateTests.cs           # Template for new test files
│   └── DayXXTests.cs                 # Individual day tests
├── Notes/                             # Puzzle descriptions
│   └── DayXX/
│       ├── part1.txt                 # Part 1 description
│       ├── part2.txt                 # Part 2 only
│       └── full.txt                  # Complete description
├── Program.cs                         # Main entry point
├── new-day.sh                         # Scaffold new day
└── refresh-notes.sh                   # Update notes after solving
```

## Typical Workflow

1. **Start a new day:**
   ```bash
   ./new-day.sh
   # Enter day number when prompted
   ```

2. **Read the puzzle description:**
   ```bash
   cat Notes/DayXX/part1.txt
   ```

3. **Add example test data:**
   - Copy example from puzzle into `AdventOfCode2025.Tests/TestData/DayXX/part1.example.input.txt`
   - Add expected answer to `AdventOfCode2025.Tests/TestData/DayXX/part1.example.expected.txt`
   - Remove `Skip` attribute from test in [AdventOfCode2025.Tests/DayXXTests.cs](AdventOfCode2025.Tests/)

4. **Implement solution:**
   - Edit [Days/DayXX.cs](Days/)
   - Run tests: `dotnet test --filter "FullyQualifiedName~DayXXTests"`

5. **Test with example:**
   ```bash
   dotnet run XX 1 example
   ```

6. **Run with real input:**
   ```bash
   dotnet run XX 1
   ```

7. **Submit and unlock Part 2:**
   ```bash
   ./refresh-notes.sh
   # Follow prompts to submit answer
   ```

8. **Repeat for Part 2:**
   - Read `Notes/DayXX/part2.txt`
   - Add Part 2 example test data
   - Implement `SolvePart2`
   - Test and run

## Tips

- **Keep your session cookie secure:** Add `.env` or similar files to `.gitignore`
- **Example inputs:** The script creates empty example files; fill them from the puzzle description
- **Manual input:** If automatic download fails, paste input when prompted by `new-day.sh`
- **Test-driven development:** Enable and pass example tests before running against real input
- **Notes organization:** The `Notes/` directory preserves puzzle descriptions for offline reference

## Troubleshooting

**Input download fails:**
- Verify `AOC_SESSION` is set correctly
- Check that the puzzle has been released (midnight EST)
- Manually download from the website if needed

**Tests not found:**
- Ensure you're running from the repository root
- Check that test data files exist in the correct location

**w3m not found:**
- Install w3m: `brew install w3m` (macOS) or `apt-get install w3m` (Linux)
- Or manually copy puzzle descriptions to `Notes/DayXX/`

## License

This is a personal template for Advent of Code challenges. Feel free to use and modify as needed.
