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
    HpBar _hpBar;
    DeathScreen _deathScreen;
    DeathScreenLogic _deathLogic;

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
        _hpBar = gameObject.AddComponent<HpBar>();
        _deathScreen = new GameObject("DeathScreen").AddComponent<DeathScreen>();
        _deathLogic = new DeathScreenLogic(respawnDelay: 3f);
    }

    void OnEnable() => _input.Player.Enable();
    void OnDisable() => _input.Player.Disable();

    void ResolveCombat(int monsterPosition)
    {
        var monster = _monsterManager.GetMonsterAt(monsterPosition);
        if (monster == null) return;

        var outcome = _combatResolver.Resolve(_logic.Cooldown, _logic.Speed, _logic.MonsterCooldown, monster.Speed);
        _logic.Cooldown = outcome.NewPlayerCooldown;
        _logic.MonsterCooldown = outcome.NewMonsterCooldown;

        monster.ActivateHpBar();

        if (outcome.WhoActs == CombatResolver.Actor.Player)
        {
            monster.Health.TakeDamage(1);
            _combatText?.Show(monster.transform.position + Vector3.up, 1);
            if (monster.Health.IsDead)
            {
                _logic.AddXp(monster.XpReward);
                _monsterManager.KillMonsterAt(monsterPosition);
            }
        }
        else
        {
            _logic.TakeDamage(1);
            _combatText?.Show(transform.position + Vector3.up, 1);
            if (_logic.IsDead)
                _deathLogic.OnPlayerDied();
        }
    }

    void Update()
    {
        bool anyInput = _input.Player.Move.ReadValue<Vector2>().sqrMagnitude > 0.01f
            || _input.Player.Attack.IsPressed()
            || _input.Player.Jump.IsPressed();

        _deathLogic.Tick(Time.deltaTime);
        bool isDying = _deathLogic.CurrentState != DeathScreenLogic.State.Alive;
        _deathScreen.SetVisible(isDying);
        _deathScreen.SetPromptVisible(_deathLogic.CurrentState == DeathScreenLogic.State.CanRespawn);

        if (_deathLogic.CurrentState == DeathScreenLogic.State.CanRespawn && anyInput)
            _deathLogic.OnAnyInput();

        if (_deathLogic.ShouldRespawn)
        {
            _logic = new GridPlayerLogic(cellSize, baseTweenSpeed, minPosition);
            transform.position = new Vector3(0f, transform.position.y, transform.position.z);
        }

        if (isDying)
        {
            _hpBar.Refresh(_logic.Hp, _logic.MaxHp);
            return;
        }

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

        _hpBar.Refresh(_logic.Hp, _logic.MaxHp);
    }
}
