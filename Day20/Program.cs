var file = File.ReadLines("input.txt");
var encoding = file.First().ToCharArray();
var inputImage = file.Skip(2).Select(str => str.ToCharArray()).ToArray();

var clusterOffsets = new (int X, int Y)[9] { (-1, -1), (0, -1), (1, -1), (-1, 0), (0, 0), (1, 0), (-1, 1), (0, 1), (1, 1) };

Console.WriteLine($"Total pixels lit after 2 iterations: {Solve(2)}");
Console.WriteLine($"Total pixels lit after 50 iterations: {Solve(50)}");

int Solve(int numIterations)
{
    var input = new Dictionary<(int X, int Y), char>();
    for (int y = 0; y < inputImage.Length; ++y)
    {
        for (int x = 0; x < inputImage[0].Length; ++x)
        {
            input.Add((x, y), inputImage[y][x]);
        }
    }

    bool keyTreats0AsFlip = encoding[0] == '#';
    bool isBorderInfinitelyLit = false;

    int minX = 0, minY = 0, maxX = inputImage[0].Length, maxY = inputImage.Length;

    Dictionary<(int X, int Y), char> output = null;
    for (int iteration = 0; iteration < numIterations; ++iteration)
    {
        output = new Dictionary<(int X, int Y), char>();

        for (int y = minY - 1; y < maxY + 2; ++y)
        {
            for (int x = minX - 1; x < maxX + 2; ++x)
            {
                var value = Decode(input, x, y, isBorderInfinitelyLit);
                output.Add((x, y), value);
            }
        }

        if (keyTreats0AsFlip)
        {
            isBorderInfinitelyLit = !isBorderInfinitelyLit;
        }
        input = output;
        minX -= 1;
        minY -= 1;
        maxX += 1;
        maxY += 1;
    }
    return output.Values.Where(x => x == '#').Count();
}

char Decode(Dictionary<(int X, int Y), char> image, int x, int y, bool isBorderInfinitelyLit)
{
    uint value = 0;
    for (int i = 0; i < clusterOffsets.Length; ++i)
    {
        int thisX = x + clusterOffsets[i].X;
        int thisY = y + clusterOffsets[i].Y;
        bool containsEntry = image.TryGetValue((thisX, thisY), out char pixel);
        if ((containsEntry && pixel == '#') || (!containsEntry && isBorderInfinitelyLit))
        {
            value |= 1u << (8 - i);
        }
    }
    return encoding[value];
}