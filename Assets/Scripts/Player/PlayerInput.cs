using System;
using UnityEngine;
public class PlayerInput: IPlayerInput
{
    public event Action<int> OnHotkeyPressed;
    public float Vertical => Input.GetAxis("Vertical");
    public float Horizontal => Input.GetAxis("Horizontal");
    public float MouseX => Input.GetAxis("Mouse X");
    public float MouseY => Input.GetAxis("Mouse Y");
    public event Action OnMoveModeTogglePressed;

    public void Tick()
    {
        if (OnMoveModeTogglePressed != null && Input.GetKeyDown(KeyCode.Minus))
        {
            OnMoveModeTogglePressed();
        }
        
        if (OnHotkeyPressed == null)
        {
            return;
        }

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                OnHotkeyPressed(i);
            }
        }
    }
}
