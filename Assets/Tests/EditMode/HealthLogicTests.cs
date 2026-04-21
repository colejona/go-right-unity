using NUnit.Framework;

[TestFixture]
public class HealthLogicTests
{
    [Test]
    public void Hp_StartsAtInitialValue()
    {
        var health = new HealthLogic(hp: 3);
        Assert.AreEqual(3, health.Hp);
    }

    [Test]
    public void TakeDamage_ReducesHp()
    {
        var health = new HealthLogic(hp: 3);
        health.TakeDamage(1);
        Assert.AreEqual(2, health.Hp);
    }

    [Test]
    public void TakeDamage_ByMoreThanHp_HpGoesNegative()
    {
        var health = new HealthLogic(hp: 1);
        health.TakeDamage(5);
        Assert.AreEqual(-4, health.Hp);
    }

    [Test]
    public void IsDead_FalseWhenHpAboveZero()
    {
        var health = new HealthLogic(hp: 3);
        Assert.IsFalse(health.IsDead);
    }

    [Test]
    public void IsDead_TrueWhenHpIsZero()
    {
        var health = new HealthLogic(hp: 1);
        health.TakeDamage(1);
        Assert.IsTrue(health.IsDead);
    }

    [Test]
    public void IsDead_TrueWhenHpIsNegative()
    {
        var health = new HealthLogic(hp: 1);
        health.TakeDamage(5);
        Assert.IsTrue(health.IsDead);
    }
}
