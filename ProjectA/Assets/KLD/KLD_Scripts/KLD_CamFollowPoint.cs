using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class KLD_CamFollowPoint : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] KLD_PlayerAim playerAim;
    [SerializeField] KLD_PlayerShoot playerShoot;
    [SerializeField] KLD_TouchInputs inputs;

    [Header("Parameters")]
    [SerializeField] Transform player;
    //[SerializeField] Transform smoothTargetPos;
    [Range(0f, 10f)]
    [SerializeField] float distanceFromPlayerOnAim = 4f;

    float t;

    [SerializeField] float acceleration = 2f;
    [SerializeField] float deceleration = 2f;

    Vector3 playerToTargetPos;

    [SerializeField] CinemachineVirtualCamera cam1;
    [SerializeField] CinemachineVirtualCamera cam2;


    void Update()
    {
        playerToTargetPos = playerAim.GetTargetPos() - player.position;
        playerToTargetPos.Normalize();

        if (inputs.IsJoystickPressed(1) && playerShoot.isAiming)
        {
            transform.position = player.position + playerToTargetPos * distanceFromPlayerOnAim;

            cam1.Priority = 0;
            cam2.Priority = 10;
        }
        else
        {
            transform.position = player.position;

            cam1.Priority = 10;
            cam2.Priority = 0;
        }
    }
}
