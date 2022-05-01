using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DistanceWeight", menuName = "KLD/Aim Weights/Distance", order = 0)]
public class KLD_DistanceWeight : KLD_AimWeight
{
    [SerializeField] float maxDistance = 20f;
    [SerializeField] float scoreOnMaxDistance = -5000f;

    float distance = 0f;

    public override float CalculateWeight(KLD_ZombieAttributes _attributes, KLD_PlayerAttributes _playerAttributes)
    {
        distance = (_attributes.transform.position - _playerAttributes.transform.position).magnitude;

        return distance < maxDistance ? -distance : scoreOnMaxDistance;

        //return -((_attributes.transform.position - _playerAttributes.transform.position).magnitude);
    }
}
