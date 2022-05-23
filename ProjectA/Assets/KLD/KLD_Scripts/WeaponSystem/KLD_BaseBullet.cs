using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newBaseBullet", menuName = "KLD/Weapons/Bullets/Base Bullet", order = 0)]
public class KLD_BaseBullet : KLD_Bullet
{
    public override void OnHit(XL_IDamageable _damageable, float _damage)
    {
        //do damage to zombie
        _damageable.TakeDamage(_damage);
        //throw new System.NotImplementedException();
    }
}
