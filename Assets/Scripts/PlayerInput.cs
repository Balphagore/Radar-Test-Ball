using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public delegate void MouseMoveHandle(float deltaPosition);
    public static event MouseMoveHandle MouseMoveEvent;
    public delegate void MouseClickHandle(bool isEnded);
    public static event MouseClickHandle MouseClickEvent;

    //Ивенты, вызываемые InputSystem.
    public void OnMouseMove(InputAction.CallbackContext value)
    {
        MouseMoveEvent?.Invoke(value.ReadValue<float>());
    }

    public void OnMouseClick(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            MouseClickEvent?.Invoke(false);
        }
        if (value.canceled)
        {
            MouseClickEvent?.Invoke(true);
        }
    }
}