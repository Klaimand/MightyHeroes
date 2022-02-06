using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KLD_AimWeight : ScriptableObject
{
    public abstract float CalculateWeight(KLD_ZombieAttributes _attributes, Transform _player);
}
