using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] int maxHp = 3;
    [SerializeField] int speed = 3;

    public int GridPosition { get; private set; }
    public int Speed => speed;
    public int Cooldown { get; set; }
    public HealthLogic Health { get; private set; }

    void Awake()
    {
        gameObject.AddComponent<PlaceholderBox>();
        GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f);
    }

    public void Init(int gridPosition, float cellSize)
    {
        GridPosition = gridPosition;
        Health = new HealthLogic(maxHp);
        Cooldown = 100;
        transform.position = new Vector3(gridPosition * cellSize, transform.position.y, transform.position.z);
    }
}
