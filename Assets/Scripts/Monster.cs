using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] int maxHp = 3;
    [SerializeField] int speed = 3;
    [SerializeField] int xpReward = 10;

    public int GridPosition { get; private set; }
    public int Speed => speed;
    public int XpReward => xpReward;
    public HealthLogic Health { get; private set; }

    HpBar _hpBar;

    void Awake()
    {
        gameObject.AddComponent<PlaceholderBox>();
        GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f);
        _hpBar = gameObject.AddComponent<HpBar>();
        _hpBar.SetVisible(false);
    }

    public void Init(int gridPosition, float cellSize)
    {
        GridPosition = gridPosition;
        Health = new HealthLogic(maxHp);
        transform.position = new Vector3(gridPosition * cellSize, transform.position.y, transform.position.z);
    }

    public void ActivateHpBar()
    {
        _hpBar.SetVisible(true);
    }

    void Update()
    {
        if (Health != null)
            _hpBar.Refresh(Health.Hp, Health.MaxHp);
    }
}
