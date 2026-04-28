public class HealthLogic
{
    public int Hp { get; private set; }
    public int MaxHp { get; }
    public bool IsDead => Hp <= 0;

    public HealthLogic(int hp)
    {
        Hp = hp;
        MaxHp = hp;
    }

    public void TakeDamage(int amount) => Hp -= amount;
    public void HealToFull() => Hp = MaxHp;
    public void SetHp(int value) => Hp = value;
}
