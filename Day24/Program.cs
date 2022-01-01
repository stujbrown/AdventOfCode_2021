var instructions = File.ReadAllLines("input.txt").Select(x => x.Split(' '));
var programBlocks = new List<List<string[]>>();
foreach (var instruction in instructions)
{
    if (instruction[0] == "inp")
    {
        programBlocks.Add(new List<string[]>());
    }
    programBlocks[programBlocks.Count - 1].Add(instruction);
}

Console.WriteLine($"Highest valid: {Run(false)}");
Console.WriteLine($"Lowest valid: {Run(true)}");

string Run(bool findLowest)
{
    var valid = new List<(string, string)>();

    var cachedResults = new List<(string input, Int64[] registers)>[programBlocks.Count + 1];
    cachedResults[0] = new List<(string input, long[] registers)>();
    cachedResults[0].Add(("", new Int64[4]));

    for (int blockIndex = 0; blockIndex < programBlocks.Count; ++blockIndex)
    {
        var visited = new HashSet<string>();
        cachedResults[blockIndex + 1] = new List<(string input, long[] registers)>();

        foreach (var cachedResult in cachedResults[blockIndex])
        {
            void RunForInput(int i)
            {
                var newInput = cachedResult.input + i;

                var newRegisters = new Int64[4];
                cachedResult.registers.CopyTo(newRegisters, 0);
                RunBlock(newRegisters, programBlocks[blockIndex], i);

                var key = string.Join(',', newRegisters);
                if (!visited.Contains(key))
                {
                    cachedResults[blockIndex + 1].Add((newInput, newRegisters));
                    visited.Add(key);

                    if (newRegisters[3] == 0)
                    {
                        valid.Add((newInput, key));
                    }
                }
            }

            if (findLowest)
            {
                for (int i = 1; i <= 9; ++i)
                {
                    RunForInput(i);
                }
            }
            else
            {
                for (int i = 9; i >= 1; --i)
                {
                    RunForInput(i);
                }
            }
        }
    }

    return valid[0].Item1;
}

void RunBlock(Int64[] registers, List<string[]> block, Int64 input)
{
    int RegisterIndex(char r) => r - 'w';
    Int64 Read(string input)
    {
        if (input[0] >= 'w' && input[0] <= 'z')
        {
            return registers[RegisterIndex(input[0])];
        }
        return Convert.ToInt64(input);
    }

    foreach (var instruction in block)
    {
        if (instruction[0] == "inp")
        {
            registers[RegisterIndex(instruction[1][0])] = input;
        }
        else if (instruction[0] == "add")
        {
            var result = registers[RegisterIndex(instruction[1][0])] + Read(instruction[2]);
            registers[RegisterIndex(instruction[1][0])] = result;
        }
        else if (instruction[0] == "mul")
        {
            var result = registers[RegisterIndex(instruction[1][0])] * Read(instruction[2]);
            registers[RegisterIndex(instruction[1][0])] = result;
        }
        else if (instruction[0] == "div")
        {
            var result = registers[RegisterIndex(instruction[1][0])] / Read(instruction[2]);
            registers[RegisterIndex(instruction[1][0])] = result;
        }
        else if (instruction[0] == "mod")
        {
            var result = registers[RegisterIndex(instruction[1][0])] % Read(instruction[2]);
            registers[RegisterIndex(instruction[1][0])] = result;
        }
        else if (instruction[0] == "eql")
        {
            var result = registers[RegisterIndex(instruction[1][0])] == Read(instruction[2]);

            registers[RegisterIndex(instruction[1][0])] = result ? 1 : 0;
        }
    }
}
