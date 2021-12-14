var lines = File.ReadAllLines("input.txt");
string template = lines[0];
var rules = lines.Skip(2).Select(rule => rule.Split(" -> ")).ToArray();

var ruleLookup = new string[26, 26];
foreach (var rule in rules)
{
    ruleLookup[CharIndex(rule[0][0]), CharIndex(rule[0][1])] = rule[1];
}

RunIterations(10);
RunIterations(40);

int CharIndex(char c) => c - 'A';

void RunIterations(int numIterations)
{
    var pairCounts = new Int64[26, 26];
    var charCounts = new Int64[26];
    for (int i = 0; i < template.Length; ++i)
    {
        ++charCounts[CharIndex(template[i])];
        if (i < template.Length - 1)
        {
            ++pairCounts[CharIndex(template[i]), CharIndex(template[i + 1])];
        }
    }

    for (int iteration = 0; iteration < numIterations; ++iteration)
    {
        var newPairCounts = new Int64[26, 26];
        for (int i = 0; i < pairCounts.Length; ++i)
        {
            var count = pairCounts[i % 26, i / 26];
            var rule = ruleLookup[i % 26, i / 26];
            if (rule != null)
            {
                newPairCounts[i % 26, CharIndex(rule[0])] += count;
                newPairCounts[CharIndex(rule[0]), i / 26] += count;
            }
        }
        pairCounts = newPairCounts;
    }

    var characterCounts = new Int64[26];
    var characterCounts2 = new Int64[26];
    for (int i = 0; i < 26; ++i)
    {
        for (int j = 0; j < 26; ++j)
        {
            characterCounts[i] += pairCounts[i, j];
            characterCounts2[i] += pairCounts[j, i];
        }
    }

    var mergedCounts = characterCounts.Zip(characterCounts2, (x, y) => Math.Max(x, y));
    Console.WriteLine($"Most common minus least common for {numIterations} iterations: {mergedCounts.Max() - mergedCounts.Min(x => x == 0 ? Int64.MaxValue : x)}");
}