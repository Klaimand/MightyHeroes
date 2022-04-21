using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KLD_Bullet : ScriptableObject
{
    [Header("FX")]
    //[SerializeField] GameObject muzzleFlashFX;
    //[SerializeField] GameObject lineRendererFX;
    //[SerializeField] GameObject impactFX;
    [SerializeField] Color raysColor;


    public abstract void OnHit(KLD_Zombie _zombie, int _damage);


    float spreadAngle = 0f;
    RaycastHit hit;
    KLD_Zombie hitZombie;
    int bulletsToShoot = 0;
    Vector3 newDir = Vector3.zero;

    public virtual void Shoot(KLD_WeaponSO _weaponSO, Vector3 _canonPos, Vector3 _dir, LayerMask _layerMask)
    {
        bulletsToShoot = _weaponSO.GetCurAttributes().isBuckshot ?
        _weaponSO.GetCurAttributes().bulletsPerShot : 1;

        for (int i = 0; i < bulletsToShoot; i++)
        {
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

            if (Physics.Raycast(_canonPos, newDir, out hit, _weaponSO.GetCurAttributes().range, _layerMask))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    hitZombie = hit.collider.gameObject.GetComponent<KLD_Zombie>();
                    if (hitZombie != null)
                    {
                        OnHit(hitZombie, _weaponSO.GetCurAttributes().bulletDamage);
                    }
                }
                DrawShot(_canonPos, hit.point, _weaponSO, true);
            }
            else
            {
                DrawShot(_canonPos, _canonPos + (newDir.normalized * _weaponSO.GetCurAttributes().range), _weaponSO, false);
            }
        }
    }

    GameObject lineRenderer;
    LineRenderer curLr;

    void DrawShot(Vector3 startPos, Vector3 impactPos, KLD_WeaponSO _weaponSO, bool impacted)
    {
        //Debug.DrawLine(startPos, impactPos, raysColor, 0.2f);
        XL_Pooler.instance.PopPosition(_weaponSO.weaponName + "_muzzle", startPos);
        if (impacted)
        {
            XL_Pooler.instance.PopPosition(_weaponSO.weaponName + "_impact", impactPos);
        }
        lineRenderer = XL_Pooler.instance.PopPosition(_weaponSO.weaponName + "_lineRenderer", startPos);

        for (int i = 0; i < 2; i++)
        {
            curLr = lineRenderer.transform.GetChild(i).GetComponent<LineRenderer>();
            curLr.SetPosition(0, startPos);
            curLr.SetPosition(1, impactPos);
        }
    }

    public void PoolBullets(KLD_WeaponSO weaponSO)
    {
        XL_Pooler.instance.CreatePool(weaponSO.weaponName + "_muzzle", weaponSO.muzzleFlashFX, weaponSO.fxPoolSize);
        XL_Pooler.instance.CreatePool(weaponSO.weaponName + "_impact", weaponSO.impactFX, weaponSO.fxPoolSize);
        XL_Pooler.instance.CreatePool(weaponSO.weaponName + "_lineRenderer", weaponSO.lineRendererFX, weaponSO.fxPoolSize);
    }
}
