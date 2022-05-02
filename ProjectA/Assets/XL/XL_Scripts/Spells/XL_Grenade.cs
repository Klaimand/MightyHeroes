using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Grenade : MonoBehaviour
{
    private int explosionDamage;
    private float explosionRadius;
    [SerializeField] private LayerMask layers;

    public void SetValue(int damage, float radius) 
    {
        explosionDamage = damage;
        explosionRadius = radius;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Collision")) 
        {
            XL_Pooler.instance.PopPosition("Explosion", transform.position).GetComponent<XL_Explosion>().StartExplosion(explosionDamage, explosionRadius, 0.1f);
            XL_Pooler.instance.DePop("BlastGrenade", gameObject);
        }
        
        
    }
}
