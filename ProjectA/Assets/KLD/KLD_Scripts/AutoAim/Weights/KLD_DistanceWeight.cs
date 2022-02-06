using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDistanceWeight", menuName = "KLD/Aim Weights/Distance", order = 0)]
public class KLD_DistanceWeight : KLD_AimWeight
{
    public override float CalculateWeight(KLD_ZombieAttributes _attributes, Transform _player)
    {
        return (_attributes.transform.position - _player.position).sqrMagnitude;
    }
}
