var splitScans = string.Join(Environment.NewLine, File.ReadAllLines("input.txt")).Split(Environment.NewLine + Environment.NewLine).Select(scan =>
    scan.Split(Environment.NewLine).Skip(1).Select(coord => coord.Split(',').Select(x => int.Parse(x)).ToArray()));
var scans = splitScans.Select(x => x.Select(coord => new Coord { X = coord[0], Y = coord[1], Z = coord[2] }).ToArray()).ToArray();

var map = scans[0].ToList();
var scanners = new List<Coord>();
scanners.Add(new Coord());
var remaining = scans.Skip(1).ToList();

while (remaining.Count > 0)
{
    foreach (var scan in remaining)
    {
        if (RunForScan(scan))
        {
            remaining.Remove(scan);
            break;
        }
    }
}

int largestDistance = 0;
for (int i = 0; i < scanners.Count; ++i)
{
    for (int j = 0; j < scanners.Count; ++j)
    {
        if (i != j)
        {
            var diff = new Coord { X = scanners[i].X - scanners[j].X, Y = scanners[i].Y - scanners[j].Y, Z = scanners[i].Z - scanners[j].Z };
            var distance = Math.Abs(diff.X) + Math.Abs(diff.Y) + Math.Abs(diff.Z);
            if (distance > largestDistance)
            {
                largestDistance = distance;
            }
        }
    }
}

Console.WriteLine($"Total beacons: {map.Count}");
Console.WriteLine($"Largest distance: {largestDistance}");

bool RunForScan(Coord[] scan)
{
    for (int i = 0; i < 24; ++i) // Check each rotation.
    {
        var match = Match(map, scan, i);
        if (match != null)
        {
            foreach (var toAdd in scan)
            {
                Coord transformedToAdd = Swizzle(toAdd, i);
                var translatedToAdd = new Coord { X = transformedToAdd.X + match.Value.X, Y = transformedToAdd.Y + match.Value.Y, Z = transformedToAdd.Z + match.Value.Z };

                if (!map.Contains(translatedToAdd))
                {
                    map.Add(translatedToAdd);
                }
            }

            scanners.Add(match.Value);
            return true;
        }
    }
    return false;
}

Coord? Match(List<Coord> lhs, Coord[] rhs, int rotationIndex)
{
    var diffCounts = new Dictionary<Coord, int>();
    foreach (var coord in lhs)
    {
        foreach (var other in rhs)
        {
            Coord transformed = Swizzle(other, rotationIndex);

            var diff = new Coord { X = coord.X - transformed.X, Y = coord.Y - transformed.Y, Z = coord.Z - transformed.Z };
            if (!diffCounts.TryGetValue(diff, out int count))
            {
                diffCounts.Add(diff, 1);
            }
            else
            {
                var newCount = count + 1;
                diffCounts[diff] = newCount;
                if (newCount >= 12)
                {
                    return diff;
                }
            }
        }
    }
    return null;
}

Coord Swizzle(Coord coord, int swizzleIndex)
{
    switch (swizzleIndex)
    {
        case 0: return new Coord { X = coord.X, Y = coord.Y, Z = coord.Z };
        case 1: return new Coord { X = coord.X, Y = coord.Z, Z = -coord.Y };
        case 2: return new Coord { X = coord.X, Y = -coord.Y, Z = -coord.Z };
        case 3: return new Coord { X = coord.X, Y = -coord.Z, Z = coord.Y };
        case 4: return new Coord { X = -coord.X, Y = coord.Y, Z = -coord.Z };
        case 5: return new Coord { X = -coord.X, Y = coord.Z, Z = coord.Y };
        case 6: return new Coord { X = -coord.X, Y = -coord.Y, Z = coord.Z };
        case 7: return new Coord { X = -coord.X, Y = -coord.Z, Z = -coord.Y };
        case 8: return new Coord { X = coord.Z, Y = coord.Y, Z = -coord.X };
        case 9: return new Coord { X = coord.Z, Y = coord.X, Z = coord.Y };
        case 10: return new Coord { X = coord.Z, Y = -coord.X, Z = -coord.Y };
        case 11: return new Coord { X = coord.Z, Y = -coord.Y, Z = coord.X };
        case 12: return new Coord { X = -coord.Z, Y = coord.Y, Z = coord.X };
        case 13: return new Coord { X = -coord.Z, Y = -coord.X, Z = coord.Y };
        case 14: return new Coord { X = -coord.Z, Y = -coord.Y, Z = -coord.X };
        case 15: return new Coord { X = -coord.Z, Y = coord.X, Z = -coord.Y };
        case 16: return new Coord { X = coord.Y, Y = -coord.X, Z = coord.Z };
        case 17: return new Coord { X = coord.Y, Y = coord.Z, Z = coord.X };
        case 18: return new Coord { X = coord.Y, Y = -coord.Z, Z = -coord.X };
        case 19: return new Coord { X = coord.Y, Y = coord.X, Z = -coord.Z };
        case 20: return new Coord { X = -coord.Y, Y = coord.X, Z = coord.Z };
        case 21: return new Coord { X = -coord.Y, Y = -coord.Z, Z = coord.X };
        case 22: return new Coord { X = -coord.Y, Y = -coord.X, Z = -coord.Z };
        case 23: return new Coord { X = -coord.Y, Y = coord.Z, Z = -coord.X };
    }
    throw new Exception();
}
struct Coord { public int X; public int Y; public int Z; };