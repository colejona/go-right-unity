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

    [Test]
    public void GetPositionsToSpawn_WithMinSpawnDistance_ExcludesNearbyPositions()
    {
        // player at 20, minSpawnDistance=10: only positions > 30 are eligible
        var positions = _logic.GetPositionsToSpawn(playerPosition: 20, spawnAhead: 15, minMonsterPosition: 10, minSpawnDistance: 10);
        foreach (var p in positions)
            Assert.Greater(p, 30);
    }

    [Test]
    public void GetPositionsToSpawn_WithMinSpawnDistance_ReturnsPositionsInWindow()
    {
        // player at 20, minSpawnDistance=10, spawnAhead=15: window is (30, 35]
        var positions = _logic.GetPositionsToSpawn(playerPosition: 20, spawnAhead: 15, minMonsterPosition: 10, minSpawnDistance: 10);
        CollectionAssert.AreEquivalent(new[] { 31, 32, 33, 34, 35 }, positions);
    }

    [Test]
    public void GetPositionsToSpawn_WithMinSpawnDistance_WindowNarrowerThanMin_ReturnsEmpty()
    {
        // spawnAhead < minSpawnDistance: no valid window
        var positions = _logic.GetPositionsToSpawn(playerPosition: 20, spawnAhead: 5, minMonsterPosition: 10, minSpawnDistance: 10);
        CollectionAssert.IsEmpty(positions);
    }

    [Test]
    public void GetPositionsToSpawn_WithMinSpawnDistance_SpawnsBothSides()
    {
        // player at 30, minSpawnDistance=5, spawnAhead=10: left [20,24], right [36,40]
        var positions = _logic.GetPositionsToSpawn(playerPosition: 30, spawnAhead: 10, minMonsterPosition: 10, minSpawnDistance: 5);
        CollectionAssert.AreEquivalent(new[] { 20, 21, 22, 23, 24, 36, 37, 38, 39, 40 }, positions);
    }

    [Test]
    public void GetPositionsToSpawn_LeftWindow_ClampedByMinMonsterPosition()
    {
        // player at 15, minSpawnDistance=3, spawnAhead=8: left window [max(11,7),11]=[11,11]
        var positions = _logic.GetPositionsToSpawn(playerPosition: 15, spawnAhead: 8, minMonsterPosition: 10, minSpawnDistance: 3);
        CollectionAssert.Contains(positions, 11);
        foreach (var p in positions)
            Assert.Greater(p, 10);
    }

    [Test]
    public void GetPositionsToSpawn_LeftWindow_EmptyWhenPlayerTooCloseToMin()
    {
        // player at 13, minSpawnDistance=5: left window would be [max(11,3),7]=[11,7] — empty
        var positions = _logic.GetPositionsToSpawn(playerPosition: 13, spawnAhead: 10, minMonsterPosition: 10, minSpawnDistance: 5);
        foreach (var p in positions)
            Assert.Greater(p, 13 + 5, "no left-side spawns expected");
    }

    // Spawn chance tests

    [Test]
    public void GetPositionsToSpawn_SpawnChanceZero_ReturnsEmpty()
    {
        var logic = new MonsterManagerLogic(() => 0.5);
        var positions = logic.GetPositionsToSpawn(playerPosition: 20, spawnAhead: 15, minMonsterPosition: 10, minSpawnDistance: 5, spawnChance: 0f);
        CollectionAssert.IsEmpty(positions);
    }

    [Test]
    public void GetPositionsToSpawn_SpawnChanceOne_ReturnsAllEligible()
    {
        var logic = new MonsterManagerLogic(() => 0.99);
        var positions = logic.GetPositionsToSpawn(playerPosition: 20, spawnAhead: 15, minMonsterPosition: 10, minSpawnDistance: 5, spawnChance: 1f);
        CollectionAssert.AreEquivalent(new[] { 11, 12, 13, 14, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 }, positions);
    }

    [Test]
    public void GetPositionsToSpawn_RandomBelowChance_Spawns()
    {
        var logic = new MonsterManagerLogic(() => 0.1); // always below 1/3
        var positions = logic.GetPositionsToSpawn(playerPosition: 20, spawnAhead: 15, minMonsterPosition: 10, minSpawnDistance: 5, spawnChance: 1f / 3f);
        CollectionAssert.AreEquivalent(new[] { 11, 12, 13, 14, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 }, positions);
    }

    [Test]
    public void GetPositionsToSpawn_RandomAboveChance_DoesNotSpawn()
    {
        var logic = new MonsterManagerLogic(() => 0.9); // always above 1/3
        var positions = logic.GetPositionsToSpawn(playerPosition: 20, spawnAhead: 15, minMonsterPosition: 10, minSpawnDistance: 5, spawnChance: 1f / 3f);
        CollectionAssert.IsEmpty(positions);
    }

    // GetPositionsToDespawn tests

    [Test]
    public void GetPositionsToDespawn_ReturnsTrackedPositionsFarBehindPlayer()
    {
        _logic.Add(5);
        _logic.Add(6);
        _logic.Add(25);
        var positions = _logic.GetPositionsToDespawn(playerPosition: 30, despawnDistance: 20);
        CollectionAssert.AreEquivalent(new[] { 5, 6 }, positions);
    }

    [Test]
    public void GetPositionsToDespawn_DoesNotReturnPositionsWithinRange()
    {
        _logic.Add(15);
        _logic.Add(20);
        var positions = _logic.GetPositionsToDespawn(playerPosition: 30, despawnDistance: 20);
        CollectionAssert.IsEmpty(positions);
    }

    [Test]
    public void GetPositionsToDespawn_WhenEmpty_ReturnsEmpty()
    {
        var positions = _logic.GetPositionsToDespawn(playerPosition: 30, despawnDistance: 20);
        CollectionAssert.IsEmpty(positions);
    }

    [Test]
    public void GetPositionsToDespawn_ExactlyAtBoundary_NotReturned()
    {
        _logic.Add(10);
        // playerPosition=30, despawnDistance=20 → threshold = 30-20 = 10; position must be < 10 to despawn
        var positions = _logic.GetPositionsToDespawn(playerPosition: 30, despawnDistance: 20);
        CollectionAssert.DoesNotContain(positions, 10);
    }
}
