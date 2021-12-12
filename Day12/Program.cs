var connections = File.ReadAllLines("input.txt").Select(connection => connection.Split('-'));

Solve(allowSmallDoubleVisit: false);
Solve(allowSmallDoubleVisit: true);

void Solve(bool allowSmallDoubleVisit)
{
    var nodeLookup = new Dictionary<string, Node>();

    foreach (var connection in connections)
    {
        Node GetNode(string name)
        {
            if (!nodeLookup.TryGetValue(name, out var node))
            {
                node = new Node { Name = name };
                nodeLookup[name] = node;
            }
            return node;
        };

        Node node0 = GetNode(connection[0]);
        Node node1 = GetNode(connection[1]);
        node0.Links.Add(node1);
        node1.Links.Add(node0);
    }

    var start = nodeLookup["start"];
    var end = nodeLookup["end"];

    var stack = new Stack<Node>();
    start.Visited++;

    int routeCount = 0;
    bool doubleVisitCount = !allowSmallDoubleVisit;

    StepDown(start);

    Console.WriteLine($"allowSmallDoubleVisit: {allowSmallDoubleVisit} - Number of routes: {routeCount}");

    void StepDown(Node node)
    {
        if (node == end)
        {
            ++routeCount;
        }
        else
        {
            stack.Push(node);

            foreach (var link in node.Links)
            {
                bool isSpecial = link == end || link == start;
                bool isSmallCave = Char.IsLower(link.Name[0]) && !isSpecial;

                int visitCount = link.Visited;

                if (visitCount == 0 || (!isSmallCave && !isSpecial) || (!doubleVisitCount && visitCount < 2 && isSmallCave))
                {
                    if (visitCount > 0 && isSmallCave)
                    {
                        doubleVisitCount = true;
                    }
                    link.Visited++;

                    StepDown(link);

                    if (link.Visited > 1 && isSmallCave)
                    {
                        doubleVisitCount = false;
                    }
                    link.Visited--;
                }
            }
            stack.Pop();
        }
    }
}

class Node
{
    public string Name;
    public int Visited;
    public List<Node> Links = new List<Node>();
};
