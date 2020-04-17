using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EntityAnimator : MonoBehaviour
{
    private Animator _animator;
    private Entity _entity;
    private static readonly int Dead = Animator.StringToHash("Dead");


    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _entity = GetComponent<Entity>();
        _entity.OnDied += Die;
        // _entity.OnDied += () => _animator.SetBool("Die", true); alternativa com classe anonima
    }

    private void Die()
    {
        Debug.Log("Start dead animation");
        _animator.SetBool(Dead, true);
    }
}
