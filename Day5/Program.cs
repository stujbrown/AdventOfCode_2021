
var ventLines = File.ReadAllLines("input.txt").Select(str =>
    {
        var startAndEnd = str.Split(" -> ");
        var start = startAndEnd[0].Split(',').Select(x => int.Parse(x)).ToArray();
        var end = startAndEnd[1].Split(',').Select(x => int.Parse(x)).ToArray();
        return (start: (x: start[0], y: start[1]), end: (x: end[0], y: end[1]));
    });

Console.WriteLine($"Number of overlaps without diagonals: {RunFill(false)}");
Console.WriteLine($"Number of overlaps with diagonals: {RunFill(true)}");

int RunFill(bool useDiagonals)
{
    var map = new int[1000, 1000];
    int overlapCounter = 0;
    foreach (var ventLine in ventLines)
    {
        (int x, int y) step = (Math.Clamp(ventLine.end.x - ventLine.start.x, -1, 1), Math.Clamp(ventLine.end.y - ventLine.start.y, -1, 1));
        if (useDiagonals || (step.x == 0 || step.y == 0))
        {
            var current = (ventLine.start.x, ventLine.start.y);
            var end = (ventLine.end.x + step.x, ventLine.end.y + step.y);
            while (current != end)
            {
                if (++map[current.x, current.y] == 2)
                {
                    ++overlapCounter;
                }
                current = (current.x + step.x, current.y + step.y);
            }
        }
    }
    return overlapCounter;
}