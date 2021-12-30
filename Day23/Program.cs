var initialState = File.ReadAllLines("input.txt").Select(str => str.ToCharArray()).ToArray();
var costs = new uint[4] { 1, 10, 100, 1000 };
var roomExitSpots = new int[4] { 2, 4, 6, 8 };

Console.WriteLine($"Lowest cost: {Solve()}");


Console.WriteLine();

UInt64 Solve()
{
    var hallway = new char[11];
    var rooms = new (char val, bool moved)[4, 2];
    for (int i = 0; i < 4; ++i)
    {
        rooms[i, 0] = (val: initialState[2][3 + (i * 2)], moved: false);
        rooms[i, 1] = (val: initialState[3][3 + (i * 2)], moved: false);
    }
    var state = new State { Hallway = hallway, Rooms = rooms };

    UInt64 lowestCost = UInt64.MaxValue;

    var queue = new PriorityQueue<State, UInt64>();
    queue.Enqueue(state, 0);

    while (queue.Count > 0)
    {
        var top = queue.Dequeue();
        if (top.Cost > lowestCost)
        {
            break;
        }
        //PushState();

        void MoveToHallway(int room, uint depth)
        {
            int exitIndex = roomExitSpots[room];
            for (int targetSpace = 0; targetSpace < hallway.Length; ++targetSpace)
            {
                if (!roomExitSpots.Contains(targetSpace))
                {
                    bool isRouteClear = true;
                    int space = exitIndex;
                    int inc = targetSpace < exitIndex ? -1 : 1;
                    while (space != targetSpace)
                    {
                        space += inc;
                        if (top.Hallway[space] != default)
                        {
                            isRouteClear = false;
                            break;
                        }
                    }

                    if (isRouteClear)
                    {
                        var newState = new State();
                        newState.Copy(top);

                        var val = newState.Rooms[room, depth].val;
                        newState.Rooms[room, depth].val = default;
                        newState.Hallway[space] = val;

                        uint numSpaces = (depth + 1) + (uint)Math.Abs(targetSpace - exitIndex);
                        newState.Cost += numSpaces * costs[val - 'A'];

                        queue.Enqueue(newState, newState.Cost);
                    }
                }
            }
        }

        for (int i = 0; i < hallway.Length; ++i)
        {
            if (top.Hallway[i] != default)
            {
                var val = top.Hallway[i];
                int placementIndex = -1;
                if (top.Rooms[val - 'A', 0].val == default)
                {
                    var bottomEntry = top.Rooms[val - 'A', 1].val;
                    if (bottomEntry == default)
                    {
                        placementIndex = 1;
                    }
                    else if (bottomEntry == val) // Can only go in if not trapping another letter
                    {
                        placementIndex = 0;
                    }
                }

                if (placementIndex != -1)
                {
                    var targetHallwaySpace = roomExitSpots[val - 'A'];
                    bool isRouteClear = true;
                    int space = i;
                    int inc = targetHallwaySpace < i ? -1 : 1;
                    while (space != targetHallwaySpace)
                    {
                        space += inc;
                        if (top.Hallway[space] != default)
                        {
                            isRouteClear = false;
                            break;
                        }
                    }

                    if (isRouteClear)
                    {
                        var newState = new State();
                        newState.Copy(top);

                        newState.Rooms[val - 'A', placementIndex] = (val: val, moved: true);
                        newState.Hallway[i] = default;

                        uint numSpaces = (uint)(placementIndex + 1) + (uint)Math.Abs(targetHallwaySpace - i);
                        newState.Cost += numSpaces * costs[val - 'A'];

                        if (HasWon(newState))
                        {
                            if (newState.Cost < lowestCost)
                            {
                                lowestCost = newState.Cost;
                            }
                        }
                        else
                        {
                            queue.Enqueue(newState, newState.Cost);
                        }


                    }
                }
            }
        }

        if (top.Hallway.Where(x => x != default).Count() <= 3)
        {

            for (int i = 0; i < rooms.GetLength(0); ++i)
            {
                if (top.Rooms[i, 0].val == default)
                {
                    if (top.Rooms[i, 1].val != default && !top.Rooms[i, 1].moved && top.Rooms[i, 1].val != 'A' + i)
                    {
                        MoveToHallway(i, 1);
                    }
                }
                else if (!top.Rooms[i, 0].moved && top.Rooms[i, 0].val != i - 'A')
                {
                    MoveToHallway(i, 0);
                }
            }
        }
    }

    return lowestCost;
}

bool HasWon(State state)
{
    for (int i = 0; i < state.Rooms.GetLength(0); ++i)
    {
        if (state.Rooms[i, 0].val != (char)('A' + i) || state.Rooms[i, 1].val != (char)('A' + i))
        {
            return false;
        }
    }
    return true;
}


struct State
{
    public char[] Hallway;
    public (char val, bool moved)[,] Rooms;
    public UInt64 Cost;
    public void Copy(State other)
    {
        Hallway = new char[other.Hallway.Length];
        Rooms = new (char val, bool moved)[other.Rooms.GetLength(0), other.Rooms.GetLength(1)];
        Cost = other.Cost;
        Array.Copy(other.Hallway, Hallway, other.Hallway.Length);
        Array.Copy(other.Rooms, Rooms, other.Rooms.Length);
    }
};

//int Solve(char[][] hallway)
//{
//    var neighbourOffsets = new (int x, int y)[] { (1, 0), (0, 1), (-1, 0), (0, -1) };

//    int MakeIndex(int x, int y) => riskGrid[0].Length * x + y;

//    var distances = new int[riskGrid.Length * riskGrid[0].Length];
//    var previous = new (int x, int y)[riskGrid.Length * riskGrid[0].Length];
//    Array.Fill(distances, int.MaxValue);
//    distances[MakeIndex(0, 0)] = 0;

//    var queue = new PriorityQueue<(int x, int y), int>();
//    queue.Enqueue((0, 0), 0);

//    while (queue.Count > 0)
//    {
//        var top = queue.Dequeue();
//        foreach (var offset in neighbourOffsets)
//        {
//            var neighbour = (x: top.x + offset.x, y: top.y + offset.y);
//            if (neighbour.x >= 0 && neighbour.x < riskGrid[0].Length && neighbour.y >= 0 && neighbour.y < riskGrid.Length)
//            {
//                var distanceTo = riskGrid[neighbour.y][neighbour.x];
//                var accumulativeDistance = distanceTo + distances[MakeIndex(top.x, top.y)];
//                if (distances[MakeIndex(neighbour.x, neighbour.y)] > accumulativeDistance)
//                {
//                    distances[MakeIndex(neighbour.x, neighbour.y)] = accumulativeDistance;
//                    previous[MakeIndex(neighbour.x, neighbour.y)] = top;
//                    queue.Enqueue(neighbour, accumulativeDistance);
//                }
//            }
//        }
//    }

//    int totalRisk = 0;
//    var current = (x: riskGrid[0].Length - 1, y: riskGrid.Length - 1);
//    while (current != (0, 0))
//    {
//        totalRisk += riskGrid[current.y][current.x];
//        current = previous[MakeIndex(current.x, current.y)];
//    }
//    return totalRisk;
//}