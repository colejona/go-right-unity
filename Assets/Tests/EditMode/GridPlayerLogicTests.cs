using NUnit.Framework;

[TestFixture]
public class GridPlayerLogicTests
{
    const float CellSize = 2f;
    const float TweenSpeed = 20f;

    GridPlayerLogic _logic;

    [SetUp]
    public void SetUp()
    {
        _logic = new GridPlayerLogic(CellSize, TweenSpeed);
    }

    // --- Logical position ---

    [Test]
    public void LogicalPosition_StartsAtZero()
    {
        Assert.AreEqual(0, _logic.LogicalPosition);
    }

    [Test]
    public void PressRight_IncrementsLogicalPositionByOne()
    {
        _logic.ProcessInput(1, 0f);
        Assert.AreEqual(1, _logic.LogicalPosition);
    }

    [Test]
    public void PressLeft_DecrementsLogicalPositionByOne()
    {
        _logic.ProcessInput(-1, 0f);
        Assert.AreEqual(-1, _logic.LogicalPosition);
    }

    [Test]
    public void RapidRightPresses_EachIncrementByOne()
    {
        Press(1, 0f);
        Press(1, 0.05f);
        Press(1, 0.1f);
        Assert.AreEqual(3, _logic.LogicalPosition);
    }

    [Test]
    public void RapidLeftPresses_EachDecrementByOne()
    {
        Press(-1, 0f);
        Press(-1, 0.05f);
        Press(-1, 0.1f);
        Assert.AreEqual(-3, _logic.LogicalPosition);
    }

    [Test]
    public void RightThenLeft_ReturnsToOrigin()
    {
        Press(1, 0f);
        Press(-1, 0.05f);
        Assert.AreEqual(0, _logic.LogicalPosition);
    }

    [Test]
    public void MinInputInterval_IsZero()
    {
        Assert.AreEqual(0f, GridPlayerLogic.MinInputInterval);
    }

    // --- Visual tween ---

    [Test]
    public void UpdateVisualX_WhenAlreadyAtLogicalPosition_DoesNotMove()
    {
        float result = _logic.UpdateVisualX(0f, 0.016f);
        Assert.AreEqual(0f, result, 0.0001f);
    }

    [Test]
    public void UpdateVisualX_MovesVisualTowardLogicalPosition()
    {
        _logic.ProcessInput(1, 0f); // logical X = 2
        _logic.ProcessInput(0, 0f);
        float newX = _logic.UpdateVisualX(0f, 0.016f);
        Assert.Greater(newX, 0f);
        Assert.LessOrEqual(newX, CellSize);
    }

    [Test]
    public void UpdateVisualX_FasterWhenMultipleStepsAhead()
    {
        var oneStep = new GridPlayerLogic(CellSize, TweenSpeed);
        Press(oneStep, 1, 0f);
        float moveOneStep = oneStep.UpdateVisualX(0f, 0.016f);

        var threeSteps = new GridPlayerLogic(CellSize, TweenSpeed);
        Press(threeSteps, 1, 0f);
        Press(threeSteps, 1, 0.05f);
        Press(threeSteps, 1, 0.1f);
        float moveThreeSteps = threeSteps.UpdateVisualX(0f, 0.016f);

        Assert.Greater(moveThreeSteps, moveOneStep);
    }

    // --- Helpers ---

    void Press(int dir, float time)
    {
        _logic.ProcessInput(dir, time);
        _logic.ProcessInput(0, time);
    }

    void Press(GridPlayerLogic logic, int dir, float time)
    {
        logic.ProcessInput(dir, time);
        logic.ProcessInput(0, time);
    }
}
