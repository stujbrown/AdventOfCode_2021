int countBits(IEnumerable<string> binNumbers, int position) => binNumbers.Sum(num => num[position] == '1' ? 1 : 0);
uint makeMask(int numBits) => 0b11111111111111111111111111111111 >> (32 - numBits);

var binNumbers = File.ReadAllLines("input.txt").ToArray();

uint gammaRate = 0;
for (int i = 0; i < binNumbers[0].Length; i++)
{
    gammaRate |= ((countBits(binNumbers, i) > binNumbers.Length / 2) ? 1u : 0u) << ((binNumbers[0].Length - 1) - i);
}

uint epsilonRate = ~gammaRate & makeMask(binNumbers[0].Length);
var oxygenGenFiltered = binNumbers.ToList();
var C02ScrubberFiltered = binNumbers.ToList();

for (int i = 0; i < binNumbers[0].Length; ++i)
{
    char matchVal = (countBits(oxygenGenFiltered, i) >= (int)((oxygenGenFiltered.Count / 2f) + 0.5f)) ? '1' : '0';
    oxygenGenFiltered.RemoveAll(x => (oxygenGenFiltered.Count > 1) ? (x[i] != matchVal) : false);
    matchVal = (countBits(C02ScrubberFiltered, i) >= (int)((C02ScrubberFiltered.Count / 2f) + 0.5f)) ? '1' : '0';
    C02ScrubberFiltered.RemoveAll(x => (C02ScrubberFiltered.Count > 1) ? (x[i] != (matchVal == '1' ? '0' : '1')) : false);
}

Console.WriteLine($"Power consumption: {gammaRate * epsilonRate}");
Console.WriteLine($"Lift support rating: {Convert.ToInt64(oxygenGenFiltered[0], 2) * Convert.ToInt64(C02ScrubberFiltered[0], 2)}");