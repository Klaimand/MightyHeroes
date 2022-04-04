using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Alpha : XL_Enemy
{

    [SerializeField] private float summonCooldown;
    [SerializeField] private int nbEnemiesSummoned;
    [SerializeField] private float summonDistance;
    [SerializeField] private float projectilespeed;
    [SerializeField] private float projectileTravelTime;
    [SerializeField] private float fireRate;

    public override void Alert()
    {
        StartCoroutine(SummonCoroutine(summonCooldown));
        isAlerted = true;
    }

    private const float g = 9.81f; //gravity
    private const float h = 0.5f; //starting height
    private float distance; //shoot
    private float[] velocity;

    public override void Attack()
    {
        projectile = XL_Pooler.instance.PopPosition("AlphaProjectile", shootDirection.normalized + transform.position);
        projectile.GetComponent<XL_Projectile>().Initialize();
        //projectile.GetComponent<Rigidbody>().velocity = shootDirection * projectilespeed;
        velocity = XL_Utilities.GetVelocity(h, distance - 1, projectileTravelTime); //-1 because the projectile is shoot 1 meter in front of the enemy
        Debug.Log("vy = " + velocity[1] + "\n" +
            "vx = " + velocity[0] + "\n" +
            "multiplier x = " + (shootDirection.x / distance) + "\n" +
            "multiplier z = " + (shootDirection.z / distance) + "\n" +
            "vxx = " + velocity[0] * (shootDirection.x / distance) + "\n" +
            "vxz = " + velocity[0] * (shootDirection.z / distance) + "\n" +
            "distance = " + distance);
        projectile.GetComponent<Rigidbody>().velocity = new Vector3(velocity[0] * (shootDirection.x / distance), velocity[1], velocity[0] * (shootDirection.z / distance));
    }

    private Vector3 summonPosition;
    private float angleOffset;
    IEnumerator SummonCoroutine(float t) 
    {
        yield return new WaitForSeconds(t);

        angleOffset = 360 / nbEnemiesSummoned;
        for (int i = 0; i < nbEnemiesSummoned; i++) 
        {
            summonPosition = Quaternion.Euler(0, angleOffset * i, 0) * transform.forward * summonDistance;
            XL_GameManager.instance.AddEnemyAttributes(XL_Pooler.instance.PopPosition("Swarmer", summonPosition + transform.position).GetComponent<XL_Enemy>().GetZombieAttributes());
        }
        StartCoroutine(SummonCoroutine(t));
    }

    public override void Die()
    {
        base.Die();
        StopAllCoroutines();
        Debug.Log("Alpha has died");
        XL_Pooler.instance.DePop("Alpha", transform.gameObject);
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
    }


    private Vector3 shootDirection;
    private GameObject projectile;
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player") && canAttack)
        {
            //Debug.Log("Alpha is attacking");
            StartCoroutine(AttackCooldownCoroutine(fireRate));
            shootDirection = (other.transform.position - transform.position);
            distance = (new Vector3(other.transform.position.x, 0, other.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).magnitude;
            Attack();
        }
    }
}