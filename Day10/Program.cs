var chunks = File.ReadAllLines("input.txt");

var closingPairs = new Dictionary<char, char>() { ['['] = ']', ['('] = ')', ['{'] = '}', ['<'] = '>' };
var pointValues = new Dictionary<char, (int illegalScore, int autocompleteScore)>() { [']'] = (57, 2), [')'] = (3, 1), ['}'] = (1197, 3), ['>'] = (25137, 4) };

int illegalScore = 0;
var autoCompleteScores = new List<Int64>();
foreach (var line in chunks)
{
    Int64 autoCompleteScore = 0;
    var stack = new Stack<char>();
    foreach (var c in line)
    {
        if (c == '[' || c == '(' || c == '{' || c == '<')
        {
            stack.Push(c);
        }
        else if (closingPairs[stack.Peek()] == c)
        {
            stack.Pop();
        }
        else // Is illegal
        {
            illegalScore += pointValues[c].illegalScore;
            stack.Clear();
            break;
        }
    }

    if (stack.Count > 0)
    {
        while (stack.Count > 0)
        {
            var closing = closingPairs[stack.Pop()];

            autoCompleteScore = (autoCompleteScore * 5) + pointValues[closing].autocompleteScore;
        }
        autoCompleteScores.Add(autoCompleteScore);
    }
}
autoCompleteScores.Sort();

Console.WriteLine($"Total illegal score: {illegalScore}");
Console.WriteLine($"Total autocomplete score: {autoCompleteScores[autoCompleteScores.Count / 2]}");