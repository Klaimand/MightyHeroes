using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Grenade : MonoBehaviour
{
    private int explosionDamage;
    private float explosionRadius;

    public void SetValue(int damage, float radius) 
    {
        explosionDamage = damage;
        explosionRadius = radius;
    }

    private void OnCollisionEnter(Collision collision)
    {
        XL_Pooler.instance.PopPosition("Explosion", transform.position).GetComponent<XL_Explosion>().StartExplosion(explosionDamage, explosionRadius, 0.1f);
        XL_Pooler.instance.DePop("BlastGrenade", gameObject);
    }
}
