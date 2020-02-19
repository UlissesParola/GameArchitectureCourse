using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float _tilt;
    [SerializeField]private float _tiltRange = 30f;
    void Update()
    {
        float mouseRotation = Input.GetAxis("Mouse Y");
        _tilt = Mathf.Clamp(_tilt - mouseRotation, -_tiltRange, _tiltRange);

        transform.localRotation = Quaternion.Euler(_tilt, 0f, 0f);
    }
}
