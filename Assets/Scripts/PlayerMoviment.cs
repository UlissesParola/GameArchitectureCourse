using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoviment : IPlayerMoviment
{
   private Player _player;
   private CharacterController _characterController;

   public PlayerMoviment(Player player)
   {
       _player = player;
       _characterController = player.GetComponent<CharacterController>();
   }

   public void Tick()
   {
        Vector3 movementInput = new Vector3(_player.PlayerInput.Horizontal, 0, _player.PlayerInput.Vertical);
        Vector3 movement = _player.transform.rotation * movementInput;
        _characterController.SimpleMove(movement);
   }
}
