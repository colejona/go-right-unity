public class HealthLogic
{
    public int Hp { get; private set; }
    public bool IsDead => Hp <= 0;

    public HealthLogic(int hp)
    {
        Hp = hp;
    }

    public void TakeDamage(int amount) => Hp -= amount;
}
