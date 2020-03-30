using System;

public interface IPlayerInput
{
    event Action OnMoveModeTogglePressed;
    event Action<int> OnHotkeyPressed;
    float Vertical{get;}
    float Horizontal{get;}
    float MouseX { get; }
    float MouseY { get; }
    void Tick();

}
