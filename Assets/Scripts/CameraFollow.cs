using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float smoothTime = 0.4f;
    [SerializeField] float yOffset = 1f;

    CameraFollowLogic _logic;
    Transform _target;

    void Start()
    {
        _logic = new CameraFollowLogic(smoothTime, yOffset);
        var player = GameObject.FindWithTag("Player");
        if (player != null) _target = player.transform;
    }

    void LateUpdate()
    {
        if (_target == null) return;
        transform.position = _logic.Update(transform.position, _target.position, Time.deltaTime);
    }
}
