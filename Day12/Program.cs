var connections = File.ReadAllLines("input.txt").Select(connection => connection.Split('-'));
var nodeLookup = new Dictionary<string, Node>();

foreach (var connection in connections)
{
    Node? node0, node1;
    if (!nodeLookup.TryGetValue(connection[0], out node0))
    {
        node0 = new Node { Name = connection[0] };
        nodeLookup[connection[0]] = node0;
    }
    if (!nodeLookup.TryGetValue(connection[1], out node1))
    {
        node1 = new Node { Name = connection[1] };
        nodeLookup[connection[1]] = node1;
    }

    node0.Links.Add(node1);
    node1.Links.Add(node0);
}

var start = nodeLookup["start"];
var end = nodeLookup["end"];

Solve(allowSmallDoubleVisit: false);
Solve(allowSmallDoubleVisit: true);

void Solve(bool allowSmallDoubleVisit)
{
    var visitCounts = new Dictionary<Node, int>();
    var stack = new Stack<Node>();
    visitCounts.Add(start, 1);

    int routeCount = 0;
    bool hasDoubleVisited = !allowSmallDoubleVisit;

    StepDown(start);

    Console.WriteLine($"allowSmallDoubleVisit: {allowSmallDoubleVisit} - Number of routes: {routeCount}");

    void StepDown(Node node)
    {
        stack.Push(node);

        if (node.Name == "end")
        {
            ++routeCount;
        }
        else
        {
            foreach (var link in node.Links)
            {
                bool isSpecial = link.Name == "end" || link.Name == "start";
                bool isSmallCave = Char.IsLower(link.Name[0]) && !isSpecial;

                int visitCount = 0;
                visitCounts.TryGetValue(link, out visitCount);

                if (visitCount == 0 || (!isSmallCave && !isSpecial) || (!hasDoubleVisited && visitCount < 2 && isSmallCave))
                {
                    visitCounts[link] = visitCount + 1;
                    if (visitCount > 0 && isSmallCave)
                    {
                        hasDoubleVisited = true;
                    }

                    StepDown(link);

                    visitCount = visitCounts[link];
                    if (visitCount == 1)
                    {
                        visitCounts.Remove(link);
                    }
                    else
                    {
                        visitCounts[link] = visitCount - 1;
                        if (isSmallCave)
                        {
                            hasDoubleVisited = false;
                        }
                    }
                }
            }
        }

        stack.Pop();
    }
}

class Node
{
    public string Name;
    public List<Node> Links = new List<Node>();
};
