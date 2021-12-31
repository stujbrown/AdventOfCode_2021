var initialState = File.ReadAllLines("input.txt").Select(str => str.ToCharArray()).ToArray();
var costs = new uint[4] { 1, 10, 100, 1000 };
var roomExitSpots = new int[4] { 2, 4, 6, 8 };

Console.WriteLine($"Lowest cost: {Solve(false)}");
Console.WriteLine($"Lowest cost with injected entries: {Solve(true)}");

UInt64 Solve(bool useInjectedEntries)
{
    var examined = new Dictionary<string, UInt64>();

    var hallway = new char[11];
    (char val, bool moved)[,] rooms;
    if (useInjectedEntries)
    {
        rooms = new (char val, bool moved)[4, 4];
        for (int i = 0; i < 4; ++i)
        {
            rooms[i, 0] = (val: initialState[2][3 + (i * 2)], moved: false);
            rooms[i, 3] = (val: initialState[3][3 + (i * 2)], moved: false);
        }
        rooms[0, 1] = (val: 'D', moved: false);
        rooms[1, 1] = (val: 'C', moved: false);
        rooms[2, 1] = (val: 'B', moved: false);
        rooms[3, 1] = (val: 'A', moved: false);
        rooms[0, 2] = (val: 'D', moved: false);
        rooms[1, 2] = (val: 'B', moved: false);
        rooms[2, 2] = (val: 'A', moved: false);
        rooms[3, 2] = (val: 'C', moved: false);
    }
    else
    {
        rooms = new (char val, bool moved)[4, 2];
        for (int i = 0; i < 4; ++i)
        {
            rooms[i, 0] = (val: initialState[2][3 + (i * 2)], moved: false);
            rooms[i, 1] = (val: initialState[3][3 + (i * 2)], moved: false);
        }
    }
    var state = new State { Hallway = hallway, Rooms = rooms };

    UInt64 lowestCost = UInt64.MaxValue;

    var queue = new PriorityQueue<State, UInt64>();
    queue.Enqueue(state, 0);

    while (queue.Count > 0)
    {
        var top = queue.Dequeue();
        if (top.Cost > lowestCost)
            break;

        void TryEnqueue(State state)
        {
            var key = state.MakeKey();
            bool shouldAdd = true;
            if (examined.TryGetValue(key, out var existingCost))
            {
                if (state.Cost >= existingCost)
                {
                    shouldAdd = false;
                }
            }
            if (shouldAdd)
            {
                examined[key] = state.Cost;
                queue.Enqueue(state, state.Cost);
            }
        }

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

                        TryEnqueue(newState);
                    }
                }
            }
        }

        for (int i = 0; i < hallway.Length; ++i)
        {
            if (top.Hallway[i] != default)
            {
                var val = top.Hallway[i];
                int placementIndex = IndexForRoomPlacement(top, val - 'A');



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
                            TryEnqueue(newState);

                        }
                    }
                }
            }
        }

        for (int i = 0; i < rooms.GetLength(0); ++i)
        {
            var indexToMove = IndexOfRoomTop(top, i);
            if (indexToMove >= 0)
            {
                MoveToHallway(i, (uint)indexToMove);

            }
        }
    }

    return lowestCost;
}

int IndexForRoomPlacement(State state, int roomIndex)
{
    int lastFree = -1;
    for (int i = 0; i < state.Rooms.GetLength(1); ++i)
    {
        if (state.Rooms[roomIndex, i].val != default)
        {
            break;
        }
        lastFree = i;
    }
    if (lastFree == -1)
    {
        return -1;
    }

    for (int i = lastFree + 1; i < state.Rooms.GetLength(1); ++i)
    {
        if (state.Rooms[roomIndex, i].val != 'A' + roomIndex)
            return -1;
    }


    return lastFree;
}

int IndexOfRoomTop(State state, int roomIndex)
{
    for (int i = 0; i < state.Rooms.GetLength(1); ++i)
    {
        if (state.Rooms[roomIndex, i].moved == true)
        {
            return -1;
        }

        if (state.Rooms[roomIndex, i].val != default)
        {
            return i;
        }
    }

    return -1;
}

bool HasWon(State state)
{
    for (int i = 0; i < state.Rooms.GetLength(0); ++i)
    {
        for (int j = 0; j < state.Rooms.GetLength(1); ++j)
        {
            if (state.Rooms[i, j].val != (char)('A' + i))
            {
                return false;
            }
        }
    }
    return true;
}


struct State
{
    public char[] Hallway;
    public (char val, bool moved)[,] Rooms;
    public UInt64 Cost;

    public string MakeKey()
    {
        var s = new System.Text.StringBuilder();
        s.AppendLine(string.Join(",", Hallway.Select(x => x == default ? '_' : x)));
        s.AppendLine(string.Join(",", Rooms.Cast<(char val, bool moved)>().Select(x => x.val == default ? ('_', false) : x)));
        s.AppendLine(Cost.ToString());
        return s.ToString();
    }

    public void Copy(State other)
    {
        Hallway = new char[other.Hallway.Length];
        Rooms = new (char val, bool moved)[other.Rooms.GetLength(0), other.Rooms.GetLength(1)];
        Cost = other.Cost;
        Array.Copy(other.Hallway, Hallway, other.Hallway.Length);
        Array.Copy(other.Rooms, Rooms, other.Rooms.Length);
    }
};
