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


    //public abstract void OnHit(KLD_Zombie _zombie, int _damage);
    public abstract void OnHit(XL_IDamageable _damageable, int _damage);


    protected float spreadAngle = 0f;
    RaycastHit hit;
    protected XL_IDamageable hitZombie;
    protected int bulletsToShoot = 0;
    protected Vector3 newDir = Vector3.zero;
    protected bool noMuzzle;

    public virtual void Shoot(KLD_WeaponSO _weaponSO, Vector3 _canonPos, Vector3 _dir, LayerMask _layerMask)
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

            if (Physics.Raycast(_canonPos, newDir, out hit, _weaponSO.GetCurAttributes().range, _layerMask))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    hitZombie = hit.collider.gameObject.GetComponent<XL_IDamageable>();
                    if (hitZombie != null)
                    {
                        OnHit(hitZombie, _weaponSO.GetCurAttributes().bulletDamage);
                    }
                    //Debug.Log(Vector3.Dot(newDir, (hit.point - _canonPos)));
                    //if (Vector3.Dot(newDir, (hit.point - _canonPos)) < 0f)
                    //{
                    //hit.point = _canonPos;
                    //Debug.Log("changed canon pos");
                    //}
                    DrawShot(_canonPos, hit.point, _weaponSO, true, true, noMuzzle);
                }
                else
                {
                    DrawShot(_canonPos, hit.point, _weaponSO, true, false, noMuzzle);
                }
            }
            else
            {
                DrawShot(_canonPos, _canonPos + (newDir.normalized * _weaponSO.GetCurAttributes().range), _weaponSO, false, false, noMuzzle);
            }
        }
    }

    GameObject lineRenderer;
    LineRenderer curLr;
    protected Vector3 shotDirection;
    protected Quaternion shotAngles;

    protected void DrawShot(Vector3 startPos, Vector3 impactPos, KLD_WeaponSO _weaponSO, bool impacted, bool impactedEnemy, bool _noMuzzle)
    {
        //Debug.DrawLine(startPos, impactPos, raysColor, 0.2f);

        shotDirection = impactPos - startPos;

        shotAngles = Quaternion.LookRotation(shotDirection, Vector3.up);

        if (!noMuzzle)
        { XL_Pooler.instance.PopPosRot(_weaponSO.weaponName + "_muzzle", startPos, shotAngles); }

        //XL_Pooler.instance.PopPosition(_weaponSO.weaponName + "_muzzle", startPos);

        if (impacted)
        {
            //XL_Pooler.instance.PopPosition(_weaponSO.weaponName + "_impact", impactPos);
            shotDirection = -shotDirection;
            shotAngles = Quaternion.LookRotation(shotDirection, Vector3.up);
            if (impactedEnemy)
            {
                XL_Pooler.instance.PopPosRot(_weaponSO.weaponName + "_impact", impactPos, shotAngles);
            }
            else
            {
                XL_Pooler.instance.PopPosRot(_weaponSO.weaponName + "_wallImpact", impactPos, shotAngles);
            }
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
        XL_Pooler.instance.CreatePool(weaponSO.weaponName + "_wallImpact", weaponSO.wallImpactFX, weaponSO.fxPoolSize);
        XL_Pooler.instance.CreatePool(weaponSO.weaponName + "_lineRenderer", weaponSO.lineRendererFX, weaponSO.fxPoolSize);
    }
}
