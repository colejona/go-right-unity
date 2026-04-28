using NUnit.Framework;

[TestFixture]
public class HpBarLogicTests
{
    [Test]
    public void FillFraction_FullHp_ReturnsOne()
    {
        Assert.AreEqual(1f, HpBarLogic.FillFraction(5, 5), 0.0001f);
    }

    [Test]
    public void FillFraction_HalfHp_ReturnsHalf()
    {
        Assert.AreEqual(0.5f, HpBarLogic.FillFraction(1, 2), 0.0001f);
    }

    [Test]
    public void FillFraction_ZeroHp_ReturnsZero()
    {
        Assert.AreEqual(0f, HpBarLogic.FillFraction(0, 5), 0.0001f);
    }

    [Test]
    public void FillFraction_NegativeHp_ClampsToZero()
    {
        Assert.AreEqual(0f, HpBarLogic.FillFraction(-2, 5), 0.0001f);
    }

    [Test]
    public void FillFraction_HpAboveMax_ClampsToOne()
    {
        Assert.AreEqual(1f, HpBarLogic.FillFraction(10, 5), 0.0001f);
    }
}
