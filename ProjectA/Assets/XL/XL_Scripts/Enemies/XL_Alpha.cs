using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Alpha : XL_Enemy
{

    [SerializeField] private float summonCooldown;
    [SerializeField] private int nbEnemiesSummoned;
    [SerializeField] private float summonDistance;

    public override void Alert()
    {
        StartCoroutine(SummonCoroutine(summonCooldown));
        isAlerted = true;
    }

    public override void Attack()
    {
        //Increase enemy spawns during waves
        throw new System.NotImplementedException();
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
            pooler.PopPosition("Swarmer", summonPosition + transform.position);
        }
        StartCoroutine(SummonCoroutine(t));
    }

    public override void Die()
    {
        StopAllCoroutines();
        throw new System.NotImplementedException();
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
    }
}
