using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_HealZone : MonoBehaviour
{
    [SerializeField] float duration = 5f;
    float curDuration = 0f;
    [SerializeField] float tickDuration = 0.2f;
    float curTickDuration = 0f;
    [SerializeField] int healingPerTick = 20;
    [SerializeField] float radius = 3f;

    XL_IDamageable playerDamageable;
    Transform playerTransform;

    Vector3 centerToPlayer = Vector3.zero;

    void OnEnable()
    {
        if (playerDamageable == null)
        {
            playerDamageable = XL_GameManager.instance.players[0].GetComponent<XL_IDamageable>();
        }
        if (playerTransform == null)
        {
            playerTransform = XL_GameManager.instance.players[0].transform;
        }

        curDuration = 0f;
        curTickDuration = 0f;
    }

    void Update()
    {
        if (curDuration < duration)
        {
            if (curTickDuration > tickDuration)
            {
                curTickDuration = 0f;
                //centerToPlayer = playerTransform.position - transform.position;

                //if (centerToPlayer.magnitude < radius)
                //{
                playerDamageable.TakeDamage(-healingPerTick);
                //}
            }
        }
        else
        {
            XL_Pooler.instance.DePop("SimoHeal", gameObject);
        }

        curDuration += Time.deltaTime;
        curTickDuration += Time.deltaTime;
    }
}
