using System.Collections.Generic;

public class MonsterManagerLogic
{
    readonly HashSet<int> _positions = new HashSet<int>();

    public void Add(int position) => _positions.Add(position);

    public void RemoveAt(int position) => _positions.Remove(position);

    public bool HasMonsterAt(int position) => _positions.Contains(position);

    public IEnumerable<int> GetPositionsToSpawn(int playerPosition, int spawnAhead, int minMonsterPosition = 10)
    {
        var result = new List<int>();
        for (int p = minMonsterPosition + 1; p <= playerPosition + spawnAhead; p++)
        {
            if (!_positions.Contains(p))
                result.Add(p);
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
