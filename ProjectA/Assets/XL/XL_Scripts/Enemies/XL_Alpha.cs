using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Alpha : XL_Enemy
{

    [SerializeField] private float summonCooldown;
    [SerializeField] private int nbEnemiesSummoned;
    [SerializeField] private float summonDistance;
    [SerializeField] private float projectilespeed;
    [SerializeField] private float fireRate;

    public override void Alert()
    {
        StartCoroutine(SummonCoroutine(summonCooldown));
        isAlerted = true;
    }

    public override void Attack()
    {
        projectile = XL_Pooler.instance.PopPosition("AlphaProjectile", shootDirection + transform.position);
        projectile.GetComponent<XL_Projectile>().Initialize();
        projectile.GetComponent<Rigidbody>().velocity = shootDirection * projectilespeed;
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
            XL_GameManager.instance.AddEnemy(XL_Pooler.instance.PopPosition("Swarmer", summonPosition + transform.position).GetComponent<XL_Enemy>().GetZombieAttributes());
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
            shootDirection = (other.transform.position - transform.position).normalized;
            Attack();
        }
    }
}
