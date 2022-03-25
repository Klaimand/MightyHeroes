using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_PlayerAttributes : MonoBehaviour, XL_IDamageable
{
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
    }

    public void Die()
    {
        XL_GameManager.instance.RemovePlayer(transform.gameObject);
        transform.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 1) 
        {
            Die();
        }
    }
}
