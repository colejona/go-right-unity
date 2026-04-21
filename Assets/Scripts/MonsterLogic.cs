public class MonsterLogic
{
    public int Hp { get; private set; }
    public bool IsDead => Hp <= 0;

    public MonsterLogic(int hp)
    {
        Hp = hp;
    }

    public void TakeDamage(int amount) => Hp -= amount;
}
