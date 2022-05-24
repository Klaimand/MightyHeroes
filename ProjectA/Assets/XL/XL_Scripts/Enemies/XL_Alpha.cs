using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class XL_Alpha : XL_Enemy
{

    [SerializeField] private float summonCooldown;
    [SerializeField] private int nbEnemiesSummoned;
    [SerializeField] private float summonDistance;

    [Header("Missile Launchers")]
    [SerializeField] private Transform cannon;
    [SerializeField] private Transform[] cannonEdgesPos;
    private int missileLaunched = 0;

    [Header("Projectile")]
    [SerializeField] private float projectilespeed;
    [SerializeField] private float projectileTravelTime;
    [SerializeField] private float fireRate;
    [SerializeField] private int projectileDamage;
    [SerializeField] private float projectileExplosionRadius;
    private GameObject impactZone;

    [Header("VFX")]
    GameObject vfx;
    GameObject deathVFX; //Second GameObject because some vfx might still be active when the summoner dies

    [SerializeField] private Animator animator;

    [SerializeField] private LookAtConstraint lookAtConstraint;
    private ConstraintSource source = new ConstraintSource();

    bool dead = false;

    protected override void Start()
    {
        base.Start();
        source.sourceTransform = XL_GameManager.instance.players[0].transform;
        source.weight = 1;
        lookAtConstraint.AddSource(source);
    }

    public override void Alert()
    {
        StartCoroutine(SummonCoroutine(summonCooldown));
        isAlerted = true;
    }

    private const float g = 9.81f; //gravity
    private float h = 5f; //starting height
    private float distance; //shoot
    private float[] velocity;

    public override void Attack()
    {
        h = cannonEdgesPos[missileLaunched % cannonEdgesPos.Length].position.y;
        projectile = XL_Pooler.instance.PopPosition("Summoner_Projectile", cannonEdgesPos[missileLaunched % cannonEdgesPos.Length].position);
        projectile.GetComponent<XL_Projectile>().Initialize(projectileDamage, projectileExplosionRadius);
        //projectile.GetComponent<Rigidbody>().velocity = shootDirection * projectilespeed;
        velocity = XL_Utilities.GetVelocity(h, distance, projectileTravelTime);
        projectile.GetComponent<Rigidbody>().velocity = new Vector3(velocity[0] * (shootDirection.x / distance), velocity[1], velocity[0] * (shootDirection.z / distance));
        missileLaunched++;

        selfAudioManager.PlaySound("Shooting");
    }

    private int j;
    private Vector3 summonPosition;
    IEnumerator SummonCoroutine(float t)
    {
        j = 0;
        yield return new WaitForSeconds(t - 0.7f);

        animator.SetBool("Summoning", true);
        vfx = XL_Pooler.instance.PopPosition("SummoningChargeVFX", transform.position + transform.up * 4);

        yield return new WaitForSeconds(0.7f);

        animator.SetBool("Summoning", false);
        animator.SetBool("Spawning", true);

        XL_Pooler.instance.DePop("SummoningChargeVFX", vfx);


        while (j < nbEnemiesSummoned)
        {

            yield return new WaitForSeconds(0.85f);
            selfAudioManager.PlaySound("Spawning");
            vfx = XL_Pooler.instance.PopPosition("SpawningVFX", transform.position + transform.up * 4);

            summonPosition = transform.forward * summonDistance;
            //XL_GameManager.instance.AddEnemyAttributes(XL_Pooler.instance.PopPosition("Swarmer", summonPosition + transform.position).GetComponent<XL_Enemy>().GetZombieAttributes());
            XL_Pooler.instance.PopPosition("Summoned_Swarmer", summonPosition + transform.position)
                .transform.LookAt(XL_GameManager.instance.players[0].transform.position);



            XL_Pooler.instance.DelayedDePop(0.8f, "SpawningVFX", vfx);
            j++;
        }
        animator.SetBool("Spawning", false);
        StartCoroutine(SummonCoroutine(t));
    }

    public override void Die()
    {
        if (dead) return;
        dead = true;
        //base.Die();
        KLD_EventsManager.instance.InvokeEnemyKill(Enemy.ALPHA);
        StopAllCoroutines();
        canAttack = false;

        deathVFX = XL_Pooler.instance.PopPosition("Summoner_DeathVFX", transform.position);
        selfAudioManager.PlaySound("Death", 1.45f);
        XL_Pooler.instance.DelayedDePop(2, "Summoner_DeathVFX", deathVFX);

        XL_Pooler.instance.DelayedDePop(1.6f, "Alpha", transform.gameObject);
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage(float damage)
    {
        if (!dead)
        {
            StartCoroutine(TakeDamageAnimationCoroutine());
            base.TakeDamage(damage);
        }

    }

    IEnumerator TakeDamageAnimationCoroutine()
    {
        animator.SetBool("Hit", true);
        yield return new WaitForSeconds(0.4f);
        animator.SetBool("Hit", false);
    }

    private Vector3 shootDirection;
    private GameObject projectile;
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player") && canAttack)
        {
            //Debug.Log("Alpha is attacking");
            impactZone = XL_Pooler.instance.PopPosition("Summoner_ImpactZone", other.transform.position);
            StartCoroutine(AttackCooldownCoroutine(fireRate));
            shootDirection = (other.transform.position - cannonEdgesPos[missileLaunched % cannonEdgesPos.Length].position);
            distance = (new Vector3(other.transform.position.x, 0, other.transform.position.z) - new Vector3(cannonEdgesPos[missileLaunched % (cannonEdgesPos.Length - 1)].position.x, 0, cannonEdgesPos[missileLaunched % (cannonEdgesPos.Length - 1)].position.z)).magnitude;
            XL_Pooler.instance.DelayedDePop(projectileTravelTime, "Summoner_ImpactZone", impactZone);
            Attack();
        }
    }
}
