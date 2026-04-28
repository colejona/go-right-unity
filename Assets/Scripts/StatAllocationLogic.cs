public class StatAllocationLogic
{
    public enum Stat { Str, Agi, Luk, Int, Hp }

    public struct Allocation
    {
        public int Str, Agi, Luk, Int, Hp;
    }

    readonly int[] _pending = new int[5];

    public int PointsRemaining { get; private set; }

    public StatAllocationLogic(int totalPoints)
    {
        PointsRemaining = totalPoints;
    }

    public bool Increase(Stat stat)
    {
        if (PointsRemaining <= 0) return false;
        _pending[(int)stat]++;
        PointsRemaining--;
        return true;
    }

    public bool Decrease(Stat stat)
    {
        if (_pending[(int)stat] <= 0) return false;
        _pending[(int)stat]--;
        PointsRemaining++;
        return true;
    }

    public int Pending(Stat stat) => _pending[(int)stat];

    public Allocation Commit() => new Allocation
    {
        Str = _pending[(int)Stat.Str],
        Agi = _pending[(int)Stat.Agi],
        Luk = _pending[(int)Stat.Luk],
        Int = _pending[(int)Stat.Int],
        Hp  = _pending[(int)Stat.Hp],
    };
}
