using UnityEngine;

public class GridPlayerLogic
{
    public const float MinInputInterval = 0f;

    readonly float _cellSize;
    readonly float _baseTweenSpeed;
    readonly int _minPosition;

    int _logicalPosition;
    float _lastInputTime = float.NegativeInfinity;
    int _lastDir;

    public int LogicalPosition => _logicalPosition;
    public float LogicalWorldX => _logicalPosition * _cellSize;
    public int BlockedInputDir { get; private set; }
    public int? KilledMonsterAt { get; private set; }

    readonly HealthLogic _health;
    public int Hp => _health.Hp;
    public bool IsDead => _health.IsDead;
    public int Speed { get; }
    public int Cooldown { get; set; }
    public int MonsterCooldown { get; set; }

    public GridPlayerLogic(float cellSize, float baseTweenSpeed, int minPosition = int.MinValue, int hp = 3, int speed = 5)
    {
        _cellSize = cellSize;
        _baseTweenSpeed = baseTweenSpeed;
        _minPosition = minPosition;
        _health = new HealthLogic(hp);
        Speed = speed;
        Cooldown = 100;
        MonsterCooldown = 100;
    }

    public void TakeDamage(int amount) => _health.TakeDamage(amount);

    public void ProcessInput(int dir, float currentTime, MonsterManagerLogic monsters = null)
    {
        BlockedInputDir = 0;
        KilledMonsterAt = null;
        if (dir != 0 && _lastDir == 0 && currentTime - _lastInputTime >= MinInputInterval)
        {
            int next = _logicalPosition + dir;
            if (monsters != null && monsters.HasMonsterAt(next))
            {
                KilledMonsterAt = next;
            }
            else if (next >= _minPosition)
            {
                _logicalPosition = next;
                _lastInputTime = currentTime;
            }
            else
            {
                BlockedInputDir = dir;
            }
        }
        _lastDir = dir;
    }

    public float UpdateVisualX(float currentX, float deltaTime)
    {
        float targetX = LogicalWorldX;
        float dist = Mathf.Abs(targetX - currentX);
        float speed = _baseTweenSpeed * Mathf.Max(1f, dist / _cellSize);
        return Mathf.MoveTowards(currentX, targetX, speed * deltaTime);
    }
}
