var playerStarts = File.ReadAllLines("input.txt").Select(line => int.Parse(line.Substring("Player x starting position: ".Length))).ToArray();

Play(1000, false);
Play(21, true);

void Play(int goal, bool isQuantumGame)
{
    var pawnPositions = new int[2] { playerStarts[0], playerStarts[1] };
    var scores = new uint[2];

    if (isQuantumGame)
    {
        var permutationsPerTotal = new int[10];
        for (int i = 0; i < 3; ++i)
            for (int j = 0; j < 3; ++j)
                for (int k = 0; k < 3; ++k)
                    ++permutationsPerTotal[(i + 1) + (j + 1) + (k + 1)];

        int currentPlayer = 0;
        var wins = new UInt64[2];
        void RunTurnRecursive(int currentPlayer, UInt64 numUniverses)
        {
            for (int roll = 3; roll <= 9; ++roll)
            {
                int oldPos = pawnPositions[currentPlayer];
                uint oldScore = scores[currentPlayer];
                pawnPositions[currentPlayer] = ((((pawnPositions[currentPlayer] + roll) - 1) % 10) + 1);
                scores[currentPlayer] += (uint)pawnPositions[currentPlayer];

                if (scores[currentPlayer] >= goal)
                    wins[currentPlayer] += numUniverses * (UInt64)permutationsPerTotal[roll];
                else
                    RunTurnRecursive(currentPlayer == 0 ? 1 : 0, numUniverses * (UInt64)permutationsPerTotal[roll]);

                pawnPositions[currentPlayer] = oldPos;
                scores[currentPlayer] = oldScore;
            }
        }

        RunTurnRecursive(currentPlayer, 1);
        Console.WriteLine($"Highest number of wins: {wins.Max()}");
    }
    else
    {
        int diceCounter = 0;
        int numRolls = 0;
        int Roll()
        {
            diceCounter = (diceCounter % 100) + 1;
            ++numRolls;
            return diceCounter;
        }

        int winner = -1;
        while (winner == -1)
        {
            for (int i = 0; i < pawnPositions.Length; ++i)
            {
                int move = Roll() + Roll() + Roll();

                pawnPositions[i] = (((pawnPositions[i] + move) - 1) % 10) + 1;
                scores[i] += (uint)pawnPositions[i];

                if (scores[i] >= goal)
                {
                    winner = i;
                    break;
                }
            }
        }
        Console.WriteLine($"Loser score: {scores[winner == 0 ? 1 : 0] * numRolls}");
    }

}