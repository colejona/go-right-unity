using UnityEngine;

public class CameraFollowLogic
{
    readonly float _smoothTime;
    readonly float _yOffset;

    float _velX;

    public CameraFollowLogic(float smoothTime, float yOffset)
    {
        _smoothTime = smoothTime;
        _yOffset = yOffset;
    }

    public Vector3 Update(Vector3 currentPos, Vector3 targetPos, float deltaTime)
    {
        float newX = Mathf.SmoothDamp(currentPos.x, targetPos.x, ref _velX, _smoothTime, float.MaxValue, deltaTime);
        return new Vector3(newX, targetPos.y + _yOffset, currentPos.z);
    }
}
