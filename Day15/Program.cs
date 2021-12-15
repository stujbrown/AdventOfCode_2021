var risks = File.ReadAllLines("input.txt").Select(row => row.ToCharArray().Select(x => x - '0').ToArray()).ToArray();

var expandedRisks = new int[risks.Length * 5][];
for (int y = 0; y < expandedRisks.Length; ++y)
{
    expandedRisks[y] = new int[risks[0].Length * 5];
    for (int x = 0; x < expandedRisks[0].Length; ++x)
    {
        int repeatIndex = (x / risks.Length) + (y / risks.Length);
        int newRisk = (risks[y % risks.Length][x % risks[0].Length] + repeatIndex);
        expandedRisks[y][x] = ((newRisk - 1) % 9) + 1;
    }
}

Console.WriteLine($"Total risk for original: {Solve(risks)}");
Console.WriteLine($"Total risk for expanded cave: {Solve(expandedRisks)}");

int Solve(int[][] riskGrid)
{
    var neighbourOffsets = new (int x, int y)[] { (1, 0), (0, 1), (-1, 0), (0, -1) };

    int MakeIndex(int x, int y) => riskGrid[0].Length * x + y;

    var distances = new int[riskGrid.Length * riskGrid[0].Length];
    var previous = new (int x, int y)[riskGrid.Length * riskGrid[0].Length];
    Array.Fill(distances, int.MaxValue);
    distances[MakeIndex(0, 0)] = 0;

    var queue = new PriorityQueue<(int x, int y), int>();
    queue.Enqueue((0, 0), 0);

    while (queue.Count > 0)
    {
        var top = queue.Dequeue();
        foreach (var offset in neighbourOffsets)
        {
            var neighbour = (x: top.x + offset.x, y: top.y + offset.y);
            if (neighbour.x >= 0 && neighbour.x < riskGrid[0].Length && neighbour.y >= 0 && neighbour.y < riskGrid.Length)
            {
                var distanceTo = riskGrid[neighbour.y][neighbour.x];
                var accumulativeDistance = distanceTo + distances[MakeIndex(top.x, top.y)];
                if (distances[MakeIndex(neighbour.x, neighbour.y)] > accumulativeDistance)
                {
                    distances[MakeIndex(neighbour.x, neighbour.y)] = accumulativeDistance;
                    previous[MakeIndex(neighbour.x, neighbour.y)] = top;
                    queue.Enqueue(neighbour, accumulativeDistance);
                }
            }
        }
    }

    int totalRisk = 0;
    var current = (x: riskGrid[0].Length - 1, y: riskGrid.Length - 1);
    while (current != (0, 0))
    {
        totalRisk += riskGrid[current.y][current.x];
        current = previous[MakeIndex(current.x, current.y)];
    }
    return totalRisk;
}