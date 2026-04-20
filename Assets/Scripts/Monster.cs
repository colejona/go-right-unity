using UnityEngine;

public class Monster : MonoBehaviour
{
    public int GridPosition { get; private set; }

    void Awake()
    {
        gameObject.AddComponent<PlaceholderBox>();
        GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f);
    }

    public void Init(int gridPosition, float cellSize)
    {
        GridPosition = gridPosition;
        transform.position = new Vector3(gridPosition * cellSize, transform.position.y, transform.position.z);
    }
}
