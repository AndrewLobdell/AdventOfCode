namespace AdventOfCode2025.Days;

public sealed class Day01 : IDay
{
    public string SolvePart1(string input)
    {
        int zeroCount = SolvePassword(input, RotateCountOnZero);
        return zeroCount.ToString();
    }

    public string SolvePart2(string input)
    {
        int zeroCount = SolvePassword(input, RotateCountPassZero);
        return zeroCount.ToString();
    }

    private static int SolvePassword(string input, Action<DialState> rotateFunc)
    {
        var state = new DialState { Dial = 50, ZeroCount = 0 };
        int i = 0;

        while (i < input.Length)
        {
            while (i < input.Length && !char.IsLetterOrDigit(input[i]))
                i++;

            if (i >= input.Length) break;

            char direction = char.ToUpper(input[i++]);
            if (direction != 'L' && direction != 'R') continue;

            while (i < input.Length && !char.IsDigit(input[i]))
            {
                if (char.IsLetter(input[i])) break;
                i++;
            }

            int number = 0;
            while (i < input.Length && char.IsDigit(input[i]))
                number = number * 10 + (input[i++] - '0');

            state.Direction = direction;
            state.Number = number;
            rotateFunc(state);
        }

        return state.ZeroCount;
    }

    private void RotateCountOnZero(DialState state)
    {
        state.Dial += state.Direction == 'L' ? -state.Number : state.Number;
        state.Dial %= 100;
        if (state.Dial < 0) state.Dial += 100;
        if (state.Dial == 0) state.ZeroCount++;
    }

    private void RotateCountPassZero(DialState state)
    {
        int start = state.Dial;
        int clicks = state.Number;
        int step = state.Direction == 'L' ? -1 : 1;

        int firstHit = step > 0
            ? (start == 0 ? 100 : 100 - start % 100)
            : (start == 0 ? 100 : start % 100);

        if (firstHit <= clicks)
            state.ZeroCount += (clicks - firstHit) / 100 + 1;

        state.Dial = ((start + clicks * step) % 100 + 100) % 100;
    }

    private class DialState
    {
        public int Dial { get; set; }
        public int ZeroCount { get; set; }
        public char Direction { get; set; }
        public int Number { get; set; }
    }
}
