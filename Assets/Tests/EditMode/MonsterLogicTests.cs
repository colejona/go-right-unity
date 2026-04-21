using NUnit.Framework;

[TestFixture]
public class MonsterLogicTests
{
    [Test]
    public void Hp_StartsAtInitialValue()
    {
        var monster = new MonsterLogic(hp: 3);
        Assert.AreEqual(3, monster.Hp);
    }

    [Test]
    public void TakeDamage_ReducesHp()
    {
        var monster = new MonsterLogic(hp: 3);
        monster.TakeDamage(1);
        Assert.AreEqual(2, monster.Hp);
    }

    [Test]
    public void TakeDamage_ByMoreThanHp_HpGoesNegative()
    {
        var monster = new MonsterLogic(hp: 1);
        monster.TakeDamage(5);
        Assert.AreEqual(-4, monster.Hp);
    }

    [Test]
    public void IsDead_FalseWhenHpAboveZero()
    {
        var monster = new MonsterLogic(hp: 3);
        Assert.IsFalse(monster.IsDead);
    }

    [Test]
    public void IsDead_TrueWhenHpIsZero()
    {
        var monster = new MonsterLogic(hp: 1);
        monster.TakeDamage(1);
        Assert.IsTrue(monster.IsDead);
    }

    [Test]
    public void IsDead_TrueWhenHpIsNegative()
    {
        var monster = new MonsterLogic(hp: 1);
        monster.TakeDamage(5);
        Assert.IsTrue(monster.IsDead);
    }
}
