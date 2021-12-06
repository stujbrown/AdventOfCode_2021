var originalLifetimes = File.ReadAllLines("input.txt")[0].Split(',').Select(x => int.Parse(x));

Console.WriteLine($"Total fish after 80 days: {CountFishAfterDays(80)}");
Console.WriteLine($"Total fish after 256 days: {CountFishAfterDays(256)}");

Int64 CountFishAfterDays(int numDays)
{
    var numFishPerInterval = new Int64[9];
    foreach (var fish in originalLifetimes)
    {
        ++numFishPerInterval[fish];
    }

    int dayOffset = 0;
    for (int day = 0; day < numDays; ++day)
    {
        Int64 numFishToday = numFishPerInterval[dayOffset];
        numFishPerInterval[dayOffset] -= numFishToday;

        dayOffset = (dayOffset + 1) % numFishPerInterval.Length;
        numFishPerInterval[(8 + dayOffset) % numFishPerInterval.Length] += numFishToday;
        numFishPerInterval[(6 + dayOffset) % numFishPerInterval.Length] += numFishToday;


    }
    return numFishPerInterval.Sum();
}