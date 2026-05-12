using UnityEngine;
using UnityEngine.InputSystem;

public class MobileTouchInput : MonoBehaviour
{
    public int Direction { get; private set; }

    void Update()
    {
        var touch = Touchscreen.current;
        if (touch == null || !touch.primaryTouch.press.isPressed)
        {
            Direction = TouchInputLogic.GetDirection(false, 0f, Screen.width);
            return;
        }
        float x = touch.primaryTouch.position.ReadValue().x;
        Direction = TouchInputLogic.GetDirection(true, x, Screen.width);
    }
}
