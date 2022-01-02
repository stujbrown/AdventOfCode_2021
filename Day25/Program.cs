var lines = File.ReadAllLines("input.txt");
var map = new char[lines[0].Length, lines.Length];
var updatedMap = new char[map.GetLength(0), map.GetLength(1)];

for (int y = 0; y < lines.Length; ++y)
    for (int x = 0; x < lines[0].Length; ++x)
        map[x, y] = lines[y][x];

int numIterations = 0;
bool shouldContinue = true;
while (shouldContinue)
{
    shouldContinue = Move('>');
    shouldContinue |= Move('v');
    ++numIterations;
}


Console.WriteLine($"Num iterations: {numIterations}");

bool Move(char direction)
{
    bool didUpdate = false;
    Array.Copy(map, updatedMap, map.Length);

    for (int y = 0; y < map.GetLength(1); ++y)
    {
        for (int x = 0; x < map.GetLength(0); ++x)
        {
            if (map[x, y] == direction)
            {
                int nextX = (direction == '>') ? (x + 1 >= map.GetLength(0) ? 0 : x + 1) : x;
                int nextY = (direction == 'v') ? (y + 1 >= map.GetLength(1) ? 0 : y + 1) : y;

                if (map[nextX, nextY] == '.')
                {
                    updatedMap[nextX, nextY] = map[x, y];
                    updatedMap[x, y] = '.';
                    didUpdate = true;
                }
            }
        }
    }

    var temp = map;
    map = updatedMap;
    updatedMap = temp;

    return didUpdate;
}