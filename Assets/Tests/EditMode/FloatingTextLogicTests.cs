using NUnit.Framework;

[TestFixture]
public class FloatingTextLogicTests
{
    [Test]
    public void Initial_YOffsetIsZero()
    {
        var logic = new FloatingTextLogic(duration: 1f, floatDistance: 1f);
        Assert.AreEqual(0f, logic.YOffset, 0.0001f);
    }

    [Test]
    public void Initial_AlphaIsOne()
    {
        var logic = new FloatingTextLogic(duration: 1f, floatDistance: 1f);
        Assert.AreEqual(1f, logic.Alpha, 0.0001f);
    }

    [Test]
    public void Initial_IsNotExpired()
    {
        var logic = new FloatingTextLogic(duration: 1f, floatDistance: 1f);
        Assert.IsFalse(logic.IsExpired);
    }

    [Test]
    public void Tick_HalfDuration_YOffsetIsHalfDistance()
    {
        var logic = new FloatingTextLogic(duration: 1f, floatDistance: 2f);
        logic.Tick(0.5f);
        Assert.AreEqual(1f, logic.YOffset, 0.0001f);
    }

    [Test]
    public void Tick_HalfDuration_AlphaIsHalf()
    {
        var logic = new FloatingTextLogic(duration: 1f, floatDistance: 1f);
        logic.Tick(0.5f);
        Assert.AreEqual(0.5f, logic.Alpha, 0.0001f);
    }

    [Test]
    public void Tick_HalfDuration_IsNotExpired()
    {
        var logic = new FloatingTextLogic(duration: 1f, floatDistance: 1f);
        logic.Tick(0.5f);
        Assert.IsFalse(logic.IsExpired);
    }

    [Test]
    public void Tick_FullDuration_IsExpired()
    {
        var logic = new FloatingTextLogic(duration: 1f, floatDistance: 1f);
        logic.Tick(1f);
        Assert.IsTrue(logic.IsExpired);
    }

    [Test]
    public void Tick_FullDuration_AlphaIsZero()
    {
        var logic = new FloatingTextLogic(duration: 1f, floatDistance: 1f);
        logic.Tick(1f);
        Assert.AreEqual(0f, logic.Alpha, 0.0001f);
    }

    [Test]
    public void Tick_FullDuration_YOffsetIsFloatDistance()
    {
        var logic = new FloatingTextLogic(duration: 1f, floatDistance: 2f);
        logic.Tick(1f);
        Assert.AreEqual(2f, logic.YOffset, 0.0001f);
    }

    [Test]
    public void Tick_BeyondDuration_DoesNotExceedFloatDistance()
    {
        var logic = new FloatingTextLogic(duration: 1f, floatDistance: 2f);
        logic.Tick(5f);
        Assert.AreEqual(2f, logic.YOffset, 0.0001f);
    }

    [Test]
    public void Tick_BeyondDuration_AlphaDoesNotGoBelowZero()
    {
        var logic = new FloatingTextLogic(duration: 1f, floatDistance: 1f);
        logic.Tick(5f);
        Assert.AreEqual(0f, logic.Alpha, 0.0001f);
    }
}
