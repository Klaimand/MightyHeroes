using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class KLD_PlayerShoot : MonoBehaviour
{
    //private references
    KLD_PlayerAim playerAim;

    [Header("Public References")]
    [SerializeField] Transform canon;
    [SerializeField] Text ammoText;

    [Header("Weapon"), Space(10)]
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [SerializeField] KLD_WeaponSO weapon;

    [Header("Shooting Parameters"), Space(10)]
    [SerializeField] float zombieVerticalOffset = 1.5f;
    [SerializeField] LayerMask layerMask;

    public bool isReloading = false;

    //private local fields
    Vector3 impactPosition = Vector3.zero;
    Vector3 shootDirection = Vector3.zero;
    Vector3 selectedZombiePos = Vector3.zero;
    int bulletsToShoot = 0;
    int missingBullets = 0;

    //weapon delays
    float curShootDelay = 0f;
    float curBurstDelay = 0f;

    //weapon data
    int curBullets = 0;

    void Awake()
    {
        playerAim = GetComponent<KLD_PlayerAim>();
    }

    // Start is called before the first frame update
    void Start()
    {
        weapon.ValidateValues();
        curBullets = weapon.GetCurAttributes().magazineSize;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAim.isShooting && curBullets > 0)
        {
            if (curShootDelay > weapon.shootDelay)
            {
                StartCoroutine(ShootCoroutine());
                curShootDelay = 0f;
            }
        }

        curShootDelay += Time.deltaTime;
        curBurstDelay += Time.deltaTime;
    }

    IEnumerator ShootCoroutine()
    {
        bulletsToShoot = weapon.GetCurAttributes().isBuckshot ?
        1 : weapon.GetCurAttributes().bulletsPerShot;

        for (int i = 0; i < bulletsToShoot; i++)
        {
            if (curBullets > 0)
            {
                DoShot();

                curBullets--;

                UpdateUI();

                if (i < bulletsToShoot - 1)
                    yield return new WaitForSeconds(weapon.GetCurAttributes().timeBetweenBullets);
            }
        }
    }

    void DoShot()
    {
        if (playerAim.GetSelectedZombie() != null)
        {
            selectedZombiePos = playerAim.GetSelectedZombie().transform.position;

            selectedZombiePos.y += zombieVerticalOffset;

            shootDirection = selectedZombiePos - canon.position;
        }
        else
        {
            shootDirection = canon.forward;
        }
        weapon.bullet.Shoot(weapon, canon.position, shootDirection, layerMask);
    }

    public void Reload()
    {
        if (!isReloading && curBullets < weapon.GetCurAttributes().magazineSize)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }


    IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        if (weapon.reloadType == ReloadType.MAGAZINE)
        {
            yield return new WaitForSeconds(weapon.GetCurAttributes().reloadSpeed);
            curBullets = weapon.GetCurAttributes().magazineSize;
        }
        else if (weapon.reloadType == ReloadType.BULLET_PER_BULLET)
        {
            missingBullets = weapon.GetCurAttributes().magazineSize - curBullets;
            for (int i = 0; i < missingBullets + 1; i++)
            {
                curBullets++;
                yield return new WaitForSeconds(weapon.GetCurAttributes().reloadSpeed);
            }
        }
        isReloading = false;
    }

    void UpdateUI()
    {
        ammoText.text = $"{curBullets} / {weapon.GetCurAttributes().magazineSize}";
    }
}
