using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_CamFollowPoint : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] KLD_PlayerAim playerAim;
    [SerializeField] KLD_PlayerShoot playerShoot;

    [Header("Parameters")]
    [SerializeField] Transform player;
    //[SerializeField] Transform smoothTargetPos;
    [Range(0f, 10f)]
    [SerializeField] float distanceFromPlayerOnAim = 4f;

    Vector3 playerToTargetPos;


    void Update()
    {
        playerToTargetPos = playerAim.GetTargetPos() - player.position;
        playerToTargetPos.Normalize();


        if (playerShoot.isAiming)
        {
            transform.position = player.position + playerToTargetPos * distanceFromPlayerOnAim;
        }
        else
        {
            transform.position = player.position;
        }
    }
}
