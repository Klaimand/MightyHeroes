using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissingHealthWeight", menuName = "KLD/Aim Weights/Missing Health", order = 1)]
public class KLD_MissingHealthWeight : KLD_AimWeight
{
    public override float CalculateWeight(KLD_ZombieAttributes _attributes, KLD_PlayerAttributes _playerAttributes)
    {
        return _attributes.maxHealth - _attributes.health;
    }
}
