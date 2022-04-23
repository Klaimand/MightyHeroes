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
    [SerializeField] Button reloadButton;

    [Header("Weapon"), Space(10)]
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [SerializeField] KLD_WeaponSO weapon;

    [Header("Shooting Parameters"), Space(10)]
    [SerializeField] float zombieVerticalOffset = 1.5f;
    [SerializeField] LayerMask layerMask;

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

    //animation
    //[HideInInspector] public bool isReloading;
    [SerializeField] Animator animator;
    [HideInInspector] public bool isReloading = false;
    [HideInInspector] public bool isAiming = false;
    [HideInInspector] public bool isShooting;

    public enum WeaponState
    {
        HOLD,
        AIMING,
        SHOOTING,
        RELOADING
    }
    WeaponState weaponState = WeaponState.HOLD;



    void Awake()
    {
        playerAim = GetComponent<KLD_PlayerAim>();
    }

    // Start is called before the first frame update
    void Start()
    {
        weapon.ValidateValues();
        curBullets = weapon.GetCurAttributes().magazineSize;
        StartCoroutine(DelayedStart());
        UpdateUI();
    }

    IEnumerator DelayedStart()
    {
        yield return null;
        weapon.PoolBullets();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessIsAimingAndShooting();

        AnimateWeaponState();

        if (isShooting && curBullets > 0)
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

    float curShootAnimDelay = 0f;
    void ProcessIsAimingAndShooting()
    {
        isAiming = playerAim.GetIsPressingAimJoystick() && playerAim.GetSelectedZombie() != null ||
         playerAim.GetIsPressingAimJoystick() && playerAim.GetInputAimVector().sqrMagnitude > 0.1f;

        if (!isAiming)
        {
            isShooting = false;
            curShootAnimDelay = 0f;
        }
        else if (isAiming && !isShooting)
        {
            curShootAnimDelay += Time.deltaTime;
            if (curShootAnimDelay > weapon.shootAnimDelay)
            {
                isShooting = true;
            }
        }
    }


    void AnimateWeaponState()
    {
        if (isReloading)
        {
            weaponState = WeaponState.RELOADING;
        }
        else if (!playerAim.GetIsPressingAimJoystick())
        {
            weaponState = WeaponState.HOLD;
        }
        else if (isAiming && !isShooting)
        {
            weaponState = WeaponState.AIMING;
        }
        else if (isAiming && isShooting)
        {
            weaponState = WeaponState.SHOOTING;
        }
        animator.SetInteger("weaponState", (int)weaponState);
    }




    public WeaponState GetWeaponState()
    {
        return weaponState;
    }
}
