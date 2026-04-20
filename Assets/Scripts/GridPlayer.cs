using UnityEngine;
using UnityEngine.InputSystem;

public class GridPlayer : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] float cellSize = 2f;

    [Header("Visual Tween")]
    [SerializeField] float baseTweenSpeed = 20f;

    [Header("Input")]
    // Minimum seconds between accepted inputs. Set to 0 for no restriction.
    const float MinInputInterval = 0f;

    int _logicalPosition;
    float _lastInputTime = -999f;
    int _lastDir;

    InputSystem_Actions _input;

    void Awake()
    {
        _input = new InputSystem_Actions();
    }

    void OnEnable() => _input.Player.Enable();
    void OnDisable() => _input.Player.Disable();

    void Update()
    {
        ReadInput();
        TweenVisualToLogical();
    }

    void ReadInput()
    {
        float rawX = _input.Player.Move.ReadValue<Vector2>().x;
        int dir = rawX > 0.5f ? 1 : rawX < -0.5f ? -1 : 0;

        if (dir != 0 && _lastDir == 0 && Time.time - _lastInputTime >= MinInputInterval)
        {
            _logicalPosition += dir;
            _lastInputTime = Time.time;
        }

        _lastDir = dir;
    }

    void TweenVisualToLogical()
    {
        float targetX = _logicalPosition * cellSize;
        float dist = Mathf.Abs(targetX - transform.position.x);

        // Accelerate tween proportionally to how many cells behind visual is
        float speed = baseTweenSpeed * Mathf.Max(1f, dist / cellSize);

        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(targetX, transform.position.y, transform.position.z),
            speed * Time.deltaTime
        );
    }

    public int LogicalPosition => _logicalPosition;
}
