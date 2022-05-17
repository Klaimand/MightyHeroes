using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPiercingBullet", menuName = "KLD/Weapons/Bullets/Piercing Bullet", order = 1)]
public class KLD_PiercingBullet : KLD_Bullet
{

    [SerializeField] int piercingPower = 3;
    int curPiercedEnemies = 0;
    int lastPiercedEnemyIndex = 0;

    //float spreadAngle = 0f;
    RaycastHit[] hits;
    //XL_IDamageable hitZombie;
    //int bulletsToShoot = 0;
    //Vector3 newDir = Vector3.zero;
    //bool noMuzzle;
    bool isLastImpact;

    public override void Shoot(KLD_WeaponSO _weaponSO, Vector3 _canonPos, Vector3 _dir, LayerMask _layerMask)
    {

        bulletsToShoot = _weaponSO.GetCurAttributes().isBuckshot ?
        _weaponSO.GetCurAttributes().bulletsPerShot : 1;

        for (int i = 0; i < bulletsToShoot; i++)
        {
            noMuzzle = _weaponSO.GetCurAttributes().isBuckshot && i != bulletsToShoot / 2;

            if (_weaponSO.GetCurAttributes().isBuckshot)
            {
                spreadAngle = Mathf.Lerp(
                    -_weaponSO.GetCurAttributes().spread,
                    _weaponSO.GetCurAttributes().spread,
                    (float)i / (float)(bulletsToShoot - 1));
            }
            else
            {
                spreadAngle = Random.Range(-_weaponSO.GetCurAttributes().spread, _weaponSO.GetCurAttributes().spread);
            }

            newDir = Quaternion.Euler(0f, spreadAngle, 0f) * _dir;


            hits = Physics.RaycastAll(_canonPos, newDir, _weaponSO.GetCurAttributes().range, _layerMask);

            System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

            if (hits != null && hits.Length > 0)
            {
                isLastImpact = false;
                curPiercedEnemies = 0;
                for (int y = 0; y < hits.Length; y++)
                {
                    if (hits[y].collider.gameObject.CompareTag("Enemy"))
                    {
                        hitZombie = hits[y].collider.gameObject.GetComponent<XL_IDamageable>();
                        if (hitZombie != null)
                        {
                            OnHit(hitZombie, _weaponSO.GetCurAttributes().bulletDamage);
                        }
                        if (y < hits.Length - 1)
                        {
                            DrawImpact(_canonPos, hits[y].point, _weaponSO, true);
                        }
                        lastPiercedEnemyIndex = curPiercedEnemies;
                        curPiercedEnemies++;
                    }
                    else
                    {
                        if (y < hits.Length - 1)
                        {
                            DrawImpact(_canonPos, hits[y].point, _weaponSO, false);
                        }
                        lastPiercedEnemyIndex = y;
                        curPiercedEnemies += 999;
                    }

                    if (curPiercedEnemies >= piercingPower)
                    {
                        isLastImpact = true;
                        //lastPiercedEnemyIndex = piercingPower - 1;
                        break;
                    }
                }

                if (isLastImpact)
                {
                    DrawShot(_canonPos, hits[lastPiercedEnemyIndex].point, _weaponSO,
                    true,
                    hits[lastPiercedEnemyIndex].collider.gameObject.CompareTag("Enemy"),
                    noMuzzle
                    );
                }
                else
                {
                    DrawShot(_canonPos, _canonPos + (newDir.normalized * _weaponSO.GetCurAttributes().range),
                    _weaponSO, false, false, noMuzzle);
                }
            }
            else
            {
                DrawShot(_canonPos, _canonPos + (newDir.normalized * _weaponSO.GetCurAttributes().range), _weaponSO, false, false, noMuzzle);
            }
        }
    }



    //Vector3 shotDirection;
    //Quaternion shotAngles;

    void DrawImpact(Vector3 _startPos, Vector3 _impactPos, KLD_WeaponSO _weaponSO, bool _impactedEnemy)
    {
        shotDirection = _impactPos - _startPos;
        shotDirection = -shotDirection;
        shotAngles = Quaternion.LookRotation(shotDirection, Vector3.up);
        if (_impactedEnemy)
        {
            XL_Pooler.instance.PopPosRot(_weaponSO.weaponName + "_impact", _impactPos, shotAngles);
        }
        else
        {
            XL_Pooler.instance.PopPosRot(_weaponSO.weaponName + "_wallImpact", _impactPos, shotAngles);
        }
    }





    public override void OnHit(XL_IDamageable _damageable, int _damage)
    {
        _damageable.TakeDamage(_damage);
    }
}
