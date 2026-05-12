using NUnit.Framework;

public class TouchInputLogicTests
{
    [Test]
    public void GetDirection_NoTouch_ReturnsZero()
    {
        Assert.AreEqual(0, TouchInputLogic.GetDirection(hasTouch: false, touchX: 0f, screenWidth: 1080f));
    }

    [Test]
    public void GetDirection_TouchOnLeftHalf_ReturnsNegativeOne()
    {
        Assert.AreEqual(-1, TouchInputLogic.GetDirection(hasTouch: true, touchX: 200f, screenWidth: 1080f));
    }

    [Test]
    public void GetDirection_TouchOnRightHalf_ReturnsPositiveOne()
    {
        Assert.AreEqual(1, TouchInputLogic.GetDirection(hasTouch: true, touchX: 800f, screenWidth: 1080f));
    }

    [Test]
    public void GetDirection_TouchExactlyAtCenter_ReturnsPositiveOne()
    {
        Assert.AreEqual(1, TouchInputLogic.GetDirection(hasTouch: true, touchX: 540f, screenWidth: 1080f));
    }

    [Test]
    public void GetDirection_TouchAtLeftEdge_ReturnsNegativeOne()
    {
        Assert.AreEqual(-1, TouchInputLogic.GetDirection(hasTouch: true, touchX: 0f, screenWidth: 1080f));
    }

    [Test]
    public void GetDirection_TouchAtRightEdge_ReturnsPositiveOne()
    {
        Assert.AreEqual(1, TouchInputLogic.GetDirection(hasTouch: true, touchX: 1079f, screenWidth: 1080f));
    }
}
