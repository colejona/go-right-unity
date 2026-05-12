using NUnit.Framework;
using UnityEngine;

public class AspectRatioLogicTests
{
    const float Target = 9f / 16f; // portrait

    [Test]
    public void ComputeViewport_ExactAspectMatch_FullGameArea_ReturnsFullScreen()
    {
        var result = AspectRatioLogic.ComputeViewport(Target, 900f, 1600f, new Rect(0, 0, 1, 1));
        Assert.AreEqual(0f, result.x, 0.0001f);
        Assert.AreEqual(0f, result.y, 0.0001f);
        Assert.AreEqual(1f, result.width, 0.0001f);
        Assert.AreEqual(1f, result.height, 0.0001f);
    }

    [Test]
    public void ComputeViewport_ExactAspectMatch_TopThird_ReturnsTopThirdOfScreen()
    {
        var result = AspectRatioLogic.ComputeViewport(Target, 900f, 1600f, new Rect(0, 2f / 3f, 1, 1f / 3f));
        Assert.AreEqual(0f, result.x, 0.0001f);
        Assert.AreEqual(2f / 3f, result.y, 0.0001f);
        Assert.AreEqual(1f, result.width, 0.0001f);
        Assert.AreEqual(1f / 3f, result.height, 0.0001f);
    }

    [Test]
    public void ComputeViewport_WiderScreen_FullGameArea_PillarboxesCentered()
    {
        // 16:9 landscape screen, target 9:16 portrait → pillarbox
        float screenAspect = 1920f / 1080f;
        float expectedW = Target / screenAspect;
        float expectedX = (1f - expectedW) / 2f;

        var result = AspectRatioLogic.ComputeViewport(Target, 1920f, 1080f, new Rect(0, 0, 1, 1));
        Assert.AreEqual(expectedX, result.x, 0.0001f);
        Assert.AreEqual(0f, result.y, 0.0001f);
        Assert.AreEqual(expectedW, result.width, 0.0001f);
        Assert.AreEqual(1f, result.height, 0.0001f);
    }

    [Test]
    public void ComputeViewport_TallerScreen_FullGameArea_LetterboxesCentered()
    {
        // 9:32 screen (very tall), target 9:16 → letterbox
        float screenAspect = 1080f / 3840f;
        float expectedH = screenAspect / Target;
        float expectedY = (1f - expectedH) / 2f;

        var result = AspectRatioLogic.ComputeViewport(Target, 1080f, 3840f, new Rect(0, 0, 1, 1));
        Assert.AreEqual(0f, result.x, 0.0001f);
        Assert.AreEqual(expectedY, result.y, 0.0001f);
        Assert.AreEqual(1f, result.width, 0.0001f);
        Assert.AreEqual(expectedH, result.height, 0.0001f);
    }

    [Test]
    public void ComputeViewport_WiderScreen_TopThird_PillarboxesAndClampsToTopThird()
    {
        float screenAspect = 1920f / 1080f;
        float portraitW = Target / screenAspect;
        float portraitX = (1f - portraitW) / 2f;

        var result = AspectRatioLogic.ComputeViewport(Target, 1920f, 1080f, new Rect(0, 2f / 3f, 1, 1f / 3f));
        Assert.AreEqual(portraitX, result.x, 0.0001f);
        Assert.AreEqual(2f / 3f, result.y, 0.0001f);
        Assert.AreEqual(portraitW, result.width, 0.0001f);
        Assert.AreEqual(1f / 3f, result.height, 0.0001f);
    }
}
