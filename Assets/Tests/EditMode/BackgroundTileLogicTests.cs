using System;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class BackgroundTileLogicTests
{
    const float TileWidth = 10f;
    const int TileCount = 5;
    const float Parallax = 0.5f;

    BackgroundTileLogic _logic;

    [SetUp]
    public void SetUp()
    {
        _logic = new BackgroundTileLogic(Parallax, TileWidth, TileCount);
    }

    [Test]
    public void GetTilePositions_ReturnsTileCountPositions()
    {
        float[] positions = _logic.GetTilePositions(0f);
        Assert.AreEqual(TileCount, positions.Length);
    }

    [Test]
    public void GetTilePositions_TilesAreEvenlySpaced()
    {
        float[] positions = _logic.GetTilePositions(0f);
        Array.Sort(positions);
        for (int i = 1; i < positions.Length; i++)
            Assert.AreEqual(TileWidth, positions[i] - positions[i - 1], 0.001f);
    }

    [Test]
    [TestCase(0f)]
    [TestCase(10f)]
    [TestCase(100f)]
    [TestCase(-50f)]
    public void GetTilePositions_AlwaysCoverCamera(float cameraX)
    {
        float[] positions = _logic.GetTilePositions(cameraX);
        Array.Sort(positions);
        float leftEdge = positions[0];
        float rightEdge = positions[positions.Length - 1] + TileWidth;
        Assert.LessOrEqual(leftEdge, cameraX, "Left edge should be <= cameraX");
        Assert.GreaterOrEqual(rightEdge, cameraX, "Right edge should be >= cameraX");
    }

    [Test]
    public void GetTilePositions_WithParallaxOne_ScreenPhaseIsConstant()
    {
        var logic = new BackgroundTileLogic(1f, TileWidth, TileCount);
        float cameraDelta = TileWidth * 3.7f;

        float[] before = logic.GetTilePositions(0f);
        float[] after = logic.GetTilePositions(cameraDelta);
        Array.Sort(before);
        Array.Sort(after);

        float phaseBefore = Mathf.Repeat(before[0], TileWidth);
        float phaseAfter = Mathf.Repeat(after[0] - cameraDelta, TileWidth);
        Assert.AreEqual(phaseBefore, phaseAfter, 0.001f);
    }
}
