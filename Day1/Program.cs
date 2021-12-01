var depths = File.ReadAllLines("input.txt").Select(str => int.Parse(str)).ToArray();

Part1(depths);
Part2(depths);


void Part1(int[] depths)
{
    int measurementIncreases = 0;
    for (int i = 1; i < depths.Length; ++i)
    {
        if (depths[i] - depths[i - 1] > 0)
        {
            ++measurementIncreases;
        }
    }

    Console.WriteLine($"{measurementIncreases} standard increases");
}

void Part2(int[] depths)
{
    int measurementIncreases = 0;
    int lastSum = depths.Take(3).Sum();
    for (int i = 1; i < depths.Length; ++i)
    {
        int sum = depths.Take(new Range(i, i + 3)).Sum();
        int depthDiff = sum - lastSum;
        if (sum > lastSum)
        {
            ++measurementIncreases;
        }
        lastSum = sum;
    }

    Console.WriteLine($"{measurementIncreases} sliding increases");
}

