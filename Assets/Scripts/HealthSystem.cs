using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;


    [SerializeField] private int health = 100;

     private int healthMax;


    private void Awake()
    {
        healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }
        OnDamaged?.Invoke(this, EventArgs.Empty);
        if (health == 0)
        {
            Die();
        }

        Debug.Log(health);
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }


    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

}