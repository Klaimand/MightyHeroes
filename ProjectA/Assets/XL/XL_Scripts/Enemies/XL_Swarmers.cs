using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Swarmers : XL_Enemy
{
    [SerializeField] private float attackRange;
    [SerializeField] private float attackWidth;
    private List<GameObject> playersHit = new List<GameObject>();

    [SerializeField] private int nbRaycast;

    private void Update()
    {
        Move();
        DebugRaycast();
    }

    private void DebugRaycast()
    {
        for (int i = 0; i < nbRaycast + 1; i++)
        {
            Debug.DrawRay(transform.position - transform.right * 0.5f + (transform.right / nbRaycast) * i + transform.up * 0.5f, attackRange * transform.forward * 1.2f, Color.red);
        }
    }

    public override void Die()
    {
        base.Die();
        XL_Pooler.instance.DePop("Swarmer", transform.gameObject);
        StopAllCoroutines();
    }

    public override void Alert()
    {
        StartCoroutine(FindTargetedPlayerCoroutine(targetedPlayerUpdateRate));
        isAlerted = true;
    }

    public override void Move()
    {
        if (isAlerted) 
        {
            if (targetedPlayer != null) 
            {
                agent.destination = targetedPlayer.position;
                if ((transform.position - targetedPlayer.position).magnitude < attackRange)
                {
                    agent.isStopped = true;
                    if (canAttack) Attack();
                }
            } 
        }
            
    }

    public override void Attack()
    {
        //Debug.Log("Attacking");
        StartCoroutine(AttackCoroutine(1));
        StartCoroutine(AttackCooldownCoroutine(2));
    }


    private RaycastHit[] hits;
    IEnumerator AttackCoroutine(float t) 
    {
        transform.LookAt(targetedPlayer);
        playersHit.Clear();
        yield return new WaitForSeconds(t);

        /*hits = Physics.BoxCastAll(transform.forward * attackRange * 0.6f, new Vector3(attackWidth/2, 0.2f, attackRange * 0.6f), transform.forward, Quaternion.identity, layer);
        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log("Player " + hits[0].transform.name + "has been hit");
        } */

        for (int i = 0; i < nbRaycast + 1; i++)
        {
            hits = Physics.RaycastAll(transform.position - transform.right * 0.5f+ (transform.right / nbRaycast) * i + transform.up * 0.5f, attackRange * transform.forward * 1.2f); //Can add a layer but I didn't make it work
            for (int j = 0; j < hits.Length; j++)
            {
                if (hits[j].transform.CompareTag("Player") && !playersHit.Contains(hits[j].transform.gameObject)) 
                {
                    //Debug.Log(transform.name + " damaged " + hits[j].transform.name);
                    playersHit.Add(hits[j].transform.gameObject);
                    if(hits[j].transform != null) hits[j].transform.GetComponent<XL_IDamageable>().TakeDamage(damage);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player")) 
        {
            Debug.Log("Player was attacked");
        }
    }
}
