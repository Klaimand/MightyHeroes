using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Swarmers : XL_Enemy
{
    [SerializeField] private float attackRange;

    private void Update()
    {
        Move();
    }

    public override void Die()
    {
        pooler.DePop("Swarmer", transform.gameObject);
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
            agent.destination = targetedPlayer.position;
            if ((transform.position - targetedPlayer.position).magnitude < attackRange) 
            {
                Attack();
            }
        }
            
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    //if alerted state is only triggered when entering player's "alert range"
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Alert();
        }
    }
}
