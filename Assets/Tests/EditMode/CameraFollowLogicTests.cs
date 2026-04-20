using NUnit.Framework;
using UnityEngine;

public class CameraFollowLogicTests
{
    const float SmoothTime = 0.4f;
    const float YOffset = 1f;
    const float DeltaTime = 0.016f;

    CameraFollowLogic _logic;

    [SetUp]
    public void SetUp()
    {
        _logic = new CameraFollowLogic(SmoothTime, YOffset);
    }

    [Test]
    public void Update_MovesXTowardTarget()
    {
        var current = new Vector3(0f, 0f, -10f);
        var target = new Vector3(10f, 0f, 0f);
        Vector3 result = _logic.Update(current, target, DeltaTime);
        Assert.Greater(result.x, 0f);
        Assert.Less(result.x, 10f);
    }

    [Test]
    public void Update_YIsTargetPlusOffset()
    {
        var current = new Vector3(0f, 0f, -10f);
        var target = new Vector3(0f, 3f, 0f);
        Vector3 result = _logic.Update(current, target, DeltaTime);
        Assert.AreEqual(target.y + YOffset, result.y, 0.0001f);
    }

    [Test]
    public void Update_DoesNotReachTargetInOneFrame()
    {
        var current = new Vector3(0f, 0f, -10f);
        var target = new Vector3(10f, 0f, 0f);
        Vector3 result = _logic.Update(current, target, DeltaTime);
        Assert.Less(result.x, target.x);
    }

    [Test]
    public void Update_ZPositionIsUnchanged()
    {
        var current = new Vector3(0f, 0f, -10f);
        var target = new Vector3(5f, 0f, 0f);
        Vector3 result = _logic.Update(current, target, DeltaTime);
        Assert.AreEqual(-10f, result.z, 0.0001f);
    }
}
