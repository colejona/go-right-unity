using UnityEngine;
using UnityEngine.InputSystem;

public class GridPlayer : MonoBehaviour
{
    [SerializeField] float cellSize = 2f;
    [SerializeField] float baseTweenSpeed = 20f;
    [SerializeField] int minPosition = -3;

    GridPlayerLogic _logic;
    BumpAnimationLogic _bump;
    InputSystem_Actions _input;

    public int LogicalPosition => _logic.LogicalPosition;

    void Awake()
    {
        _logic = new GridPlayerLogic(cellSize, baseTweenSpeed, minPosition);
        _bump = new BumpAnimationLogic(duration: 0.2f, amplitude: cellSize * 0.02f);
        _input = new InputSystem_Actions();
    }

    void OnEnable() => _input.Player.Enable();
    void OnDisable() => _input.Player.Disable();

    void Update()
    {
        float rawX = _input.Player.Move.ReadValue<Vector2>().x;
        int dir = rawX > 0.5f ? 1 : rawX < -0.5f ? -1 : 0;

        _logic.ProcessInput(dir, Time.time);

        if (_logic.BlockedInputDir != 0)
            _bump.Trigger(_logic.BlockedInputDir);

        float newX = _logic.UpdateVisualX(transform.position.x, Time.deltaTime);
        newX += _bump.UpdateOffset(Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
