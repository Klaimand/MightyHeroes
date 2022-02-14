using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Kamikaze : XL_Enemy
{
    [SerializeField] protected MeshRenderer[] meshRenderers;

    [SerializeField] protected GameObject explosionVFX; // GameObject for now, but might change
    [SerializeField] protected GameObject[] playersInExplosionRange;
    [SerializeField] protected float explosionDamage;
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
        explosionVFX.transform.position = transform.position; // Will change, just a "placeholder"
        explosionVFX.SetActive(true);   // Will change, just a "placeholder"

        yield return new WaitForSeconds(t);

        for (int i = 0; i < playersInExplosionRange.Length; i++)
        {
            Debug.Log("Player " + i + " is taking damage from explosion");
            //playersInExplosionRange[i].TakeDamage(explosionDamage);
        }
        //pooler.DePop("Kamikaze", transform.gameObject);
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
}
