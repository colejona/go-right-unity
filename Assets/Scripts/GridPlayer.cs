using UnityEngine;
using UnityEngine.InputSystem;

public class GridPlayer : MonoBehaviour
{
    [SerializeField] float cellSize = 2f;
    [SerializeField] float baseTweenSpeed = 20f;
    [SerializeField] int minPosition = -3;

    GridPlayerLogic _logic;
    InputSystem_Actions _input;

    public int LogicalPosition => _logic.LogicalPosition;

    void Awake()
    {
        _logic = new GridPlayerLogic(cellSize, baseTweenSpeed, minPosition);
        _input = new InputSystem_Actions();
    }

    void OnEnable() => _input.Player.Enable();
    void OnDisable() => _input.Player.Disable();

    void Update()
    {
        float rawX = _input.Player.Move.ReadValue<Vector2>().x;
        int dir = rawX > 0.5f ? 1 : rawX < -0.5f ? -1 : 0;

        _logic.ProcessInput(dir, Time.time);

        float newX = _logic.UpdateVisualX(transform.position.x, Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
