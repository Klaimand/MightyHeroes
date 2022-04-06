using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KLD_Bullet : ScriptableObject
{
    public abstract void OnHit(KLD_Zombie _zombie, int _damage);

    public virtual void ProcessShootHits(RaycastHit[] _hits)
    {
        Debug.LogError("NOT IMPLEMENTED HITS PROCESSING");
    }
}
