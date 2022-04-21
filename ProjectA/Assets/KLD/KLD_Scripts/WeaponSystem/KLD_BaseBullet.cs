using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newBaseBullet", menuName = "KLD/Weapons/Bullets/Base Bullet", order = 0)]
public class KLD_BaseBullet : KLD_Bullet
{
    public override void OnHit(KLD_Zombie _zombie, int _damage)
    {
        //do damage to zombie

        //throw new System.NotImplementedException();
    }
}
