using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Explosion : MonoBehaviour
{
    private float radius;
    private int damage;
    //protected List<XL_IDamageable> objectsInExplosionRange = new List<XL_IDamageable>();
    private XL_IDamageable damageableObject;

    public void StartExplosion(int damage, float radius, float explosionTime)
    {
        this.damage = damage;
        this.radius = radius;
        StartCoroutine(ExplosionCoroutine(explosionTime));
    }

    private Collider[] hitColliders;
    IEnumerator ExplosionCoroutine(float t)
    {
        yield return new WaitForSeconds(t);

        hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders) 
        {
            Debug.Log(hitCollider.name + "is in sphere");

            if ((damageableObject = hitCollider.GetComponent<XL_IDamageable>()) != null) 
            {
                Debug.Log(hitCollider.name + "can be damaged");
                damageableObject.TakeDamage(damage);
            }
        }
    }
}
