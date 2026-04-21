using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] float cellSize = 2f;
    [SerializeField] int spawnAhead = 15;
    [SerializeField] int minSpawnDistance = 10;
    [SerializeField] int despawnDistance = 20;
    [SerializeField] int minMonsterPosition = 10;
    [SerializeField] int chunkSize = 10;
    [SerializeField] float spawnChance = 1f / 3f;

    MonsterManagerLogic _logic = new MonsterManagerLogic();
    GridPlayer _player;

    public MonsterManagerLogic Logic => _logic;

    void Start()
    {
        _player = FindFirstObjectByType<GridPlayer>();
        if (_player != null)
            _player.OnPositionChanged += OnPlayerPositionChanged;
    }

    void OnDestroy()
    {
        if (_player != null)
            _player.OnPositionChanged -= OnPlayerPositionChanged;
    }

    void OnPlayerPositionChanged(int playerPosition)
    {
        foreach (int chunk in _logic.GetChunksToSpawn(playerPosition, spawnAhead, minMonsterPosition, minSpawnDistance, chunkSize))
        {
            _logic.MarkChunkSpawned(chunk);
            foreach (int p in _logic.GetPositionsForChunk(chunk, minMonsterPosition, chunkSize, spawnChance))
                SpawnMonster(p);
        }

        var chunksToRemove = new List<int>(_logic.GetChunksToDespawn(playerPosition, despawnDistance, minMonsterPosition, chunkSize));
        foreach (int chunk in chunksToRemove)
        {
            _logic.MarkChunkDespawned(chunk);
            foreach (int p in _logic.GetPositionsInChunk(chunk, minMonsterPosition, chunkSize))
                DespawnMonsterAt(p);
        }
    }

    void SpawnMonster(int gridPosition)
    {
        _logic.Add(gridPosition);
        var go = new GameObject("Monster");
        go.transform.SetParent(transform);
        var monster = go.AddComponent<Monster>();
        monster.Init(gridPosition, cellSize);
    }

    void DespawnMonsterAt(int gridPosition)
    {
        _logic.RemoveAt(gridPosition);
        foreach (Transform child in transform)
        {
            var monster = child.GetComponent<Monster>();
            if (monster != null && monster.GridPosition == gridPosition)
            {
                Destroy(child.gameObject);
                return;
            }
        }
    }

    public void KillMonsterAt(int gridPosition)
    {
        DespawnMonsterAt(gridPosition);
    }
}
