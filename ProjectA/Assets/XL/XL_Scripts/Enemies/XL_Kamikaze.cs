using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Kamikaze : XL_Enemy
{
    [SerializeField] protected List<XL_IDamageable> objectsInExplosionRange = new List<XL_IDamageable>();
    [SerializeField] protected int explosionDamage;
    [SerializeField] protected float explosionRange;
    [SerializeField] protected float detonationTime;
    [SerializeField] protected float chargingAnimationTime;
    [SerializeField] protected ParticleSystem[] chargeParticles;
    [SerializeField] protected ParticleSystem[] chillParticles;
    protected bool isCharged;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator.ResetTrigger("Charging");
        isCharged = false;
    }

    private void OnEnable()
    {
        foreach (ParticleSystem ps in chillParticles)
        {
            ps.Play();
        }
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
        foreach (ParticleSystem ps in chargeParticles)
        {
            ps.Stop();
        }
        StopAllCoroutines();
        XL_Pooler.instance.PopPosition("Kamikaze_Explosion", transform.position).GetComponent<XL_Explosion>().StartExplosion(explosionDamage, explosionRange, detonationTime);
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
            if ((transform.position - targetedPlayer.position).magnitude < explosionRange * 2 && !isCharged)
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
        isCharged = true;
        yield return new WaitForSeconds(chargingAnimationTime);
        foreach (ParticleSystem ps in chillParticles)
        {
            ps.Stop();
        }
        foreach (ParticleSystem ps in chargeParticles)
        {
            ps.Play();
        }
        animator.SetBool("Attacking", true);
        agent.isStopped = false;
        
    }
}
