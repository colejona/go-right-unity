using UnityEngine;

public class GridPlayerLogic
{
    public const float MinInputInterval = 0f;

    readonly float _cellSize;
    readonly float _baseTweenSpeed;
    readonly int _minPosition;

    int _logicalPosition;
    float _lastInputTime = float.NegativeInfinity;
    int _lastDir;

    public int LogicalPosition => _logicalPosition;
    public float LogicalWorldX => _logicalPosition * _cellSize;
    public int BlockedInputDir { get; private set; }

    public GridPlayerLogic(float cellSize, float baseTweenSpeed, int minPosition = int.MinValue)
    {
        _cellSize = cellSize;
        _baseTweenSpeed = baseTweenSpeed;
        _minPosition = minPosition;
    }

    public void ProcessInput(int dir, float currentTime)
    {
        BlockedInputDir = 0;
        if (dir != 0 && _lastDir == 0 && currentTime - _lastInputTime >= MinInputInterval)
        {
            int next = _logicalPosition + dir;
            if (next >= _minPosition)
            {
                _logicalPosition = next;
                _lastInputTime = currentTime;
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
