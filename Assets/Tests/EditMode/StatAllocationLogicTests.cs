using NUnit.Framework;

[TestFixture]
public class StatAllocationLogicTests
{
    [Test]
    public void PointsRemaining_StartsAtTotal()
    {
        var logic = new StatAllocationLogic(totalPoints: 5);
        Assert.AreEqual(5, logic.PointsRemaining);
    }

    [Test]
    public void Increase_ReducesPointsRemaining()
    {
        var logic = new StatAllocationLogic(totalPoints: 5);
        logic.Increase(StatAllocationLogic.Stat.Str);
        Assert.AreEqual(4, logic.PointsRemaining);
    }

    [Test]
    public void Increase_WhenNoPointsLeft_ReturnsFalse()
    {
        var logic = new StatAllocationLogic(totalPoints: 0);
        bool result = logic.Increase(StatAllocationLogic.Stat.Str);
        Assert.IsFalse(result);
    }

    [Test]
    public void Increase_WhenPointsAvailable_ReturnsTrue()
    {
        var logic = new StatAllocationLogic(totalPoints: 5);
        bool result = logic.Increase(StatAllocationLogic.Stat.Str);
        Assert.IsTrue(result);
    }

    [Test]
    public void Decrease_WhenPendingIsZero_ReturnsFalse()
    {
        var logic = new StatAllocationLogic(totalPoints: 5);
        bool result = logic.Decrease(StatAllocationLogic.Stat.Str);
        Assert.IsFalse(result);
    }

    [Test]
    public void Decrease_WhenPendingAboveZero_ReturnsTrue()
    {
        var logic = new StatAllocationLogic(totalPoints: 5);
        logic.Increase(StatAllocationLogic.Stat.Str);
        bool result = logic.Decrease(StatAllocationLogic.Stat.Str);
        Assert.IsTrue(result);
    }

    [Test]
    public void Decrease_RestoresPointsRemaining()
    {
        var logic = new StatAllocationLogic(totalPoints: 5);
        logic.Increase(StatAllocationLogic.Stat.Str);
        logic.Decrease(StatAllocationLogic.Stat.Str);
        Assert.AreEqual(5, logic.PointsRemaining);
    }

    [Test]
    public void Pending_StartsAtZeroForAllStats()
    {
        var logic = new StatAllocationLogic(totalPoints: 5);
        Assert.AreEqual(0, logic.Pending(StatAllocationLogic.Stat.Str));
        Assert.AreEqual(0, logic.Pending(StatAllocationLogic.Stat.Agi));
        Assert.AreEqual(0, logic.Pending(StatAllocationLogic.Stat.Luk));
        Assert.AreEqual(0, logic.Pending(StatAllocationLogic.Stat.Int));
        Assert.AreEqual(0, logic.Pending(StatAllocationLogic.Stat.Hp));
    }

    [Test]
    public void Pending_TracksIncreases()
    {
        var logic = new StatAllocationLogic(totalPoints: 5);
        logic.Increase(StatAllocationLogic.Stat.Agi);
        logic.Increase(StatAllocationLogic.Stat.Agi);
        Assert.AreEqual(2, logic.Pending(StatAllocationLogic.Stat.Agi));
    }

    [Test]
    public void Commit_ReturnsPendingAllocations()
    {
        var logic = new StatAllocationLogic(totalPoints: 5);
        logic.Increase(StatAllocationLogic.Stat.Str);
        logic.Increase(StatAllocationLogic.Stat.Hp);
        logic.Increase(StatAllocationLogic.Stat.Hp);
        var result = logic.Commit();
        Assert.AreEqual(1, result.Str);
        Assert.AreEqual(0, result.Agi);
        Assert.AreEqual(0, result.Luk);
        Assert.AreEqual(0, result.Int);
        Assert.AreEqual(2, result.Hp);
    }
}
