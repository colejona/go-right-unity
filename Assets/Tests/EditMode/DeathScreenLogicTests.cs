using NUnit.Framework;

[TestFixture]
public class DeathScreenLogicTests
{
    [Test]
    public void InitialState_IsAlive()
    {
        var logic = new DeathScreenLogic(respawnDelay: 5f);
        Assert.AreEqual(DeathScreenLogic.State.Alive, logic.CurrentState);
    }

    [Test]
    public void OnPlayerDied_TransitionsToDead()
    {
        var logic = new DeathScreenLogic(respawnDelay: 5f);
        logic.OnPlayerDied();
        Assert.AreEqual(DeathScreenLogic.State.Dead, logic.CurrentState);
    }

    [Test]
    public void Tick_WhileDead_BeforeDelay_StaysDead()
    {
        var logic = new DeathScreenLogic(respawnDelay: 5f);
        logic.OnPlayerDied();
        logic.Tick(4.99f);
        Assert.AreEqual(DeathScreenLogic.State.Dead, logic.CurrentState);
    }

    [Test]
    public void Tick_WhileDead_AfterDelay_TransitionsToCanRespawn()
    {
        var logic = new DeathScreenLogic(respawnDelay: 5f);
        logic.OnPlayerDied();
        logic.Tick(5f);
        Assert.AreEqual(DeathScreenLogic.State.CanRespawn, logic.CurrentState);
    }

    [Test]
    public void Tick_WhileAlive_DoesNothing()
    {
        var logic = new DeathScreenLogic(respawnDelay: 5f);
        logic.Tick(10f);
        Assert.AreEqual(DeathScreenLogic.State.Alive, logic.CurrentState);
    }

    [Test]
    public void ShouldRespawn_FalseInitially()
    {
        var logic = new DeathScreenLogic(respawnDelay: 5f);
        Assert.IsFalse(logic.ShouldRespawn);
    }

    [Test]
    public void OnAnyInput_WhenCanRespawn_SetsShouldRespawn()
    {
        var logic = new DeathScreenLogic(respawnDelay: 5f);
        logic.OnPlayerDied();
        logic.Tick(5f);
        logic.OnAnyInput();
        Assert.IsTrue(logic.ShouldRespawn);
    }

    [Test]
    public void OnAnyInput_WhenDead_DoesNotSetShouldRespawn()
    {
        var logic = new DeathScreenLogic(respawnDelay: 5f);
        logic.OnPlayerDied();
        logic.OnAnyInput();
        Assert.IsFalse(logic.ShouldRespawn);
    }

    [Test]
    public void OnAnyInput_WhenCanRespawn_TransitionsToAlive()
    {
        var logic = new DeathScreenLogic(respawnDelay: 5f);
        logic.OnPlayerDied();
        logic.Tick(5f);
        logic.OnAnyInput();
        Assert.AreEqual(DeathScreenLogic.State.Alive, logic.CurrentState);
    }

    [Test]
    public void ShouldRespawn_ClearsAfterTick()
    {
        var logic = new DeathScreenLogic(respawnDelay: 5f);
        logic.OnPlayerDied();
        logic.Tick(5f);
        logic.OnAnyInput();
        logic.Tick(0f);
        Assert.IsFalse(logic.ShouldRespawn);
    }
}
