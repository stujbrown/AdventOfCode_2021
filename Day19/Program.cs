var splitScans = string.Join(Environment.NewLine, File.ReadAllLines("input.txt")).Split(Environment.NewLine + Environment.NewLine).Select(scan =>
    scan.Split(Environment.NewLine).Skip(1).Select(coord => coord.Split(',').Select(x => int.Parse(x)).ToArray()));
var scans = splitScans.Select(x => x.Select(coord => new Coord { X = coord[0], Y = coord[1], Z = coord[2] }).ToArray()).ToArray();

var map = scans[0].ToList(); // Start from 0 to avoid having to do peicemeal coordinate resolution .
var remaining = scans.Skip(1).ToList();

while (remaining.Count > 0)
{
    foreach (var scan in remaining)
    {
        if(RunForScan(scan))
        {
            remaining.Remove(scan);
            break;
        }
    }
}


Console.WriteLine($"Total beacons: {map.Count}");

bool RunForScan(Coord[] scan)
{
    for (int i = 0; i < 24; ++i) // Check each rotation.
    {
        foreach (var coord in map) // Check each possible offset translation
        {
            foreach (var other in scan)
            {
                Coord transformedOther = Swizzle(other, i);
                var originOffset = new Coord { X = coord.X - transformedOther.X, Y = coord.Y - transformedOther.Y, Z = coord.Z - transformedOther.Z };
                if (Match(map, scan, originOffset, i) >= 12)
                {
                    foreach (var toAdd in scan)
                    {
                        Coord transformedToAdd = Swizzle(toAdd, i);
                        var translatedToAdd = new Coord { X = transformedToAdd.X + originOffset.X, Y = transformedToAdd.Y + originOffset.Y, Z = transformedToAdd.Z + originOffset.Z };
                        if (!map.Contains(translatedToAdd))
                        {
                            map.Add(translatedToAdd);
                        }
                    }

                    return true;
                }
            }
        }
    }
    return false;
}

int Match(List<Coord> lhs, Coord[] rhs, Coord originOffset, int rotationIndex)
{
    int numMatches = 0;
    foreach (var coord in lhs)
    {
        foreach (var other in rhs)
        {
            Coord transformed = Swizzle(other, rotationIndex);
            var translatedOther = new Coord { X = transformed.X + originOffset.X, Y = transformed.Y + originOffset.Y, Z = transformed.Z + originOffset.Z };
            if (coord.X == translatedOther.X && coord.Y == translatedOther.Y && coord.Z == translatedOther.Z)
            {
                ++numMatches;
                break;
            }
        }
    }
    return numMatches;
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