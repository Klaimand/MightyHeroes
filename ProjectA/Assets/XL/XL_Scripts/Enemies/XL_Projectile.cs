using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Projectile : MonoBehaviour
{
    private int damage;
    private float radius;



    public void Initialize(int damage, float radius)
    {
        this.damage = damage;
        this.radius = radius;
    }


    XL_IDamageable objectHit;
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Enemy"))
        {
            objectHit = collision.transform.GetComponent<XL_IDamageable>();
            //if (objectHit != null)
            //{
            //    objectHit.TakeDamage(damage);
            //}
            //Debug.Log("Depoped because of collision");
            StopAllCoroutines();
            XL_Pooler.instance.PopPosition("Summoner_Explosion", transform.position).GetComponent<XL_Explosion>().StartExplosion(damage, radius, 0.1f);
            XL_Pooler.instance.DePop("Summoner_Projectile", transform.gameObject);
        }

    }
}
