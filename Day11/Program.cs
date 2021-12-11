var rows = File.ReadAllLines("input.txt").Select(str => str.ToCharArray().Select(c => c - '0').ToArray()).ToArray();
var hasFlashed = new bool[10, 10];
var flashList = new List<(int x, int y)>();
var neighbourOffsets = new (int x, int y)[] { (-1, 0), (-1, -1), (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1) };

bool hasSynchronised = false;
int totalFlashed = 0;
int step = 0;
do
{
    Array.Clear(hasFlashed);
    flashList.Clear();
    for (int y = 0; y < 10; ++y)
    {
        for (int x = 0; x < 10; ++x)
        {
            Increment(x, y);
        }
    }

    totalFlashed += flashList.Count;
    foreach (var flashed in flashList)
    {
        rows[flashed.y][flashed.x] = 0;
    }

    if (step == 99)
    {
        Console.WriteLine($"Total flashes: {totalFlashed}");
    }

    var compVal = rows[0][0];
    hasSynchronised = rows.All(row => row.All(x => x == compVal));
    ++step;

} while (!hasSynchronised);

Console.WriteLine($"Synchronised on step {step}");

void Increment(int x, int y)
{
    if (++rows[y][x] > 9)
    {
        if (!hasFlashed[x, y])
        {
            hasFlashed[x, y] = true;
            flashList.Add((x, y));

            foreach (var neighourOffset in neighbourOffsets)
            {
                var newX = x + neighourOffset.x;
                var newY = y + neighourOffset.y;
                if (newX >= 0 && newX < 10 && newY >= 0 && newY < 10)
                {
                    Increment(newX, newY);
                }
            }
        }
    }
}