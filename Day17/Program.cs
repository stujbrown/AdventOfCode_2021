var input = File.ReadAllLines("input.txt")[0].Substring("target area: ".Length).Split(", ");
var rangeX = input[0].Substring("x=".Length).Split("..").Select(x => int.Parse(x)).ToArray();
var rangeY = input[1].Substring("y=".Length).Split("..").Select(x => int.Parse(x)).ToArray();

int SolveStartX(int x) => (int)(((-1 + MathF.Sqrt(1 + (8 * x))) / 2.0f) + 0.5f);
(int x, int y) startVelocity = (SolveStartX(rangeX[0]), 0);

int overallHighestPoint = int.MinValue;

int numHits = 0;
for (; startVelocity.x <= Math.Abs(rangeX[1]); ++startVelocity.x)
{
    for (startVelocity.y = -Math.Abs(rangeY[0]); startVelocity.y <= Math.Abs(rangeY[0]); ++startVelocity.y)
    {
        var pos = (x: 0, y: 0);
        var velocity = startVelocity;
        int highestPoint = 0;
        while (pos.y >= rangeY[0] || velocity.y >= 0)
        {
            if (pos.y > highestPoint)
            {
                highestPoint = pos.y;
            }

            pos = (pos.x + velocity.x, pos.y + velocity.y);
            if (pos.x >= rangeX[0] && pos.x <= rangeX[1] && pos.y >= rangeY[0] && pos.y <= rangeY[1])
            {
                if (highestPoint > overallHighestPoint)
                {
                    overallHighestPoint = highestPoint;
                }
                ++numHits;
                break;
            }
            velocity = (Math.Max(velocity.x - 1, 0), velocity.y - 1);
        }
    }
}

Console.WriteLine($"Highest reachable point: {overallHighestPoint}");
Console.WriteLine($"Total valid velocities: {numHits}");
