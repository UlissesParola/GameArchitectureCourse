using UnityEngine;

public class PlayerRotator
{
    [SerializeField] private float _tiltRange = 30f;

    private Player _player;
    private float _tilt;


    public PlayerRotator(Player player)
    {
        this._player = player;
    }

    public void Tick()
    {
        //Horizontal
        var rotation = new Vector3(0, _player.PlayerInput.MouseX, 0);
        _player.transform.Rotate(rotation);

        //Vertical
        _tilt = Mathf.Clamp(_tilt - _player.PlayerInput.MouseY, -_tiltRange, _tiltRange);
        Camera.main.transform.localRotation = Quaternion.Euler(_tilt, 0f, 0f);
    }
}