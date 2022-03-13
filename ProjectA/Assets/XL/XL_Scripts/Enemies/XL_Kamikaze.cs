using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Kamikaze : XL_Enemy
{
    [SerializeField] protected MeshRenderer[] meshRenderers;

    [SerializeField] protected List<XL_IDamageable> objectsInExplosionRange = new List<XL_IDamageable>();
    [SerializeField] protected int explosionDamage;
    [SerializeField] protected float explosionRange;
    [SerializeField] protected float detonationTime;

    private void Update()
    {
        Move();
    }

    public override void Alert()
    {
        StartCoroutine(FindTargetedPlayerCoroutine(targetedPlayerUpdateRate));
        isAlerted = true;
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        StartCoroutine(ExplosionCoroutine(detonationTime));
    }

    IEnumerator ExplosionCoroutine(float t) 
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].enabled = false;
        }

        yield return new WaitForSeconds(t);

        foreach(XL_IDamageable objects in objectsInExplosionRange)
        {
            objects.TakeDamage(explosionDamage);
        }

    

    }

    public override void Move()
    {
        if (isAlerted)
        {
            agent.destination = targetedPlayer.position;
            if ((transform.position - targetedPlayer.position).magnitude < explosionRange * 0.5f)
            {
                Die();
            }
        }
    }

    private XL_IDamageable damageableObject;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " has entered explosion range");
        damageableObject = other.transform.GetComponent<XL_IDamageable>();
        if (damageableObject != null)
        {
            objectsInExplosionRange.Add(damageableObject);
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name + " has exited explosion range");
        damageableObject = other.transform.GetComponent<XL_IDamageable>();
        if (objectsInExplosionRange.Contains(damageableObject))
        {
            objectsInExplosionRange.Remove(damageableObject);
        }
    }
}
