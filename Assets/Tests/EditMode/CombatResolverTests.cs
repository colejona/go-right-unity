using NUnit.Framework;

[TestFixture]
public class CombatResolverTests
{
    CombatResolver _resolver;

    [SetUp]
    public void SetUp()
    {
        _resolver = new CombatResolver();
    }

    // Pre-check: act immediately without ticking if already <= 0

    [Test]
    public void Resolve_PlayerCooldownAlreadyZero_PlayerActsWithoutTicking()
    {
        var outcome = _resolver.Resolve(playerCooldown: 0, playerSpeed: 5, monsterCooldown: 50, monsterSpeed: 3);
        Assert.AreEqual(CombatResolver.Actor.Player, outcome.WhoActs);
        Assert.AreEqual(50, outcome.NewMonsterCooldown); // not ticked
    }

    [Test]
    public void Resolve_PlayerCooldownNegative_PlayerActsWithoutTicking()
    {
        var outcome = _resolver.Resolve(playerCooldown: -3, playerSpeed: 5, monsterCooldown: 50, monsterSpeed: 3);
        Assert.AreEqual(CombatResolver.Actor.Player, outcome.WhoActs);
        Assert.AreEqual(50, outcome.NewMonsterCooldown); // not ticked
    }

    [Test]
    public void Resolve_MonsterCooldownAlreadyZero_MonsterActsWithoutTicking()
    {
        var outcome = _resolver.Resolve(playerCooldown: 50, playerSpeed: 5, monsterCooldown: 0, monsterSpeed: 3);
        Assert.AreEqual(CombatResolver.Actor.Monster, outcome.WhoActs);
        Assert.AreEqual(50, outcome.NewPlayerCooldown); // not ticked
    }

    [Test]
    public void Resolve_BothAlreadyZeroOrLess_PlayerWinsTie()
    {
        var outcome = _resolver.Resolve(playerCooldown: 0, playerSpeed: 5, monsterCooldown: -3, monsterSpeed: 3);
        Assert.AreEqual(CombatResolver.Actor.Player, outcome.WhoActs);
        Assert.AreEqual(-3, outcome.NewMonsterCooldown); // unchanged
    }

    // Cooldown reset with debt

    [Test]
    public void Resolve_PlayerActs_CooldownResetsWithDebt()
    {
        // player at -10 → reset to 100 + (-10) = 90
        var outcome = _resolver.Resolve(playerCooldown: -10, playerSpeed: 5, monsterCooldown: 50, monsterSpeed: 3, initialCooldown: 100);
        Assert.AreEqual(90, outcome.NewPlayerCooldown);
    }

    [Test]
    public void Resolve_MonsterActs_CooldownResetsWithDebt()
    {
        // monster at -5 → reset to 100 + (-5) = 95
        var outcome = _resolver.Resolve(playerCooldown: 50, playerSpeed: 5, monsterCooldown: -5, monsterSpeed: 3, initialCooldown: 100);
        Assert.AreEqual(95, outcome.NewMonsterCooldown);
    }

    // Tick loop

    [Test]
    public void Resolve_PlayerReachesZeroFirst_PlayerActs()
    {
        // player speed=11, monster speed=4, both start at 100
        // player hits 0 at tick 10 (100-110=-10), monster at 60
        var outcome = _resolver.Resolve(playerCooldown: 100, playerSpeed: 11, monsterCooldown: 100, monsterSpeed: 4, initialCooldown: 100);
        Assert.AreEqual(CombatResolver.Actor.Player, outcome.WhoActs);
        Assert.AreEqual(90, outcome.NewPlayerCooldown);  // 100 + (-10)
        Assert.AreEqual(60, outcome.NewMonsterCooldown); // 100 - 4*10
    }

    [Test]
    public void Resolve_MonsterReachesZeroFirst_MonsterActs()
    {
        // player speed=3, monster speed=15, both start at 100
        // monster hits 0 at tick 7 (100-105=-5), player at 79
        var outcome = _resolver.Resolve(playerCooldown: 100, playerSpeed: 3, monsterCooldown: 100, monsterSpeed: 15, initialCooldown: 100);
        Assert.AreEqual(CombatResolver.Actor.Monster, outcome.WhoActs);
        Assert.AreEqual(79, outcome.NewPlayerCooldown);  // 100 - 3*7
        Assert.AreEqual(95, outcome.NewMonsterCooldown); // 100 + (-5)
    }

    [Test]
    public void Resolve_BothReachZeroSameTick_PlayerWinsTie()
    {
        // player speed=10, monster speed=8, player starts at 10, monster at 8
        // tick 1: player=0, monster=0 → tie, player wins
        var outcome = _resolver.Resolve(playerCooldown: 10, playerSpeed: 10, monsterCooldown: 8, monsterSpeed: 8, initialCooldown: 100);
        Assert.AreEqual(CombatResolver.Actor.Player, outcome.WhoActs);
        Assert.AreEqual(100, outcome.NewPlayerCooldown); // 100 + 0
        Assert.AreEqual(0, outcome.NewMonsterCooldown);  // unchanged at 0
    }

    [Test]
    public void Resolve_DebtCarriesIntoReset()
    {
        // player speed=7, player cooldown=5: tick → -2, player acts, reset = 100 + (-2) = 98
        var outcome = _resolver.Resolve(playerCooldown: 5, playerSpeed: 7, monsterCooldown: 100, monsterSpeed: 1, initialCooldown: 100);
        Assert.AreEqual(CombatResolver.Actor.Player, outcome.WhoActs);
        Assert.AreEqual(98, outcome.NewPlayerCooldown);
        Assert.AreEqual(99, outcome.NewMonsterCooldown); // 100 - 1
    }
}
