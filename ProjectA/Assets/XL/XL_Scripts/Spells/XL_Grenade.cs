using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Grenade : MonoBehaviour
{
    private int explosionDamage;
    private float explosionRadius;
    [SerializeField] private LayerMask layers;

    [SerializeField] float shakeLenght = 1.3f;
    [SerializeField] float shakePower = 7f;
    [SerializeField] float shakeFrequency = 2.5f;

    public void SetValue(int damage, float radius)
    {
        explosionDamage = damage;
        explosionRadius = radius;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Collision")) 
        if (!collision.transform.CompareTag("Player"))
        {
            KLD_ScreenShakes.instance.StartShake(shakeLenght, shakePower, shakeFrequency);

            XL_Pooler.instance.PopPosition("Explosion", transform.position).GetComponent<XL_Explosion>().StartExplosion(explosionDamage, explosionRadius, 0.1f);
            XL_Pooler.instance.DePop("BlastGrenade", gameObject);
        }


    }
}
