using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioLock : MonoBehaviour
{
    [SerializeField] private float targetWidth = 9f;
    [SerializeField] private float targetHeight = 16f;
    [SerializeField] private Rect gameAreaInPortrait = new Rect(0f, 2f / 3f, 1f, 1f / 3f);

    private Camera _camera;

    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        _camera.rect = AspectRatioLogic.ComputeViewport(
            targetWidth / targetHeight,
            Screen.width,
            Screen.height,
            gameAreaInPortrait
        );
    }
}
