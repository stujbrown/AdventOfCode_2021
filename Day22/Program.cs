var instructions = File.ReadAllLines("input.txt").Select(str => (instruction: str.Split(' ')[0], coords: str.Split(' ')[1].Split(',').Select(axis => axis.Split('=')[1].Split("..").Select(x => int.Parse(x)).ToArray()).ToArray()));

Console.WriteLine($"Number of cubes (simple): {Run(false)}");
Console.WriteLine($"Number of cubes: {Run(true)}");

Int64 Run(bool useBigRamges)
{
    var ranges = new List<Range>();
    foreach (var instruction in instructions)
    {
        bool on = instruction.instruction == "on";
        var newRange = new Range
        {
            X = (min: instruction.coords[0][0], max: instruction.coords[0][1]),
            Y = (min: instruction.coords[1][0], max: instruction.coords[1][1]),
            Z = (min: instruction.coords[2][0], max: instruction.coords[2][1])
        };

        if (!useBigRamges && (newRange.X.min < -50 || newRange.X.max > 50 || newRange.Y.min < -50 || newRange.Y.max > 50 || newRange.Z.min < -50 || newRange.Z.max > 50))
            continue;

        var newRanges = new List<Range>();
        foreach (var range in ranges)
        {
            var xoredRanges = DiffRanges(range, newRange); // Poke a newRange shaped hole in ranges
            newRanges.AddRange(xoredRanges);
        }
        if (on)
        {
            newRanges.Add(newRange);
        }
        ranges = newRanges;
    }

    Int64 count = 0;
    foreach (var range in ranges)
    {
        count += Volume(range);
    }
    return count;
}

Int64 Volume(Range range) => (range.X.max + 1 - range.X.min) * (range.Y.max + 1 - range.Y.min) * (range.Z.max + 1 - range.Z.min);

List<Range> DiffRanges(Range toSplit, Range other)
{
    var newRanges = new List<Range>();

    if ((toSplit.X.min >= other.X.min) && (toSplit.Y.min >= other.Y.min) && (toSplit.Z.min >= other.Z.min) &&
                (toSplit.X.max <= other.X.max) && (toSplit.Y.max <= other.Y.max) && (toSplit.Z.max <= other.Z.max))
    {
        return newRanges;
    }

    if ((other.X.max < toSplit.X.min || other.X.min > toSplit.X.max ||
        other.Y.max < toSplit.Y.min || other.Y.min > toSplit.Y.max ||
        other.Z.max < toSplit.Z.min || other.Z.min > toSplit.Z.max))
    {
        newRanges.Add(toSplit);
        return newRanges;
    }

    if (toSplit.X.max > other.X.max)
        newRanges.Add(new Range { X = (other.X.max + 1, toSplit.X.max), Y = (toSplit.Y.min, toSplit.Y.max), Z = (toSplit.Z.min, toSplit.Z.max) });

    if (toSplit.X.min < other.X.min)
        newRanges.Add(new Range { X = (toSplit.X.min, other.X.min - 1), Y = (toSplit.Y.min, toSplit.Y.max), Z = (toSplit.Z.min, toSplit.Z.max) });

    Int64 minX = toSplit.X.min < other.X.min ? other.X.min : toSplit.X.min;
    Int64 maxX = toSplit.X.max > other.X.max ? other.X.max : toSplit.X.max;

    if (toSplit.Y.max > other.Y.max)
        newRanges.Add(new Range { X = (minX, maxX), Y = (other.Y.max + 1, toSplit.Y.max), Z = (toSplit.Z.min, toSplit.Z.max) });

    if (toSplit.Y.min < other.Y.min)
        newRanges.Add(new Range { X = (minX, maxX), Y = (toSplit.Y.min, other.Y.min - 1), Z = (toSplit.Z.min, toSplit.Z.max) });

    Int64 minY = toSplit.Y.min < other.Y.min ? other.Y.min : toSplit.Y.min;
    Int64 maxY = toSplit.Y.max > other.Y.max ? other.Y.max : toSplit.Y.max;

    if (toSplit.Z.max > other.Z.max)
        newRanges.Add(new Range { X = (minX, maxX), Y = (minY, maxY), Z = (other.Z.max + 1, toSplit.Z.max) });

    if (toSplit.Z.min < other.Z.min)
        newRanges.Add(new Range { X = (minX, maxX), Y = (minY, maxY), Z = (toSplit.Z.min, other.Z.min - 1) });

    return newRanges;
}

struct Range { public (Int64 min, Int64 max) X; public (Int64 min, Int64 max) Y; public (Int64 min, Int64 max) Z; };
