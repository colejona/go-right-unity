public class FloatingTextLogic
{
    readonly float _duration;
    readonly float _floatDistance;
    float _elapsed;

    public float YOffset { get; private set; }
    public float Alpha { get; private set; } = 1f;
    public bool IsExpired { get; private set; }

    public FloatingTextLogic(float duration, float floatDistance)
    {
        _duration = duration;
        _floatDistance = floatDistance;
    }

    public void Tick(float deltaTime)
    {
        _elapsed += deltaTime;
        float t = System.Math.Min(_elapsed / _duration, 1f);
        YOffset = t * _floatDistance;
        Alpha = 1f - t;
        IsExpired = _elapsed >= _duration;
    }
}
