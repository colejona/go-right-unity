using UnityEngine;

public class BumpAnimationLogic
{
    readonly float _duration;
    readonly float _amplitude;

    float _elapsed = float.MaxValue;
    int _dir;

    public BumpAnimationLogic(float duration, float amplitude)
    {
        _duration = duration;
        _amplitude = amplitude;
    }

    public void Trigger(int dir)
    {
        _dir = dir;
        _elapsed = 0f;
    }

    public float UpdateOffset(float deltaTime)
    {
        if (_elapsed >= _duration)
            return 0f;

        _elapsed += deltaTime;
        float t = Mathf.Clamp01(_elapsed / _duration);
        return _dir * _amplitude * Mathf.Sin(t * Mathf.PI);
    }
}
