using UnityEngine;

internal class PlayerRotator
{
    private Player _player;

    public PlayerRotator(Player player)
    {
        this._player = player;
    }

    public void Tick()
    {
        var rotation = new Vector3(0, _player.PlayerInput.MouseX, 0);
        _player.transform.Rotate(rotation);
    }
}