using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KLD_Bullet : ScriptableObject
{
    public abstract void OnHit(KLD_Zombie _zombie, int _damage);

    float randomSpread = 0f;
    RaycastHit hit;

    public virtual Vector3 Shoot(KLD_WeaponSO _weaponSO, Vector3 _canonPos, Vector3 _dir, LayerMask _layerMask)
    {
        randomSpread = Random.Range(-_weaponSO.GetCurAttributes().spread, _weaponSO.GetCurAttributes().spread);

        _dir = Quaternion.Euler(0f, randomSpread, 0f) * _dir;

        if (Physics.Raycast(_canonPos, _dir, out hit, _weaponSO.GetCurAttributes().range, _layerMask))
        {
            return hit.point;
        }
        else
        {
            return _canonPos + (_dir.normalized * _weaponSO.GetCurAttributes().range);
        }

        //return _canonPos + (_dir.normalized * _weaponSO.GetCurAttributes().range);
    }
}
