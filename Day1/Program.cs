var depths = File.ReadAllLines("input.txt").Select(str => int.Parse(str)).ToArray();

int measurementIncreases = 0;
int measurementIncreasesSliding = 0;

int lastWindowSum = depths.Take(3).Sum();
for (int i = 1; i < depths.Length; ++i)
{
    int windowSum = depths.Take(new Range(i, i + 3)).Sum();

    measurementIncreases += (depths[i] - depths[i - 1] > 0) ? 1 : 0;
    measurementIncreasesSliding += (windowSum > lastWindowSum) ? 1 : 0;
    lastWindowSum = windowSum;
}

Console.WriteLine($"{measurementIncreases} standard increases");
Console.WriteLine($"{measurementIncreasesSliding} sliding increases");