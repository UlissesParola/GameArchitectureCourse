using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _characterController;
    public IPlayerInput PlayerInput;

    private void Awake() 
    {
        _characterController = GetComponent<CharacterController>();
        PlayerInput = new PlayerInput();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 movementInput = new Vector3(PlayerInput.Horizontal, 0, PlayerInput.Vertical);
        Vector3 movement = transform.rotation * movementInput;
        _characterController.SimpleMove(movement);

    }
}
