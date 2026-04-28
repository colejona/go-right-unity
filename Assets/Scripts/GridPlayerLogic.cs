using UnityEngine;

public class GridPlayerLogic
{
    public const float MinInputInterval = 0f;

    readonly float _cellSize;
    readonly float _baseTweenSpeed;
    readonly int _minPosition;
    readonly float _repeatInterval;
    readonly int _baseSpeed;

    int _logicalPosition;
    float _lastInputTime = float.NegativeInfinity;
    int _lastDir;

    public int LogicalPosition => _logicalPosition;
    public float LogicalWorldX => _logicalPosition * _cellSize;
    public int BlockedInputDir { get; private set; }
    public int? KilledMonsterAt { get; private set; }

    HealthLogic _health;
    readonly PlayerStats _stats;
    public int Hp => _health.Hp;
    public int MaxHp => _health.MaxHp + (_stats?.BonusMaxHp ?? 0);
    public bool IsDead => _health.IsDead;
    public int BaseDamage => _stats?.Str ?? 1;
    public int Speed => _baseSpeed + (_stats?.Agi ?? 0);
    public int Cooldown { get; set; }
    public int MonsterCooldown { get; set; }
    public int Xp { get; private set; }
    public int Level { get; private set; } = 1;
    public int XpToNextLevel => Level * 100;
    public int Mp { get; private set; }
    public int MaxMp { get; }

    public GridPlayerLogic(float cellSize, float baseTweenSpeed, int minPosition = int.MinValue, int hp = 3, int speed = 5, float repeatInterval = 0.33f, int maxMp = 10, PlayerStats stats = null)
    {
        _cellSize = cellSize;
        _baseTweenSpeed = baseTweenSpeed;
        _minPosition = minPosition;
        _health = new HealthLogic(hp);
        _baseSpeed = speed;
        _stats = stats;
        Cooldown = 100;
        MonsterCooldown = 100;
        _repeatInterval = repeatInterval;
        MaxMp = maxMp;
        Mp = maxMp;
    }

    public void TakeDamage(int amount) => _health.TakeDamage(amount);

    public bool UseMp(int amount)
    {
        if (Mp < amount) return false;
        Mp -= amount;
        return true;
    }

    public void AddMp(int amount) => Mp = System.Math.Min(Mp + amount, MaxMp);

    public int AddXp(int amount)
    {
        Xp += amount;
        int levelsGained = 0;
        while (Xp >= XpToNextLevel)
        {
            Xp -= XpToNextLevel;
            Level++;
            levelsGained++;
            _health.HealToFull();
        }
        return levelsGained;
    }

    public void ResetForRespawn()
    {
        _health = new HealthLogic(_health.MaxHp);
        Cooldown = 100;
        MonsterCooldown = 100;
        Mp = MaxMp;
        _logicalPosition = 0;
        _lastInputTime = float.NegativeInfinity;
        _lastDir = 0;
        BlockedInputDir = 0;
        KilledMonsterAt = null;
    }

    public void ProcessInput(int dir, float currentTime, MonsterManagerLogic monsters = null)
    {
        BlockedInputDir = 0;
        KilledMonsterAt = null;

        bool isNewPress = dir != 0 && _lastDir == 0;
        bool isHeldRepeat = dir != 0 && dir == _lastDir && currentTime - _lastInputTime >= _repeatInterval;

        if (isNewPress || isHeldRepeat)
        {
            _lastInputTime = currentTime;
            int next = _logicalPosition + dir;
            if (monsters != null && monsters.HasMonsterAt(next))
            {
                KilledMonsterAt = next;
            }
            else if (next >= _minPosition)
            {
                _logicalPosition = next;
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
