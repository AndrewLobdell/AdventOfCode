using System.Data;

namespace AdventOfCode2025.Days;

public sealed class Day04 : IDay
{
    public string SolvePart1(string input)
    {
        char[][] factoryMap = ParseInput(input);
        return CalculateForkliftAccessible(factoryMap).ToString();
    }

    public string SolvePart2(string input)
    {
        char[][] factoryMap = ParseInput(input);
        int totalAccessible = 0;
        int amountToAdd;
        while ((amountToAdd = CalculateForkliftAccessible(factoryMap)) > 0)
        {
            totalAccessible += amountToAdd;
        }
        return totalAccessible.ToString();
    }

    private static char[][] ParseInput(string input)
    {
        return [.. input.Split('\n')
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.ToCharArray())];
    }

    private static int CalculateForkliftAccessible(char[][] factoryMap)
    {
        int rows = factoryMap.Length, columns = factoryMap[0].Length;
        //I did it without the second list first, but honestly. It's just so much cleaner
        // than setting them to a placeholder and then converting them.
        List<(int row, int col)> accessibleRolls = [];

        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < columns; columnIndex++)
            {
                if (factoryMap[rowIndex][columnIndex] == '@')
                {
                    if (IsRollAccessible(factoryMap, rowIndex, columnIndex))
                    {
                        accessibleRolls.Add((rowIndex, columnIndex));
                    }
                }
            }
        }

        foreach (var (row, col) in accessibleRolls)
        {
            factoryMap[row][col] = '.';
        }

        return accessibleRolls.Count;
    }

    private static bool IsRollAccessible(char[][] factoryMap, int rowIndex, int columnIndex)
    {
        int surroundingRolls = 0, maximumSurroundingRolls = 4;
        int rows = factoryMap.Length;
        int columns = factoryMap[0].Length;
        List<int> rowsToCheck = IndexesToCheck(rowIndex, rows), columnsToCheck = IndexesToCheck(columnIndex, columns);
        foreach (int row in rowsToCheck)
        {
            foreach (int col in columnsToCheck)
            {
                if (row != rowIndex || col != columnIndex)
                {
                    if (factoryMap[row][col] == '@')
                    {
                        surroundingRolls++;
                    }
                }
            }
        }
        return surroundingRolls < maximumSurroundingRolls;
    }

    private static List<int> IndexesToCheck(int index, int maxIndex)
    {
        List<int> indexesToCheck = [];
        if (index > 0)
        {
            indexesToCheck.Add(index - 1);
        }
        //Placing here for sorting sanity. Don't judge me.
        indexesToCheck.Add(index);
        if (index < maxIndex - 1)
        {
            indexesToCheck.Add(index + 1);
        }
        return indexesToCheck;
    }
}
