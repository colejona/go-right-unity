using NUnit.Framework;

[TestFixture]
public class GridPlayerLogicTests
{
    const float CellSize = 2f;
    const float TweenSpeed = 20f;

    GridPlayerLogic _logic;

    [SetUp]
    public void SetUp()
    {
        _logic = new GridPlayerLogic(CellSize, TweenSpeed);
    }

    // --- Logical position ---

    [Test]
    public void LogicalPosition_StartsAtZero()
    {
        Assert.AreEqual(0, _logic.LogicalPosition);
    }

    [Test]
    public void PressRight_IncrementsLogicalPositionByOne()
    {
        _logic.ProcessInput(1, 0f);
        Assert.AreEqual(1, _logic.LogicalPosition);
    }

    [Test]
    public void PressLeft_DecrementsLogicalPositionByOne()
    {
        _logic.ProcessInput(-1, 0f);
        Assert.AreEqual(-1, _logic.LogicalPosition);
    }

    [Test]
    public void RapidRightPresses_EachIncrementByOne()
    {
        Press(1, 0f);
        Press(1, 0.05f);
        Press(1, 0.1f);
        Assert.AreEqual(3, _logic.LogicalPosition);
    }

    [Test]
    public void RapidLeftPresses_EachDecrementByOne()
    {
        Press(-1, 0f);
        Press(-1, 0.05f);
        Press(-1, 0.1f);
        Assert.AreEqual(-3, _logic.LogicalPosition);
    }

    [Test]
    public void RightThenLeft_ReturnsToOrigin()
    {
        Press(1, 0f);
        Press(-1, 0.05f);
        Assert.AreEqual(0, _logic.LogicalPosition);
    }

    [Test]
    public void MinInputInterval_IsZero()
    {
        Assert.AreEqual(0f, GridPlayerLogic.MinInputInterval);
    }

    // --- Blocked input signal ---

    [Test]
    public void BlockedInputDir_IsZeroInitially()
    {
        Assert.AreEqual(0, _logic.BlockedInputDir);
    }

    [Test]
    public void BlockedInputDir_IsZeroOnNormalMove()
    {
        _logic.ProcessInput(1, 0f);
        Assert.AreEqual(0, _logic.BlockedInputDir);
    }

    [Test]
    public void BlockedInputDir_IsSetWhenMoveIsBlocked()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, minPosition: 0);
        logic.ProcessInput(-1, 0f);
        Assert.AreEqual(-1, logic.BlockedInputDir);
    }

    [Test]
    public void BlockedInputDir_ClearsOnNextProcessInput()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, minPosition: 0);
        logic.ProcessInput(-1, 0f);
        logic.ProcessInput(0, 0.016f);
        Assert.AreEqual(0, logic.BlockedInputDir);
    }

    // --- Left boundary ---

    [Test]
    public void CannotMoveLeftPastMinPosition()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, minPosition: -3);
        Press(logic, -1, 0f);
        Press(logic, -1, 0.05f);
        Press(logic, -1, 0.1f);
        Press(logic, -1, 0.15f); // 4th press should be blocked
        Assert.AreEqual(-3, logic.LogicalPosition);
    }

    [Test]
    public void CanMoveRightFromMinPosition()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, minPosition: -3);
        Press(logic, -1, 0f);
        Press(logic, -1, 0.05f);
        Press(logic, -1, 0.1f);
        Press(logic, 1, 0.15f);
        Assert.AreEqual(-2, logic.LogicalPosition);
    }

    [Test]
    public void NoMinPosition_AllowsUnlimitedLeftMovement()
    {
        // default constructor has no min — existing behaviour unchanged
        Press(-1, 0f);
        Press(-1, 0.05f);
        Press(-1, 0.1f);
        Press(-1, 0.15f);
        Assert.AreEqual(-4, _logic.LogicalPosition);
    }

    // --- Visual tween ---

    [Test]
    public void UpdateVisualX_WhenAlreadyAtLogicalPosition_DoesNotMove()
    {
        float result = _logic.UpdateVisualX(0f, 0.016f);
        Assert.AreEqual(0f, result, 0.0001f);
    }

    [Test]
    public void UpdateVisualX_MovesVisualTowardLogicalPosition()
    {
        _logic.ProcessInput(1, 0f); // logical X = 2
        _logic.ProcessInput(0, 0f);
        float newX = _logic.UpdateVisualX(0f, 0.016f);
        Assert.Greater(newX, 0f);
        Assert.LessOrEqual(newX, CellSize);
    }

    [Test]
    public void UpdateVisualX_FasterWhenMultipleStepsAhead()
    {
        var oneStep = new GridPlayerLogic(CellSize, TweenSpeed);
        Press(oneStep, 1, 0f);
        float moveOneStep = oneStep.UpdateVisualX(0f, 0.016f);

        var threeSteps = new GridPlayerLogic(CellSize, TweenSpeed);
        Press(threeSteps, 1, 0f);
        Press(threeSteps, 1, 0.05f);
        Press(threeSteps, 1, 0.1f);
        float moveThreeSteps = threeSteps.UpdateVisualX(0f, 0.016f);

        Assert.Greater(moveThreeSteps, moveOneStep);
    }

    // --- Speed and Cooldown ---

    [Test]
    public void Speed_ReturnsConstructorValue()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, speed: 7);
        Assert.AreEqual(7, logic.Speed);
    }

    [Test]
    public void Cooldown_StartsAt100ByDefault()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        Assert.AreEqual(100, logic.Cooldown);
    }

    [Test]
    public void Cooldown_CanBeUpdated()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        logic.Cooldown = 42;
        Assert.AreEqual(42, logic.Cooldown);
    }

    [Test]
    public void MonsterCooldown_StartsAt100ByDefault()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        Assert.AreEqual(100, logic.MonsterCooldown);
    }

    [Test]
    public void MonsterCooldown_CanBeUpdated()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        logic.MonsterCooldown = 42;
        Assert.AreEqual(42, logic.MonsterCooldown);
    }

    // --- Combat ---

    [Test]
    public void KilledMonsterAt_IsNullInitially()
    {
        Assert.IsNull(_logic.KilledMonsterAt);
    }

    [Test]
    public void KilledMonsterAt_IsNullOnNormalMove()
    {
        _logic.ProcessInput(1, 0f);
        Assert.IsNull(_logic.KilledMonsterAt);
    }

    [Test]
    public void MoveIntoMonster_DoesNotMovePlayer()
    {
        var monsters = new MonsterManagerLogic();
        monsters.Add(1);
        _logic.ProcessInput(1, 0f, monsters);
        Assert.AreEqual(0, _logic.LogicalPosition);
    }

    [Test]
    public void MoveIntoMonster_SetsKilledMonsterAt()
    {
        var monsters = new MonsterManagerLogic();
        monsters.Add(1);
        _logic.ProcessInput(1, 0f, monsters);
        Assert.AreEqual(1, _logic.KilledMonsterAt);
    }

    [Test]
    public void KilledMonsterAt_ClearsOnNextProcessInput()
    {
        var monsters = new MonsterManagerLogic();
        monsters.Add(1);
        _logic.ProcessInput(1, 0f, monsters);
        _logic.ProcessInput(0, 0.016f, monsters);
        Assert.IsNull(_logic.KilledMonsterAt);
    }

    [Test]
    public void MoveWithNoMonsters_MovesNormally()
    {
        _logic.ProcessInput(1, 0f, new MonsterManagerLogic());
        Assert.AreEqual(1, _logic.LogicalPosition);
    }

    // --- HP ---

    [Test]
    public void Hp_StartsAtInitialValue()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, hp: 5);
        Assert.AreEqual(5, logic.Hp);
    }

    [Test]
    public void MaxHp_ReturnsConstructorValue()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, hp: 5);
        Assert.AreEqual(5, logic.MaxHp);
    }

    [Test]
    public void MaxHp_DoesNotChangeAfterTakeDamage()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, hp: 5);
        logic.TakeDamage(3);
        Assert.AreEqual(5, logic.MaxHp);
    }

    [Test]
    public void TakeDamage_ReducesPlayerHp()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, hp: 5);
        logic.TakeDamage(2);
        Assert.AreEqual(3, logic.Hp);
    }

    [Test]
    public void IsDead_FalseWhenHpAboveZero()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, hp: 5);
        Assert.IsFalse(logic.IsDead);
    }

    [Test]
    public void IsDead_TrueWhenHpReachesZero()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, hp: 1);
        logic.TakeDamage(1);
        Assert.IsTrue(logic.IsDead);
    }

    [Test]
    public void IsDead_TrueWhenHpIsNegative()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, hp: 1);
        logic.TakeDamage(5);
        Assert.IsTrue(logic.IsDead);
    }

    // --- MP ---

    [Test]
    public void Mp_StartsAtMaxMp()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, maxMp: 10);
        Assert.AreEqual(10, logic.Mp);
    }

    [Test]
    public void MaxMp_ReturnsConstructorValue()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, maxMp: 10);
        Assert.AreEqual(10, logic.MaxMp);
    }

    [Test]
    public void UseMp_SufficientMp_ReturnsTrueAndDeducts()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, maxMp: 10);
        bool result = logic.UseMp(5);
        Assert.IsTrue(result);
        Assert.AreEqual(5, logic.Mp);
    }

    [Test]
    public void UseMp_InsufficientMp_ReturnsFalseAndDoesNotDeduct()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, maxMp: 10);
        logic.UseMp(8);
        bool result = logic.UseMp(5);
        Assert.IsFalse(result);
        Assert.AreEqual(2, logic.Mp);
    }

    [Test]
    public void UseMp_ExactAmount_ReturnsTrueAndReachesZero()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, maxMp: 10);
        bool result = logic.UseMp(10);
        Assert.IsTrue(result);
        Assert.AreEqual(0, logic.Mp);
    }

    [Test]
    public void AddMp_IncreasesMp()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, maxMp: 10);
        logic.UseMp(5);
        logic.AddMp(3);
        Assert.AreEqual(8, logic.Mp);
    }

    [Test]
    public void AddMp_ClampsToMaxMp()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, maxMp: 10);
        logic.AddMp(5);
        Assert.AreEqual(10, logic.Mp);
    }

    [Test]
    public void ResetForRespawn_ResetsMpToMax()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, maxMp: 10);
        logic.UseMp(5);
        logic.ResetForRespawn();
        Assert.AreEqual(10, logic.Mp);
    }

    // --- Hold to repeat ---

    [Test]
    public void HoldInput_BeforeRepeatInterval_DoesNotRepeat()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, repeatInterval: 0.33f);
        logic.ProcessInput(1, 0f);   // initial press → position 1
        logic.ProcessInput(1, 0.1f); // still held, too early
        Assert.AreEqual(1, logic.LogicalPosition);
    }

    [Test]
    public void HoldInput_AtRepeatInterval_Repeats()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, repeatInterval: 0.33f);
        logic.ProcessInput(1, 0f);    // initial press → position 1
        logic.ProcessInput(1, 0.33f); // held long enough → position 2
        Assert.AreEqual(2, logic.LogicalPosition);
    }

    [Test]
    public void HoldInput_RepeatsTwice()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, repeatInterval: 0.33f);
        logic.ProcessInput(1, 0f);    // → 1
        logic.ProcessInput(1, 0.33f); // → 2
        logic.ProcessInput(1, 0.66f); // → 3
        Assert.AreEqual(3, logic.LogicalPosition);
    }

    [Test]
    public void HoldInput_DirectionChange_RequiresRelease()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, repeatInterval: 0.33f);
        logic.ProcessInput(1, 0f);   // → 1, lastDir = 1
        logic.ProcessInput(-1, 0.5f); // direction change without release — not a new press, not same dir
        Assert.AreEqual(1, logic.LogicalPosition);
    }

    [Test]
    public void HoldInput_AfterRelease_MovesImmediately()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, repeatInterval: 0.33f);
        logic.ProcessInput(1, 0f);   // → 1
        logic.ProcessInput(0, 0.01f); // release
        logic.ProcessInput(1, 0.02f); // re-press immediately → 2
        Assert.AreEqual(2, logic.LogicalPosition);
    }

    // --- XP and Leveling ---

    [Test]
    public void Xp_StartsAtZero()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        Assert.AreEqual(0, logic.Xp);
    }

    [Test]
    public void AddXp_IncreasesXp()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        logic.AddXp(10);
        Assert.AreEqual(10, logic.Xp);
    }

    [Test]
    public void AddXp_Accumulates()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        logic.AddXp(10);
        logic.AddXp(10);
        Assert.AreEqual(20, logic.Xp);
    }

    [Test]
    public void AddXp_ZeroAmount_NoChange()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        logic.AddXp(0);
        Assert.AreEqual(0, logic.Xp);
    }

    [Test]
    public void Level_StartsAtOne()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        Assert.AreEqual(1, logic.Level);
    }

    [Test]
    public void XpToNextLevel_LevelOne_Returns100()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        Assert.AreEqual(100, logic.XpToNextLevel);
    }

    [Test]
    public void XpToNextLevel_LevelTwo_Returns200()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        logic.AddXp(100);
        Assert.AreEqual(200, logic.XpToNextLevel);
    }

    [Test]
    public void AddXp_WhenXpReachesThreshold_LevelsUp()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        logic.AddXp(100);
        Assert.AreEqual(2, logic.Level);
    }

    [Test]
    public void AddXp_WhenLevelingUp_XpCarriesOver()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        logic.AddXp(150);
        Assert.AreEqual(2, logic.Level);
        Assert.AreEqual(50, logic.Xp);
    }

    [Test]
    public void AddXp_CanLevelUpMultipleTimes()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        logic.AddXp(300); // 100 for L1->L2, 200 for L2->L3, 0 leftover
        Assert.AreEqual(3, logic.Level);
        Assert.AreEqual(0, logic.Xp);
    }

    [Test]
    public void AddXp_NoLevelUp_ReturnsZero()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        int gained = logic.AddXp(50);
        Assert.AreEqual(0, gained);
    }

    [Test]
    public void AddXp_OneLevelUp_ReturnsOne()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        int gained = logic.AddXp(100);
        Assert.AreEqual(1, gained);
    }

    [Test]
    public void AddXp_TwoLevelUps_ReturnsTwo()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        int gained = logic.AddXp(300);
        Assert.AreEqual(2, gained);
    }

    [Test]
    public void AddXp_OnLevelUp_HealsToFull()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, hp: 5);
        logic.TakeDamage(3);
        logic.AddXp(100);
        Assert.AreEqual(5, logic.Hp);
    }

    [Test]
    public void ResetForRespawn_ResetsHpToMax()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed, hp: 5);
        logic.TakeDamage(3);
        logic.ResetForRespawn();
        Assert.AreEqual(5, logic.Hp);
    }

    [Test]
    public void ResetForRespawn_PreservesXp()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        logic.AddXp(50);
        logic.ResetForRespawn();
        Assert.AreEqual(50, logic.Xp);
    }

    [Test]
    public void ResetForRespawn_PreservesLevel()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        logic.AddXp(100);
        logic.ResetForRespawn();
        Assert.AreEqual(2, logic.Level);
    }

    [Test]
    public void ResetForRespawn_ResetsLogicalPosition()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        logic.ProcessInput(1, 0f);
        logic.ProcessInput(0, 0f);
        logic.ResetForRespawn();
        Assert.AreEqual(0, logic.LogicalPosition);
    }

    [Test]
    public void ResetForRespawn_ResetsCooldown()
    {
        var logic = new GridPlayerLogic(CellSize, TweenSpeed);
        logic.Cooldown = 42;
        logic.ResetForRespawn();
        Assert.AreEqual(100, logic.Cooldown);
    }

    // --- Helpers ---

    void Press(int dir, float time)
    {
        _logic.ProcessInput(dir, time);
        _logic.ProcessInput(0, time);
    }

    void Press(GridPlayerLogic logic, int dir, float time)
    {
        logic.ProcessInput(dir, time);
        logic.ProcessInput(0, time);
    }
}
