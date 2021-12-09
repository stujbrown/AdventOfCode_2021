var grid = File.ReadAllLines("input.txt").Select(str => str.ToCharArray().Select(c => Convert.ToInt32(c) - '0').ToArray()).ToArray();

int totalRiskLevel = 0;
var basinSizes = new List<int>();
for (int y = 0; y < grid.Length; ++y)
{
    for (int x = 0; x < grid[y].Length; ++x)
    {
        int numHigherRatedNeighbours = 0;
        foreach (var neighbourCoordinate in GetNeighbours(x, y))
        {
            numHigherRatedNeighbours += (grid[y][x] >= grid[neighbourCoordinate.y][neighbourCoordinate.x]) ? 1 : 0;
        }

        if (numHigherRatedNeighbours == 0)
        {
            totalRiskLevel += 1 + grid[y][x];
            basinSizes.Add(CountBasin((x, y), new HashSet<(int x, int y)>()));
        }
    }
}
basinSizes.Sort();

Console.WriteLine($"Total risk level for low points: {totalRiskLevel}");
Console.WriteLine($"Largest basin factor: {basinSizes[basinSizes.Count - 1] * basinSizes[basinSizes.Count - 2] * basinSizes[basinSizes.Count - 3]}");


int CountBasin((int x, int y) coordinate, HashSet<(int x, int y)> visited)
{
    visited.Add(coordinate);
    foreach (var neighbourCoordinate in GetNeighbours(coordinate.x, coordinate.y))
    {
        if (grid[neighbourCoordinate.y][neighbourCoordinate.x] != 9 && grid[coordinate.y][coordinate.x] < grid[neighbourCoordinate.y][neighbourCoordinate.x])
        {
            if (!visited.Contains(neighbourCoordinate))
            {
                CountBasin(neighbourCoordinate, visited);
            }
        }
    }

    return visited.Count;
}

IEnumerable<(int x, int y)> GetNeighbours(int x, int y)
{
    (int x, int y)[] directions = new (int x, int y)[4] { (1, 0), (0, -1), (-1, 0), (0, 1) };
    for (int directionIndex = 0; directionIndex < directions.Length; ++directionIndex)
    {
        var offsetCoordinate = (x: x + directions[directionIndex].x, y: y + directions[directionIndex].y);
        if (offsetCoordinate.y < grid.Length && offsetCoordinate.y >= 0 && offsetCoordinate.x < grid[offsetCoordinate.y].Length && offsetCoordinate.x >= 0)
        {
            yield return offsetCoordinate;
        }
    }
}