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
        base.Die();
        StopAllCoroutines();
        XL_Pooler.instance.PopPosition("Explosion", transform.position).GetComponent<XL_Explosion>().StartExplosion(explosionDamage, explosionRange, detonationTime);
        XL_Pooler.instance.DePop("Kamikaze", transform.gameObject);
    }

    public override void Move()
    {
        if (isAlerted && targetedPlayer != null)
        {
            agent.destination = targetedPlayer.position;
            if ((transform.position - targetedPlayer.position).magnitude < explosionRange * 2)
            {
                agent.speed = speed * 2;
            }
            if ((transform.position - targetedPlayer.position).magnitude < explosionRange * 0.5f)
            {
                Die();
            }
        }
    }
}
