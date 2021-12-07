var crabPositions = File.ReadAllLines("input.txt")[0].Split(',').Select(x => int.Parse(x)).ToList();
crabPositions.Sort();

Int64[] exponentialCosts = new Int64[crabPositions.Last() + 1];
for (int i = 1; i < exponentialCosts.Length; ++i)
{
    exponentialCosts[i] = exponentialCosts[i - 1] + i;
}

int halfIndex = crabPositions.Count / 2;
int median = (crabPositions.Count % 2) == 0 ? (crabPositions[halfIndex] + crabPositions[halfIndex - 1]) / 2 : crabPositions[halfIndex];

var average = crabPositions.Average();
int averageRounded = crabPositions.Count % 2 == 0 ? (int)average : (int)Math.Ceiling(average);

(Int64 linear, Int64 exponential) totals = (0, 0);
foreach (var value in crabPositions)
{
    totals.linear += Math.Abs(median - value);
    totals.exponential += exponentialCosts[Math.Abs(averageRounded - value)];
}

Console.WriteLine($"Linear fuel cost: {totals.linear}");
Console.WriteLine($"Exponential fuel cost: {totals.exponential}");