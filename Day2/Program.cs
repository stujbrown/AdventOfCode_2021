var instructions = File.ReadAllLines("input.txt").Select(str =>
{
    var split = str.Split(' ');
    return (split[0], int.Parse(split[1]));
}).ToArray();

int ProcessInstructions(bool useAim)
{
    int horizontalPosition = 0;
    int depth = 0;
    int aim = 0;

    foreach (var instruction in instructions)
    {
        if (useAim)
        {
            if (instruction.Item1.Equals("forward"))
            {
                horizontalPosition += instruction.Item2;
                depth += aim * instruction.Item2;
            }
            else if (instruction.Item1.Equals("down")) 
                aim += instruction.Item2;
            else if (instruction.Item1.Equals("up")) 
                aim -= instruction.Item2;
        }
        else
        {
            if (instruction.Item1.Equals("forward"))
                horizontalPosition += instruction.Item2;
            else if (instruction.Item1.Equals("down")) 
                depth += instruction.Item2;
            else if (instruction.Item1.Equals("up")) 
                depth -= instruction.Item2;
        }
    }
    return horizontalPosition * depth;
}

Console.WriteLine($"Final position without aim: {ProcessInstructions(useAim: false)}");
Console.WriteLine($"Final position with aim: {ProcessInstructions(useAim: true)}");