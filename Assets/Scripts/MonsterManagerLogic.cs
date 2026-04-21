using System;
using System.Collections.Generic;

public class MonsterManagerLogic
{
    readonly HashSet<int> _positions = new HashSet<int>();
    readonly Func<double> _randomSource;

    public MonsterManagerLogic(Func<double> randomSource = null)
    {
        var rng = new Random();
        _randomSource = randomSource ?? (() => rng.NextDouble());
    }

    public void Add(int position) => _positions.Add(position);

    public void RemoveAt(int position) => _positions.Remove(position);

    public bool HasMonsterAt(int position) => _positions.Contains(position);

    public IEnumerable<int> GetPositionsToSpawn(int playerPosition, int spawnAhead, int minMonsterPosition = 10, int minSpawnDistance = 0, float spawnChance = 1f)
    {
        var result = new List<int>();

        // Right window: (playerPosition + minSpawnDistance, playerPosition + spawnAhead]
        int rightLower = minSpawnDistance > 0
            ? Math.Max(minMonsterPosition, playerPosition + minSpawnDistance)
            : minMonsterPosition;
        for (int p = rightLower + 1; p <= playerPosition + spawnAhead; p++)
        {
            if (!_positions.Contains(p) && _randomSource() < spawnChance)
                result.Add(p);
        }

        // Left window: [max(minMonsterPosition+1, playerPosition - spawnAhead), playerPosition - minSpawnDistance - 1]
        if (minSpawnDistance > 0)
        {
            int leftStart = Math.Max(minMonsterPosition + 1, playerPosition - spawnAhead);
            int leftEnd = playerPosition - minSpawnDistance - 1;
            for (int p = leftStart; p <= leftEnd; p++)
            {
                if (!_positions.Contains(p) && _randomSource() < spawnChance)
                    result.Add(p);
            }
        }

        return result;
    }

    public IEnumerable<int> GetPositionsToDespawn(int playerPosition, int despawnDistance)
    {
        var result = new List<int>();
        foreach (int p in _positions)
        {
            if (p < playerPosition - despawnDistance)
                result.Add(p);
        }
        return result;
    }
}
