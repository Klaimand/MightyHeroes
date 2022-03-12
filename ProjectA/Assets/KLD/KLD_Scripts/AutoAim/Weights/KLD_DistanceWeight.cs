using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DistanceWeight", menuName = "KLD/Aim Weights/Distance", order = 0)]
public class KLD_DistanceWeight : KLD_AimWeight
{
    public override float CalculateWeight(KLD_ZombieAttributes _attributes, KLD_PlayerAttributes _playerAttributes)
    {
        return -((_attributes.transform.position - _playerAttributes.transform.position).magnitude);
    }
}