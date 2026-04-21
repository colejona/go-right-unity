using System;
using System.Collections.Generic;

public class MonsterManagerLogic
{
    readonly HashSet<int> _positions = new HashSet<int>();
    readonly HashSet<int> _spawnedChunks = new HashSet<int>();
    readonly Func<double> _randomSource;

    public MonsterManagerLogic(Func<double> randomSource = null)
    {
        var rng = new Random();
        _randomSource = randomSource ?? (() => rng.NextDouble());
    }

    // Combat tracking

    public void Add(int position) => _positions.Add(position);
    public void RemoveAt(int position) => _positions.Remove(position);
    public bool HasMonsterAt(int position) => _positions.Contains(position);

    // Chunk helpers

    public int ChunkStart(int chunkIndex, int minMonsterPosition, int chunkSize) =>
        minMonsterPosition + chunkIndex * chunkSize + 1;

    public int ChunkEnd(int chunkIndex, int minMonsterPosition, int chunkSize) =>
        minMonsterPosition + (chunkIndex + 1) * chunkSize;

    // Chunk tracking

    public bool HasSpawnedChunk(int chunkIndex) => _spawnedChunks.Contains(chunkIndex);
    public void MarkChunkSpawned(int chunkIndex) => _spawnedChunks.Add(chunkIndex);
    public void MarkChunkDespawned(int chunkIndex) => _spawnedChunks.Remove(chunkIndex);

    // Spawn

    public IEnumerable<int> GetChunksToSpawn(int playerPosition, int spawnAhead, int minMonsterPosition, int minSpawnDistance, int chunkSize)
    {
        var result = new List<int>();
        int nMax = (playerPosition + spawnAhead - minMonsterPosition - 1) / chunkSize;
        for (int n = 0; n <= nMax; n++)
        {
            if (_spawnedChunks.Contains(n)) continue;
            int start = ChunkStart(n, minMonsterPosition, chunkSize);
            int end = ChunkEnd(n, minMonsterPosition, chunkSize);

            bool inRightWindow = start <= playerPosition + spawnAhead && end > playerPosition + minSpawnDistance;
            bool inLeftWindow = end < playerPosition - minSpawnDistance && end >= playerPosition - spawnAhead;

            if (inRightWindow || inLeftWindow)
                result.Add(n);
        }
        return result;
    }

    public IEnumerable<int> GetPositionsForChunk(int chunkIndex, int minMonsterPosition, int chunkSize, float spawnChance = 1f / 3f)
    {
        var result = new List<int>();
        int start = ChunkStart(chunkIndex, minMonsterPosition, chunkSize);
        int end = ChunkEnd(chunkIndex, minMonsterPosition, chunkSize);
        for (int p = start; p <= end; p++)
        {
            if (_randomSource() < spawnChance)
                result.Add(p);
        }
        return result;
    }

    // Despawn

    public IEnumerable<int> GetChunksToDespawn(int playerPosition, int despawnDistance, int minMonsterPosition, int chunkSize)
    {
        var result = new List<int>();
        foreach (int n in _spawnedChunks)
        {
            if (ChunkEnd(n, minMonsterPosition, chunkSize) < playerPosition - despawnDistance)
                result.Add(n);
        }
        return result;
    }

    public IEnumerable<int> GetPositionsInChunk(int chunkIndex, int minMonsterPosition, int chunkSize)
    {
        var result = new List<int>();
        int start = ChunkStart(chunkIndex, minMonsterPosition, chunkSize);
        int end = ChunkEnd(chunkIndex, minMonsterPosition, chunkSize);
        for (int p = start; p <= end; p++)
            result.Add(p);
        return result;
    }
}
