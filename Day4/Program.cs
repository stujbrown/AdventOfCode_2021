var lines = File.ReadAllLines("input.txt");
var callouts = lines[0].Split(',').Select(str => int.Parse(str));
int boardSize = lines[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
int startingBoardCount = (lines.Length - 1) / (boardSize + 1);

var boards = new List<Board>();
for (int i = 0; i < startingBoardCount; ++i)
{
    boards.Add(new Board { columnHits = new int[boardSize], rowHits = new int[boardSize] });
    var boardLines = lines.Skip(2 + (i * (boardSize + 1))).Take(boardSize).ToArray();
    for (int y = 0; y < boardSize; ++y)
    {
        var numbers = boardLines[y].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (int x = 0; x < boardSize; ++x)
        {
            boards.Last().valuesAndPosition.Add(int.Parse(numbers[x]), (x, y));
        }
    };
}

foreach (int callout in callouts)
{
    foreach (var board in boards.ToArray())
    {
        if (board.valuesAndPosition.TryGetValue(callout, out (int, int) position))
        {
            board.valuesAndPosition.Remove(callout);
            if (++board.rowHits[position.Item2] >= boardSize || ++board.columnHits[position.Item1] >= boardSize)
            {
                if (boards.Count == startingBoardCount)
                {
                    Console.WriteLine($"Winning board score: {board.valuesAndPosition.Keys.Sum() * callout}");
                }
                else if (boards.Count == 1)
                {
                    Console.WriteLine($"Losing board score: {board.valuesAndPosition.Keys.Sum() * callout}");
                }
                boards.Remove(board);
            }
        }
    }
}

class Board
{
    public Dictionary<int, (int, int)> valuesAndPosition = new Dictionary<int, (int, int)>();
    public int[] columnHits = new int[0];
    public int[] rowHits = new int[0];
};