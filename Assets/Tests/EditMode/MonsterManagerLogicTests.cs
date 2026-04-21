using NUnit.Framework;

[TestFixture]
public class MonsterManagerLogicTests
{
    MonsterManagerLogic _logic;

    [SetUp]
    public void SetUp()
    {
        _logic = new MonsterManagerLogic();
    }

    [Test]
    public void HasMonsterAt_WhenEmpty_ReturnsFalse()
    {
        Assert.IsFalse(_logic.HasMonsterAt(0));
    }

    [Test]
    public void HasMonsterAt_AfterAdd_ReturnsTrue()
    {
        _logic.Add(5);
        Assert.IsTrue(_logic.HasMonsterAt(5));
    }

    [Test]
    public void HasMonsterAt_DifferentPosition_ReturnsFalse()
    {
        _logic.Add(5);
        Assert.IsFalse(_logic.HasMonsterAt(6));
    }

    [Test]
    public void HasMonsterAt_AfterRemove_ReturnsFalse()
    {
        _logic.Add(5);
        _logic.RemoveAt(5);
        Assert.IsFalse(_logic.HasMonsterAt(5));
    }

    [Test]
    public void RemoveAt_NonExistentPosition_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => _logic.RemoveAt(99));
    }

    [Test]
    public void Add_MultipleMonsters_AllTracked()
    {
        _logic.Add(3);
        _logic.Add(7);
        _logic.Add(10);
        Assert.IsTrue(_logic.HasMonsterAt(3));
        Assert.IsTrue(_logic.HasMonsterAt(7));
        Assert.IsTrue(_logic.HasMonsterAt(10));
    }

    // GetPositionsToSpawn tests

    [Test]
    public void GetPositionsToSpawn_ReturnsPositionsAheadOfPlayerAboveMin()
    {
        var positions = _logic.GetPositionsToSpawn(playerPosition: 12, spawnAhead: 3, minMonsterPosition: 10);
        CollectionAssert.AreEquivalent(new[] { 11, 12, 13, 14, 15 }, positions);
    }

    [Test]
    public void GetPositionsToSpawn_DoesNotReturnAlreadySpawnedPositions()
    {
        _logic.Add(11);
        _logic.Add(12);
        var positions = _logic.GetPositionsToSpawn(playerPosition: 12, spawnAhead: 3, minMonsterPosition: 10);
        CollectionAssert.DoesNotContain(positions, 11);
        CollectionAssert.DoesNotContain(positions, 12);
    }

    [Test]
    public void GetPositionsToSpawn_DoesNotReturnPositionsAtOrBelowMin()
    {
        var positions = _logic.GetPositionsToSpawn(playerPosition: 12, spawnAhead: 3, minMonsterPosition: 10);
        foreach (var p in positions)
            Assert.Greater(p, 10);
    }

    [Test]
    public void GetPositionsToSpawn_PlayerBelowMin_ReturnsEmpty()
    {
        var positions = _logic.GetPositionsToSpawn(playerPosition: 5, spawnAhead: 3, minMonsterPosition: 10);
        CollectionAssert.IsEmpty(positions);
    }

    [Test]
    public void GetPositionsToSpawn_PlayerJustReachingMin_ReturnsFirstPositions()
    {
        var positions = _logic.GetPositionsToSpawn(playerPosition: 8, spawnAhead: 3, minMonsterPosition: 10);
        CollectionAssert.AreEquivalent(new[] { 11 }, positions);
    }
}
