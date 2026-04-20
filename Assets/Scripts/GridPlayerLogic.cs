using UnityEngine;

public class GridPlayerLogic
{
    public const float MinInputInterval = 0f;

    readonly float _cellSize;
    readonly float _baseTweenSpeed;

    int _logicalPosition;
    float _lastInputTime = float.NegativeInfinity;
    int _lastDir;

    public int LogicalPosition => _logicalPosition;
    public float LogicalWorldX => _logicalPosition * _cellSize;

    public GridPlayerLogic(float cellSize, float baseTweenSpeed)
    {
        _cellSize = cellSize;
        _baseTweenSpeed = baseTweenSpeed;
    }

    public void ProcessInput(int dir, float currentTime)
    {
        if (dir != 0 && _lastDir == 0 && currentTime - _lastInputTime >= MinInputInterval)
        {
            _logicalPosition += dir;
            _lastInputTime = currentTime;
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
