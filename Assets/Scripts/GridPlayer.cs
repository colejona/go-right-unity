using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridPlayer : MonoBehaviour
{
    [SerializeField] float cellSize = 2f;
    [SerializeField] float baseTweenSpeed = 20f;
    [SerializeField] int minPosition = -3;

    GridPlayerLogic _logic;
    BumpAnimationLogic _bump;
    InputSystem_Actions _input;
    MonsterManager _monsterManager;
    CombatResolver _combatResolver;
    CombatTextSpawner _combatText;

    public event Action<int> OnPositionChanged;

    public int LogicalPosition => _logic.LogicalPosition;

    void Awake()
    {
        _logic = new GridPlayerLogic(cellSize, baseTweenSpeed, minPosition);
        _bump = new BumpAnimationLogic(duration: 0.2f, amplitude: cellSize * 0.02f);
        _input = new InputSystem_Actions();
        _monsterManager = FindFirstObjectByType<MonsterManager>();
        _combatResolver = new CombatResolver();
        _combatText = FindFirstObjectByType<CombatTextSpawner>();
    }

    void OnEnable() => _input.Player.Enable();
    void OnDisable() => _input.Player.Disable();

    void ResolveCombat(int monsterPosition)
    {
        var monster = _monsterManager.GetMonsterAt(monsterPosition);
        if (monster == null) return;

        var outcome = _combatResolver.Resolve(_logic.Cooldown, _logic.Speed, monster.Cooldown, monster.Speed);
        _logic.Cooldown = outcome.NewPlayerCooldown;
        monster.Cooldown = outcome.NewMonsterCooldown;

        if (outcome.WhoActs == CombatResolver.Actor.Player)
        {
            monster.Health.TakeDamage(1);
            _combatText?.Show(monster.transform.position + Vector3.up, 1);
            if (monster.Health.IsDead)
                _monsterManager.KillMonsterAt(monsterPosition);
        }
        else
        {
            _logic.TakeDamage(1);
            _combatText?.Show(transform.position + Vector3.up, 1);
        }
    }

    void Update()
    {
        float rawX = _input.Player.Move.ReadValue<Vector2>().x;
        int dir = rawX > 0.5f ? 1 : rawX < -0.5f ? -1 : 0;

        int prevPosition = _logic.LogicalPosition;
        _logic.ProcessInput(dir, Time.time, _monsterManager?.Logic);

        if (_logic.KilledMonsterAt.HasValue)
            ResolveCombat(_logic.KilledMonsterAt.Value);

        if (_logic.LogicalPosition != prevPosition)
            OnPositionChanged?.Invoke(_logic.LogicalPosition);

        if (_logic.BlockedInputDir != 0)
            _bump.Trigger(_logic.BlockedInputDir);

        float newX = _logic.UpdateVisualX(transform.position.x, Time.deltaTime);
        newX += _bump.UpdateOffset(Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
