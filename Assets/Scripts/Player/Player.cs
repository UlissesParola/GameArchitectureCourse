using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public IPlayerInput PlayerInput;
    private CharacterController _characterController;
    private IPlayerMoviment _playerMoviment;
    private PlayerRotator _playerRotator;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        PlayerInput = new PlayerInput();
        _playerMoviment = new PlayerMoviment(this);
        _playerRotator = new PlayerRotator(this);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _playerMoviment = new PlayerMoviment(this);
            GetComponent<NavMeshAgent>().enabled = false;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            _playerMoviment = new NavmeshPlayerMoviment(this);
        }

        _playerMoviment.Tick();
        _playerRotator.Tick();
    }
}
