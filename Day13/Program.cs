var lines = File.ReadAllLines("input.txt").ToArray();
int div = Array.FindIndex(lines, x => string.IsNullOrWhiteSpace(x));
var coordinates = lines.Take(div).Select(line => line.Split(',').Select(x => int.Parse(x)).ToArray());
var instructions = lines.Skip(div + 1);

var page = new char[coordinates?.MaxBy(x => x[0])?[0] + 1 ?? 0, coordinates?.MaxBy(x => x[1])?[1] + 1 ?? 0];
for (int i = 0; i < page.Length; ++i)
{
    page[i / page.GetLength(1), i % page.GetLength(1)] = '.';
}

foreach (var coordinate in coordinates)
{
    page[coordinate[0], coordinate[1]] = '#';
}

int pageWidth = page.GetLength(0);
int pageLength = page.GetLength(1);


int axisIndex = "fold along ".Length;
foreach (var instruction in instructions)
{
    int foldIndex = int.Parse(instruction.Substring(axisIndex + 2));
    if (instruction[axisIndex] == 'y')
    {
        for (int copyY = foldIndex + 1, placementY = foldIndex - 1; copyY < pageLength; ++copyY, --placementY)
        {
            for (int x = 0; x < pageWidth; ++x)
            {
                page[x, placementY] = page[x, placementY] == '#' ? '#' : page[x, copyY];
            }
        }
        pageLength = (foldIndex >= pageLength / 2) ? foldIndex : pageLength - (foldIndex + 1);
    }
    else if (instruction[axisIndex] == 'x')
    {
        // mirror-flip left hand side, then copy right hand side across
        for (int y = 0; y < pageLength; ++y)
        {
            for (int x = 0, x2 = foldIndex - 1; x < x2; ++x, --x2)
            {
                var temp = page[x, y];
                page[x, y] = page[x2, y];
                page[x2, y] = temp;
            }

            for (int x = 0; x < pageWidth - (foldIndex + 1); ++x)
            {
                page[x, y] = (page[x, y] == '#' && x < foldIndex) ? '#' : page[x + foldIndex + 1, y];
            }
        }
        pageWidth = (foldIndex >= pageWidth / 2) ? foldIndex : pageWidth - (foldIndex + 1);
    }

    int count = 0;
    for (int y = 0; y < pageLength; ++y)
    {
        for (int x = 0; x < pageWidth; ++x)
        {
            count += page[x,y] == '#' ? 1 : 0;
        }
    }

    Console.WriteLine($"Visible dots after fold: {count}");
}

for (int y = 0; y < pageLength; ++y)
{
    for (int x = 0; x < pageWidth; ++x)
    {
        Console.Write(page[x, y]);

    }
    Console.WriteLine();
}