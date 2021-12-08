var lines = File.ReadAllLines("input.txt").Select(str => str.Split(" | "));
var entries = lines.Select(str => (digits: str[0].Split(' ').Select(code => MakeIntCode(code)).ToArray(), outputDigits: str[1].Split(' ').Select(code => MakeIntCode(code)).ToArray()));

int uniqueAppearances = 0;
Int64 totalSum = 0;
foreach (var entry in entries)
{
    var digitsAndCounts = entry.digits.Select(digit => (digit: digit, numBitsSet: CountBits(digit))).ToList();
    var sortedDigits = digitsAndCounts.OrderBy(x => x.numBitsSet).ToList();

    var mappingTable = new uint[10];
    mappingTable[1] = sortedDigits[0].digit; // 2 bits set = 1
    mappingTable[7] = sortedDigits[1].digit; // 3 bits set = 7
    mappingTable[4] = sortedDigits[2].digit; // 4 bits set = 4
    mappingTable[8] = sortedDigits[9].digit; // 7 bits set = 8

    var fiveBitDigits = new uint[3] { sortedDigits[3].digit, sortedDigits[4].digit, sortedDigits[5].digit };
    var sixBitDigits = new uint[3] { sortedDigits[6].digit, sortedDigits[7].digit, sortedDigits[8].digit };

    mappingTable[9] = ResolveValueUsingMask(mappingTable[4] | mappingTable[7], sixBitDigits, 5); // Using a mask to filter the bottom left corner, which then finds 9
    mappingTable[6] = ResolveValueUsingMask(mappingTable[8] ^ mappingTable[1], sixBitDigits, 5); // Removing 1's bits from 8 makes a similar case for finding 6
    mappingTable[0] = sixBitDigits.Where(x => x != mappingTable[6] && x != mappingTable[9]).ToArray()[0];

    mappingTable[2] = ResolveValueUsingMask(mappingTable[9], fiveBitDigits, 4); // Find 2 as it'll only share four set bits with 9, the other two share five
    mappingTable[5] = ResolveValueUsingMask(mappingTable[6], fiveBitDigits, 5); // 6 shares five bits with 5
    mappingTable[3] = fiveBitDigits.Where(x => x != mappingTable[2] && x != mappingTable[5]).ToArray()[0];

    var number = new System.Text.StringBuilder();
    foreach (var digit in entry.outputDigits)
    {
        int numBits = CountBits(digit);
        uniqueAppearances += (numBits == 2 || numBits == 4 || numBits == 3 || numBits == 7) ? 1 : 0;

        number.Append(Array.IndexOf(mappingTable, digit));
    }
    totalSum += int.Parse(number.ToString());
}

Console.WriteLine($"Unique digit appearances: {uniqueAppearances}");
Console.WriteLine($"Total output sum: {totalSum}");


uint ResolveValueUsingMask(uint mask, uint[] values, int numBitsRequired)
{
    for (int i = 0; i < values.Length; ++i)
    {
        if (CountBits(mask & values[i]) == numBitsRequired)
        {
            return values[i];
        }
    }
    return uint.MaxValue;

}

uint MakeIntCode(string code)
{
    uint result = 0;
    foreach (var letter in code)
    {
        int index = letter - 'a';
        result |= 1u << index;
    }
    return result;
}

int CountBits(uint code) => (int)System.Runtime.Intrinsics.X86.Popcnt.PopCount(code);

