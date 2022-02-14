using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Alpha : XL_Enemy
{

    [SerializeField] private float summonCooldown;

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

    Vector2 summonPosition;

    IEnumerator SummonCoroutine(float t) 
    {
        yield return new WaitForSeconds(t);

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i != 0 && j != 0) 
                {
                    pooler.PopPosition("Swarmer",new Vector3(i*3, 0, j*3) + transform.position);
                    Debug.Log("Swarmer Poped");
                }
            }
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
