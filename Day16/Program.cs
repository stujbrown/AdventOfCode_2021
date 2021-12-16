var binary = File.ReadAllLines("input.txt").Select(str => string.Join(string.Empty, str.Select(c => Convert.ToString(Convert.ToUInt32(c.ToString(), 16), 2).PadLeft(4, '0')))).First();

uint versionSum = 0;
int currentIndex = 0;
UInt64 result = EvaluatePacket(binary);

Console.WriteLine($"Version sum: {versionSum}");
Console.WriteLine($"Evaluated value: {result}");

UInt32 ReadInt(int numBits)
{
    var val = Convert.ToUInt32(binary.Substring(currentIndex, numBits), 2);
    currentIndex += numBits;
    return val;
}

UInt64 EvaluatePacket(string binary)
{
    versionSum += ReadInt(3);
    uint typeId = ReadInt(3);

    if (typeId == 4) // Literal
    {
        bool isLastBlock = false;
        UInt64 value = 0;
        do
        {
            isLastBlock = binary[currentIndex++] == '0' ? true : false;
            value = value << 4;
            value |= (ReadInt(4) & 0xF);

        } while (!isLastBlock);
        return value;
    }
    else // Operator
    {
        var operands = new List<UInt64>();
        if (binary[currentIndex++] == '0')
        {
            uint length = ReadInt(15);
            int endIndex = currentIndex + (int)length;
            while (currentIndex < endIndex)
            {
                operands.Add(EvaluatePacket(binary));
            }
            currentIndex = endIndex; //?
        }
        else
        {
            uint numSubPackets = ReadInt(11);
            for (int count = 0; count < numSubPackets; ++count)
            {
                operands.Add(EvaluatePacket(binary));
            }
        }

        switch (typeId)
        {
            case 0: return operands.Aggregate((a, x) => a + x);
            case 1: return operands.Aggregate((a, x) => a * x);
            case 2: return operands.Min();
            case 3: return operands.Max();
            case 5: return operands[0] > operands[1] ? 1u : 0u;
            case 6: return operands[0] < operands[1] ? 1u : 0u;
            default: return operands[0] == operands[1] ? 1u : 0u;
        }
    }
}