using NUnit.Framework;

[TestFixture]
public class PlayerStatsTests
{
    [Test]
    public void StatPoints_StartsAtZero()
    {
        var stats = new PlayerStats();
        Assert.AreEqual(0, stats.StatPoints);
    }

    [Test]
    public void Str_StartsAtOne()
    {
        var stats = new PlayerStats();
        Assert.AreEqual(1, stats.Str);
    }

    [Test]
    public void Agi_StartsAtZero()
    {
        var stats = new PlayerStats();
        Assert.AreEqual(0, stats.Agi);
    }

    [Test]
    public void Luk_StartsAtOne()
    {
        var stats = new PlayerStats();
        Assert.AreEqual(1, stats.Luk);
    }

    [Test]
    public void Int_StartsAtOne()
    {
        var stats = new PlayerStats();
        Assert.AreEqual(1, stats.Int);
    }

    [Test]
    public void BonusMaxHp_StartsAtZero()
    {
        var stats = new PlayerStats();
        Assert.AreEqual(0, stats.BonusMaxHp);
    }

    [Test]
    public void AddLevelUpPoints_IncreasesStatPoints()
    {
        var stats = new PlayerStats();
        stats.AddLevelUpPoints(5);
        Assert.AreEqual(5, stats.StatPoints);
    }

    [Test]
    public void AddLevelUpPoints_Accumulates()
    {
        var stats = new PlayerStats();
        stats.AddLevelUpPoints(5);
        stats.AddLevelUpPoints(5);
        Assert.AreEqual(10, stats.StatPoints);
    }

    [Test]
    public void ApplyAllocation_SpendsStat_Points()
    {
        var stats = new PlayerStats();
        stats.AddLevelUpPoints(5);
        stats.ApplyAllocation(str: 2, agi: 1, luk: 0, intel: 0, hp: 0);
        Assert.AreEqual(2, stats.StatPoints);
    }

    [Test]
    public void ApplyAllocation_IncreasesStr()
    {
        var stats = new PlayerStats();
        stats.AddLevelUpPoints(5);
        stats.ApplyAllocation(str: 2, agi: 0, luk: 0, intel: 0, hp: 0);
        Assert.AreEqual(3, stats.Str);
    }

    [Test]
    public void ApplyAllocation_IncreasesAgi()
    {
        var stats = new PlayerStats();
        stats.AddLevelUpPoints(5);
        stats.ApplyAllocation(str: 0, agi: 2, luk: 0, intel: 0, hp: 0);
        Assert.AreEqual(2, stats.Agi);
    }

    [Test]
    public void ApplyAllocation_IncreasesLuk()
    {
        var stats = new PlayerStats();
        stats.AddLevelUpPoints(5);
        stats.ApplyAllocation(str: 0, agi: 0, luk: 2, intel: 0, hp: 0);
        Assert.AreEqual(3, stats.Luk);
    }

    [Test]
    public void ApplyAllocation_IncreasesInt()
    {
        var stats = new PlayerStats();
        stats.AddLevelUpPoints(5);
        stats.ApplyAllocation(str: 0, agi: 0, luk: 0, intel: 2, hp: 0);
        Assert.AreEqual(3, stats.Int);
    }

    [Test]
    public void ApplyAllocation_IncreasesBonusMaxHp_ByFivePerPoint()
    {
        var stats = new PlayerStats();
        stats.AddLevelUpPoints(5);
        stats.ApplyAllocation(str: 0, agi: 0, luk: 0, intel: 0, hp: 2);
        Assert.AreEqual(10, stats.BonusMaxHp);
    }
}
