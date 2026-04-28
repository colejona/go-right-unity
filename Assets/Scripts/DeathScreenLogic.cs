public class DeathScreenLogic
{
    public enum State { Alive, Dead, CanRespawn }

    readonly float _respawnDelay;
    float _elapsed;

    public State CurrentState { get; private set; }
    public bool ShouldRespawn { get; private set; }

    public DeathScreenLogic(float respawnDelay)
    {
        _respawnDelay = respawnDelay;
    }

    public void OnPlayerDied()
    {
        if (CurrentState == State.Alive)
        {
            CurrentState = State.Dead;
            _elapsed = 0f;
        }
    }

    public void Tick(float deltaTime)
    {
        ShouldRespawn = false;
        if (CurrentState == State.Dead)
        {
            _elapsed += deltaTime;
            if (_elapsed >= _respawnDelay)
                CurrentState = State.CanRespawn;
        }
    }

    public void OnAnyInput()
    {
        if (CurrentState == State.CanRespawn)
        {
            ShouldRespawn = true;
            CurrentState = State.Alive;
        }
    }
}
