using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, ITakeHits
{
    [SerializeField] private int _maxHealth;
    public int Health { get; private set; }

    private void OnEnable()
    {
        Health = _maxHealth;
    }

    public void TakeHit(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Die();
        }
        else
        {
            HandleNonLethalHit();
        }
    }

    private void HandleNonLethalHit()
    {
        Debug.Log("Take hit");
    }

    private void Die()
    {
        Debug.Log("Dead");
    }
}