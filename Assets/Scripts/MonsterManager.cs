using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] float cellSize = 2f;

    MonsterManagerLogic _logic = new MonsterManagerLogic();

    public MonsterManagerLogic Logic => _logic;

    void Start()
    {
        SpawnMonster(10);
    }

    void SpawnMonster(int gridPosition)
    {
        _logic.Add(gridPosition);
        var go = new GameObject("Monster");
        go.transform.SetParent(transform);
        var monster = go.AddComponent<Monster>();
        monster.Init(gridPosition, cellSize);
    }

    public void KillMonsterAt(int gridPosition)
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
}
