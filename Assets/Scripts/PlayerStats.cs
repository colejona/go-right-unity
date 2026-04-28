public class PlayerStats
{
    public int StatPoints { get; private set; }
    public int Str        { get; private set; } = 1;
    public int Agi        { get; private set; } = 0;
    public int Luk        { get; private set; } = 1;
    public int Int        { get; private set; } = 1;
    public int BonusMaxHp { get; private set; } = 0;

    public void AddLevelUpPoints(int amount) => StatPoints += amount;

    public void ApplyAllocation(int str, int agi, int luk, int intel, int hp)
    {
        int total = str + agi + luk + intel + hp;
        StatPoints  -= total;
        Str         += str;
        Agi         += agi;
        Luk         += luk;
        Int         += intel;
        BonusMaxHp  += hp * 5;
    }
}
