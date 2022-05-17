using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KLD_TriggerOnDamage : MonoBehaviour, XL_IDamageable
{
    [SerializeField] float health = 400f;
    [SerializeField] UnityEvent onDamageTake;
    [SerializeField] UnityEvent onDeath;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health >= 0f)
        {
            onDamageTake.Invoke();
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        onDeath.Invoke();
    }

    private void OnValidate()
    {
        if (health < 1f) health = 1f;
    }

}
