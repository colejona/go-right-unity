using System.Collections.Generic;

public class MonsterManagerLogic
{
    readonly HashSet<int> _positions = new HashSet<int>();

    public void Add(int position) => _positions.Add(position);

    public void RemoveAt(int position) => _positions.Remove(position);

    public bool HasMonsterAt(int position) => _positions.Contains(position);
}
