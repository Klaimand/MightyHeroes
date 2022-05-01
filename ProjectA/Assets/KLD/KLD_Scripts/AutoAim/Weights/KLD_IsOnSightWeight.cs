using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IsOnSightWeight", menuName = "KLD/Aim Weights/Is On Sight", order = 2)]
public class KLD_IsOnSightWeight : KLD_AimWeight
{
    [SerializeField] LayerMask sightBlockers;
    [SerializeField] float onSightScore = 0f;
    [SerializeField] float outOfSightScore = -5000f;

    [SerializeField, Space(10)] float heightOffset = 1.8f;
    [SerializeField] bool drawRays = false;

    Vector3 playerPos = Vector3.zero;
    Vector3 zombiePos = Vector3.zero;

    public override float CalculateWeight(KLD_ZombieAttributes _attributes, KLD_PlayerAttributes _playerAttributes)
    {
        playerPos = _playerAttributes.transform.position;
        playerPos.y += heightOffset;

        zombiePos = _attributes.transform.position;
        zombiePos.y += heightOffset;

        RaycastHit hit;

        if (drawRays) Debug.DrawRay(playerPos, zombiePos - playerPos, Color.blue); //this should not be there

        if (Physics.Raycast(playerPos, zombiePos - playerPos, out hit,
        Vector3.Distance(playerPos, zombiePos), sightBlockers))
        {
            if (hit.collider.transform == _attributes.transform)
            {
                return onSightScore;
            }
        }
        return outOfSightScore;
    }
}