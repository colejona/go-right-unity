using NUnit.Framework;
using UnityEngine;

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

    [Test]
    public void BarColor_FullHealth_ReturnsHealthy()
    {
        Assert.AreEqual(HpBarLogic.ColorHealthy, HpBarLogic.BarColor(1f));
    }

    [Test]
    public void BarColor_AtHealthyThreshold_ReturnsHealthy()
    {
        Assert.AreEqual(HpBarLogic.ColorHealthy, HpBarLogic.BarColor(0.75f));
    }

    [Test]
    public void BarColor_JustBelowHealthyThreshold_ReturnsCaution()
    {
        Assert.AreEqual(HpBarLogic.ColorCaution, HpBarLogic.BarColor(0.74f));
    }

    [Test]
    public void BarColor_AtCriticalThreshold_ReturnsCaution()
    {
        Assert.AreEqual(HpBarLogic.ColorCaution, HpBarLogic.BarColor(0.25f));
    }

    [Test]
    public void BarColor_JustBelowCriticalThreshold_ReturnsCritical()
    {
        Assert.AreEqual(HpBarLogic.ColorCritical, HpBarLogic.BarColor(0.24f));
    }

    [Test]
    public void BarColor_ZeroHealth_ReturnsCritical()
    {
        Assert.AreEqual(HpBarLogic.ColorCritical, HpBarLogic.BarColor(0f));
    }
}
