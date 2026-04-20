using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float smoothTime = 0.4f;
    [SerializeField] float yOffset = 1f;

    Transform _target;
    float _velX;

    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null) _target = player.transform;
    }

    void LateUpdate()
    {
        if (_target == null) return;

        float targetX = Mathf.SmoothDamp(transform.position.x, _target.position.x, ref _velX, smoothTime);
        transform.position = new Vector3(targetX, _target.position.y + yOffset, transform.position.z);
    }
}
