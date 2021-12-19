var lines = File.ReadAllLines("input.txt").Select(str => str.ToCharArray()).ToArray();

var allSnailfishNumbers = new List<(int value, int depth)[]>();
foreach (var line in lines)
{
    var newLine = new List<(int value, int depth)>();
    int depth = 0;
    int digit = 0;
    bool buildingDigit = false;

    void AddDigitIfBuilt()
    {
        if (buildingDigit)
        {
            newLine.Add((value: digit, depth: depth));
            buildingDigit = false;
        }
    };

    foreach (var c in line)
    {
        if (c == '[')
        {
            AddDigitIfBuilt();
            ++depth;
        }
        else if (c == ']')
        {
            AddDigitIfBuilt();
            --depth;
        }
        else if (c == ',')
        {
            AddDigitIfBuilt();
            digit = 0;
        }
        else
        {
            digit = (digit * 10) + (c - '0');
            buildingDigit = true;
        }
    }
    allSnailfishNumbers.Add(newLine.ToArray());
}

Console.WriteLine($"Final sum: {SumNumbers(allSnailfishNumbers)}");
int largestSum = 0;

for (int i = 0; i < allSnailfishNumbers.Count; ++i)
{
    for (int j = 0; j < allSnailfishNumbers.Count; ++j)
    {
        if (i != j)
        {
            int sum = SumNumbers(new List<(int value, int depth)[]> { allSnailfishNumbers[i], allSnailfishNumbers[j] });
            if (sum > largestSum)
            {
                largestSum = sum;
            }
        }
    }
}
Console.WriteLine($"Largest possible sum: {largestSum}");


int SumNumbers(List<(int value, int depth)[]> numbers)
{
    void IncValue(List<(int value, int depth)> list, int index, int incValue)
    {
        var oldVal = list[index];
        list[index] = (value: oldVal.value + incValue, depth: oldVal.depth);
    }

    var merged = new List<(int value, int depth)>(numbers[0]);
    for (int lineIndex = 1; lineIndex < numbers.Count; ++lineIndex)
    {
        merged.AddRange(numbers[lineIndex]);
        for (int i = 0; i < merged.Count; ++i)
        {
            merged[i] = (value: merged[i].value, depth: merged[i].depth + 1);
        }

        bool didAThing = true;
        while (didAThing)
        {
            didAThing = false;
            for (int searchIndex = 0; searchIndex < merged.Count; ++searchIndex)
            {
                var thisDepth = merged[searchIndex].depth;
                if (thisDepth > 4)
                {
                    // Is this a leaf? This can only hit on first-members so if the next number is on the same level it must be a flat pair.
                    if (merged[searchIndex + 1].depth == thisDepth)
                    {

                        didAThing = true;
                        if (searchIndex > 0)
                        {
                            IncValue(merged, searchIndex - 1, merged[searchIndex].value);
                        }
                        if (searchIndex < merged.Count - 2)
                        {
                            IncValue(merged, searchIndex + 2, merged[searchIndex + 1].value);
                        }
                        merged[searchIndex] = (value: 0, depth: merged[searchIndex].depth - 1);
                        merged.RemoveAt(searchIndex + 1);
                        break;
                    }
                }
            }

            if (!didAThing)
            {
                for (int searchIndex = 0; searchIndex < merged.Count; ++searchIndex)
                {
                    if (merged[searchIndex].value >= 10)
                    {
                        didAThing = true;
                        var val = merged[searchIndex];
                        merged[searchIndex] = (value: val.value / 2, depth: val.depth + 1);
                        merged.Insert(searchIndex + 1, (value: (int)((val.value / 2f) + 0.5f), depth: val.depth + 1));
                        break;
                    }
                }
            }
        }
    }

    int index = 0;
    int depth = 0;
    int ResolvePairs()
    {
        ++depth;
        int left = 0;
        if (merged[index].depth != depth)
        {
            left = ResolvePairs();
        }
        else
        {
            left = merged[index++].value;
        }

        int right = 0;
        if (merged[index].depth > depth)
        {
            right = ResolvePairs();
        }
        else
        {
            right = merged[index++].value;
        }

        --depth;
        return (3 * left) + (2 * right);

    }

    int result = ResolvePairs();
    return result;
}


