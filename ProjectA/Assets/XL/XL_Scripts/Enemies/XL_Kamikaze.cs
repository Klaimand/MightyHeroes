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
    [SerializeField] protected float chargingAnimationTime;
    protected bool isCharged;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator.ResetTrigger("Charging");
        isCharged = false;
    }

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
        ResetAnimator();
        XL_Pooler.instance.DePop("Kamikaze", transform.gameObject);
    }

    private void ResetAnimator()
    {
        animator.ResetTrigger("Charging");
        animator.SetBool("Attacking", false);
    }

    public override void Move()
    {
        if (isAlerted && targetedPlayer != null)
        {
            agent.destination = targetedPlayer.position;
            if ((transform.position - targetedPlayer.position).magnitude < explosionRange * 2)
            {
                animator.SetBool("Charging", true);
                if (!isCharged) StartCoroutine(ChargingCoroutine());
                agent.speed = speed * 2;
            }
            if ((transform.position - targetedPlayer.position).magnitude < explosionRange * 0.5f)
            {
                Die();
            }
        }
    }

    IEnumerator ChargingCoroutine() 
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(chargingAnimationTime);
        animator.SetBool("Attacking", true);
        agent.isStopped = false;
        isCharged = true;
    }
}
