using NUnit.Framework;

[TestFixture]
public class BumpAnimationLogicTests
{
    const float Duration = 0.2f;
    const float Amplitude = 0.3f;

    BumpAnimationLogic _logic;

    [SetUp]
    public void SetUp()
    {
        _logic = new BumpAnimationLogic(Duration, Amplitude);
    }

    [Test]
    public void UpdateOffset_WhenNotTriggered_ReturnsZero()
    {
        Assert.AreEqual(0f, _logic.UpdateOffset(0.016f), 0.0001f);
    }

    [Test]
    public void UpdateOffset_AfterTriggerLeft_ReturnsNegativeOffset()
    {
        _logic.Trigger(-1);
        float offset = _logic.UpdateOffset(Duration * 0.5f);
        Assert.Less(offset, 0f);
    }

    [Test]
    public void UpdateOffset_AfterTriggerRight_ReturnsPositiveOffset()
    {
        _logic.Trigger(1);
        float offset = _logic.UpdateOffset(Duration * 0.5f);
        Assert.Greater(offset, 0f);
    }

    [Test]
    public void UpdateOffset_PeaksAtAmplitudeAtHalfDuration()
    {
        _logic.Trigger(-1);
        float offset = _logic.UpdateOffset(Duration * 0.5f);
        Assert.AreEqual(-Amplitude, offset, 0.001f);
    }

    [Test]
    public void UpdateOffset_AfterFullDuration_ReturnsZero()
    {
        _logic.Trigger(-1);
        _logic.UpdateOffset(Duration * 0.5f);
        float offset = _logic.UpdateOffset(Duration * 0.5f);
        Assert.AreEqual(0f, offset, 0.001f);
    }
}
