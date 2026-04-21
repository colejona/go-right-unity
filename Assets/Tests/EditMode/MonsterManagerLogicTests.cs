using System.Linq;
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

    // Combat tracking (unchanged)

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

    // Chunk helpers
    // minMonsterPosition=10, chunkSize=10: chunk 0=[11..20], chunk 1=[21..30], chunk 2=[31..40]

    [Test]
    public void ChunkStart_ReturnsFirstPositionInChunk()
    {
        Assert.AreEqual(11, _logic.ChunkStart(0, minMonsterPosition: 10, chunkSize: 10));
        Assert.AreEqual(21, _logic.ChunkStart(1, minMonsterPosition: 10, chunkSize: 10));
    }

    [Test]
    public void ChunkEnd_ReturnsLastPositionInChunk()
    {
        Assert.AreEqual(20, _logic.ChunkEnd(0, minMonsterPosition: 10, chunkSize: 10));
        Assert.AreEqual(40, _logic.ChunkEnd(2, minMonsterPosition: 10, chunkSize: 10));
    }

    // Chunk tracking

    [Test]
    public void HasSpawnedChunk_InitiallyFalse()
    {
        Assert.IsFalse(_logic.HasSpawnedChunk(0));
    }

    [Test]
    public void HasSpawnedChunk_TrueAfterMark()
    {
        _logic.MarkChunkSpawned(0);
        Assert.IsTrue(_logic.HasSpawnedChunk(0));
    }

    [Test]
    public void HasSpawnedChunk_FalseAfterDespawn()
    {
        _logic.MarkChunkSpawned(0);
        _logic.MarkChunkDespawned(0);
        Assert.IsFalse(_logic.HasSpawnedChunk(0));
    }

    // GetChunksToSpawn
    // chunk 0=[11..20], chunk 1=[21..30], chunk 2=[31..40], chunk 3=[41..50]

    [Test]
    public void GetChunksToSpawn_RightWindow_ReturnsChunksInRange()
    {
        // player=5, spawnAhead=20, minSpawnDistance=10: right window (15,25] overlaps chunks 0 and 1
        var chunks = _logic.GetChunksToSpawn(playerPosition: 5, spawnAhead: 20, minMonsterPosition: 10, minSpawnDistance: 10, chunkSize: 10);
        CollectionAssert.AreEquivalent(new[] { 0, 1 }, chunks);
    }

    [Test]
    public void GetChunksToSpawn_AlreadySpawnedChunk_Excluded()
    {
        _logic.MarkChunkSpawned(0);
        var chunks = _logic.GetChunksToSpawn(playerPosition: 5, spawnAhead: 20, minMonsterPosition: 10, minSpawnDistance: 10, chunkSize: 10);
        CollectionAssert.DoesNotContain(chunks, 0);
    }

    [Test]
    public void GetChunksToSpawn_BothWindows_ReturnsBothSides()
    {
        // player=40, spawnAhead=25, minSpawnDistance=10
        // right window (50,65]: chunk 4=[51..60] and chunk 5=[61..70] overlap
        // left window [15,30): chunk 0=[11..20] has end=20 < 30 ✓
        var chunks = _logic.GetChunksToSpawn(playerPosition: 40, spawnAhead: 25, minMonsterPosition: 10, minSpawnDistance: 10, chunkSize: 10);
        CollectionAssert.AreEquivalent(new[] { 0, 4, 5 }, chunks);
    }

    [Test]
    public void GetChunksToSpawn_NothingInRange_ReturnsEmpty()
    {
        // player=0, spawnAhead=5, minSpawnDistance=10: window is (10,5] — impossible
        var chunks = _logic.GetChunksToSpawn(playerPosition: 0, spawnAhead: 5, minMonsterPosition: 10, minSpawnDistance: 10, chunkSize: 10);
        CollectionAssert.IsEmpty(chunks);
    }

    // GetChunksToDespawn

    [Test]
    public void GetChunksToDespawn_SpawnedChunkTooFarBehind_Returned()
    {
        _logic.MarkChunkSpawned(0); // chunk 0 end = 20
        // player=45, despawnDistance=20: threshold=25, end=20 < 25 → despawn
        var chunks = _logic.GetChunksToDespawn(playerPosition: 45, despawnDistance: 20, minMonsterPosition: 10, chunkSize: 10);
        CollectionAssert.Contains(chunks, 0);
    }

    [Test]
    public void GetChunksToDespawn_SpawnedChunkInRange_NotReturned()
    {
        _logic.MarkChunkSpawned(1); // chunk 1 end = 30
        // player=45, despawnDistance=20: threshold=25, end=30 >= 25 → keep
        var chunks = _logic.GetChunksToDespawn(playerPosition: 45, despawnDistance: 20, minMonsterPosition: 10, chunkSize: 10);
        CollectionAssert.DoesNotContain(chunks, 1);
    }

    [Test]
    public void GetChunksToDespawn_UnspawnedChunk_NotReturned()
    {
        // chunk 0 never marked spawned
        var chunks = _logic.GetChunksToDespawn(playerPosition: 45, despawnDistance: 20, minMonsterPosition: 10, chunkSize: 10);
        CollectionAssert.IsEmpty(chunks);
    }

    // GetPositionsForChunk

    [Test]
    public void GetPositionsForChunk_SpawnChanceOne_ReturnsAllPositions()
    {
        var logic = new MonsterManagerLogic(() => 0.99);
        var positions = logic.GetPositionsForChunk(chunkIndex: 0, minMonsterPosition: 10, chunkSize: 10, spawnChance: 1f);
        CollectionAssert.AreEquivalent(Enumerable.Range(11, 10), positions);
    }

    [Test]
    public void GetPositionsForChunk_SpawnChanceZero_ReturnsEmpty()
    {
        var logic = new MonsterManagerLogic(() => 0.5);
        var positions = logic.GetPositionsForChunk(chunkIndex: 0, minMonsterPosition: 10, chunkSize: 10, spawnChance: 0f);
        CollectionAssert.IsEmpty(positions);
    }

    [Test]
    public void GetPositionsForChunk_RandomBelowChance_ReturnsAll()
    {
        var logic = new MonsterManagerLogic(() => 0.1); // always < 1/3
        var positions = logic.GetPositionsForChunk(chunkIndex: 0, minMonsterPosition: 10, chunkSize: 10, spawnChance: 1f / 3f);
        CollectionAssert.AreEquivalent(Enumerable.Range(11, 10), positions);
    }

    [Test]
    public void GetPositionsForChunk_RandomAboveChance_ReturnsEmpty()
    {
        var logic = new MonsterManagerLogic(() => 0.9); // always > 1/3
        var positions = logic.GetPositionsForChunk(chunkIndex: 0, minMonsterPosition: 10, chunkSize: 10, spawnChance: 1f / 3f);
        CollectionAssert.IsEmpty(positions);
    }

    // GetPositionsInChunk

    [Test]
    public void GetPositionsInChunk_ReturnsAllPositionsInChunk()
    {
        var positions = _logic.GetPositionsInChunk(chunkIndex: 0, minMonsterPosition: 10, chunkSize: 10);
        CollectionAssert.AreEquivalent(Enumerable.Range(11, 10), positions);
    }

    [Test]
    public void GetPositionsInChunk_Chunk1_ReturnsCorrectRange()
    {
        var positions = _logic.GetPositionsInChunk(chunkIndex: 1, minMonsterPosition: 10, chunkSize: 10);
        CollectionAssert.AreEquivalent(Enumerable.Range(21, 10), positions);
    }
}
