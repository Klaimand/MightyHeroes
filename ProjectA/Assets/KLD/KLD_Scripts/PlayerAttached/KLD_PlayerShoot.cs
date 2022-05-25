using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;

public class KLD_PlayerShoot : MonoBehaviour
{
    //private references
    KLD_PlayerAim playerAim;

    [Header("Public References")]
    Transform canon;
    [SerializeField] Text ammoText;
    [SerializeField] KLD_TouchInputs touchInputs;
    [SerializeField] Button reloadButton;
    [SerializeField] Animator reloadFlames;
    [SerializeField] Animator animator;
    [SerializeField] KLD_PlayerController controller;
    [SerializeField] XL_Characters character;

    //[Header("Weapon"), Space(10)]
    //[InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    KLD_WeaponSO weapon;

    [Header("Shooting Parameters"), Space(10)]
    [SerializeField] float zombieVerticalOffset = 1.5f;
    [SerializeField] LayerMask layerMask;

    bool canReload = false;

    //private local fields
    Vector3 impactPosition = Vector3.zero;
    Vector3 shootDirection = Vector3.zero;
    Vector3 selectedZombiePos = Vector3.zero;
    int bulletsToShoot = 0;
    int missingBullets = 0;
    Coroutine curReloadCoroutine;

    //weapon delays
    float curShootDelay = 0f;
    float curBurstDelay = 0f;

    //weapon data
    int curBullets = 0;

    //animation
    //[HideInInspector] public bool isReloading;
    [ReadOnly] public bool isReloading = false;
    [ReadOnly] public bool isAiming = false;
    [ReadOnly] public bool isShooting;
    [ReadOnly] public bool isUsingUltimate = false;
    //[ReadOnly] public bool canUseUltimateWhenReloading = false;


    public enum WeaponState
    {
        HOLD,
        AIMING,
        SHOOTING,
        RELOADING,
        USING_ULTI,
        RELOADING_BPB
    }
    WeaponState weaponState = WeaponState.HOLD;


    //weapon mesh and anims references
    [Header("Weapon Mesh/Anims"), Space(10)]
    [SerializeField] Transform weaponHolderParent;
    [SerializeField] RigBuilder rigBuilder;
    [SerializeField] TwoBoneIKConstraint leftHandIK;
    [SerializeField] TwoBoneIKConstraint rightHandIK;

    //sayuri active spell
    bool isInSayuriUlt = false;
    bool canFreeShoot = false;
    [SerializeField, ReadOnly] float rpmRatio = 1f;

    [Header("Sound")]
    [SerializeField] Vector2Int minMaxEnemyKilledToSound = new Vector2Int(2, 4);
    int curEnemyToSound;
    int curEnemyKilledSinceLastSound;

    void Awake()
    {
        playerAim = GetComponent<KLD_PlayerAim>();
        //InitWeaponMesh();
    }

    // Start is called before the first frame update
    void Start()
    {
        //weapon.ValidateValues();
        //curBullets = weapon.GetCurAttributes().magazineSize;
        //playerAim.targetPosAngleOffset = weapon.angleOffset;
        //StartCoroutine(DelayedStart());
        //UpdateUI();
    }

    public void Init(KLD_WeaponSO _weapon, int _weaponLevel)
    {
        weapon = _weapon;
        weapon.level = _weaponLevel;

        InitWeaponMesh();

        weapon.ValidateValues();
        curBullets = weapon.GetCurAttributes().magazineSize;
        playerAim.targetPosAngleOffset = weapon.angleOffset;

        animator.SetFloat("reloadSpeedScale",
        (weapon.weaponAttributes[0].reloadSpeed / weapon.GetCurAttributes().reloadSpeed)
        );

        StartCoroutine(DelayedStart());
        UpdateUI();
    }

    IEnumerator DelayedStart()
    {
        yield return null;
        weapon.PoolBullets();
    }

    void OnEnable()
    {
        touchInputs.onReloadButton += Reload;

        KLD_EventsManager.instance.onEnemyKill += SoundOnEnemyKilled;
    }

    void OnDisable()
    {
        touchInputs.onReloadButton -= Reload;

        KLD_EventsManager.instance.onEnemyKill -= SoundOnEnemyKilled;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessIsAimingAndShooting();

        AnimateWeaponState();

        canReload = !isReloading && curBullets < weapon.GetCurAttributes().magazineSize;
        reloadButton.interactable = canReload;
        reloadFlames.SetBool("enabled", isReloading);

        if (isShooting && curBullets > 0 && !isReloading && !isUsingUltimate)
        {
            if (curShootDelay > weapon.shootDelay * (1f / rpmRatio))
            {
                StartCoroutine(ShootCoroutine());
                curShootDelay = 0f;
            }
        }
        else if (!isReloading && curBullets <= 0)
        {
            Reload();
        }

        //controller.SetSpeed(
        //    ((weaponState == WeaponState.AIMING || weaponState == WeaponState.SHOOTING) ?
        //    character.GetCharacterSpeed() * weapon.GetCurAttributes().aimSpeedMultiplier :
        //    character.GetCharacterSpeed()) / 3.34f
        //);

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

                if (!canFreeShoot)
                {
                    curBullets--;
                    UpdateUI();
                }

                if (i < bulletsToShoot - 1)
                    yield return new WaitForSeconds(weapon.GetCurAttributes().timeBetweenBullets);
            }
        }
    }

    KLD_ZombieAttributes selectedZombie;
    Vector3 canonOffset;

    void DoShot()
    {
        selectedZombie = playerAim.GetSelectedZombie();
        //if (playerAim.GetSelectedZombie() != null)
        if (selectedZombie != null && selectedZombie.transform != null)
        {
            //selectedZombiePos = playerAim.GetSelectedZombie().transform.position;
            selectedZombiePos = selectedZombie.transform.position;

            selectedZombiePos.y += zombieVerticalOffset;

            shootDirection = selectedZombiePos - canon.position;

            if (Vector3.Dot(canon.forward, shootDirection) < 0f || shootDirection.sqrMagnitude < 0.5f)
            {
                shootDirection = canon.forward;
                shootDirection.y = -0.35f;
                canonOffset = -canon.forward * 1f;
            }
            else
            {
                canonOffset = Vector3.zero;
            }
        }
        else
        {
            shootDirection = canon.forward;
            shootDirection.y = 0f;
        }
        weapon.bullet.Shoot(weapon, canon.position + canonOffset, shootDirection, layerMask);
        animator.Play("(2) Weapon_ShootingAnim", 2);
        KLD_ScreenShakes.instance.StartShake(weapon.shakeLenght, weapon.shakePower, weapon.shakeFrequency);
    }

    public void Reload()
    {
        if (canReload)
        {
            KLD_AudioManager.Instance.PlayCharacterSound("Reload", 1);
            curReloadCoroutine = StartCoroutine(ReloadCoroutine());
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
            for (int i = 0; i < missingBullets; i++)
            {
                curBullets++;
                UpdateUI();
                yield return new WaitForSeconds(weapon.GetCurAttributes().reloadSpeed);
            }
        }

        isReloading = false;
        UpdateUI();

        curReloadCoroutine = null;
    }

    void UpdateUI()
    {
        ammoText.text = $"{curBullets} / {weapon.GetCurAttributes().magazineSize}";
    }

    float curShootAnimDelay = 0f;
    void ProcessIsAimingAndShooting()
    {

        isAiming = (playerAim.GetIsPressingAimJoystick() && playerAim.GetSelectedZombie() != null) ||
         (playerAim.GetIsPressingAimJoystick() && playerAim.GetInputAimVector().sqrMagnitude > 0.1f);

        if (isAiming && (playerAim.GetSelectedZombie() == null || playerAim.GetSelectedZombie()?.transform == null) && playerAim.GetInputAimVector().sqrMagnitude < 0.1f)
        {
            isAiming = false;
        }

        if (!isAiming || isReloading)
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

        controller.SetSpeedRatio(isAiming || isShooting ? weapon.GetCurAttributes().aimSpeedMultiplier : 1f);
    }

    public void UseUltimate(float _time)
    {
        isUsingUltimate = true;
        StartCoroutine(IUseUltimate(_time));
    }

    IEnumerator IUseUltimate(float _time)
    {
        yield return new WaitForSeconds(_time);
        isUsingUltimate = false;
    }

    void AnimateWeaponState()
    {
        if (isUsingUltimate)
        {
            weaponState = WeaponState.USING_ULTI;
        }
        else if (isReloading)
        {
            weaponState = (weapon.reloadType == ReloadType.MAGAZINE ? WeaponState.RELOADING : WeaponState.RELOADING_BPB);
        }
        else if (!playerAim.GetIsPressingAimJoystick())
        {
            weaponState = WeaponState.HOLD;
        }
        else if (isAiming && isShooting && curBullets > 0)
        {
            weaponState = WeaponState.SHOOTING;
        }
        //else if (isAiming && (!isShooting || isShooting && curBullets == 0))
        else if (isAiming)
        {
            weaponState = WeaponState.AIMING;
        }
        else
        {
            weaponState = WeaponState.HOLD;
        }
        animator.SetInteger("weaponState", (int)weaponState);
    }



    public WeaponState GetWeaponState()
    {
        return weaponState;
    }

    public void SetCharacterMeshComponents(
    Animator _animator,
    Transform _weaponHolderParent,
    RigBuilder _rigBuilder,
    TwoBoneIKConstraint _leftHandIK,
    TwoBoneIKConstraint _rightHandIK
    )
    {
        animator = _animator;
        weaponHolderParent = _weaponHolderParent;
        rigBuilder = _rigBuilder;
        leftHandIK = _leftHandIK;
        rightHandIK = _rightHandIK;
    }

    public float GetWeaponUltChargeOnKill()
    {
        return weapon.GetCurAttributes().activePointsPerKill;
    }

    void SoundOnEnemyKilled(Enemy _enemy)
    {
        curEnemyKilledSinceLastSound++;
        if (curEnemyKilledSinceLastSound >= curEnemyToSound)
        {
            curEnemyKilledSinceLastSound = 0;
            curEnemyToSound = Random.Range(minMaxEnemyKilledToSound.x, minMaxEnemyKilledToSound.y);

            KLD_AudioManager.Instance.PlayCharacterSound("Kill", 0);
        }
    }


    #region Sayuri Ult

    Coroutine curSayuriUltCoroutine;

    public void LaunchSayuriUlt(float _duration, float _rpmRatio)
    {
        if (curSayuriUltCoroutine != null)
        {
            StopCoroutine(curSayuriUltCoroutine);
        }
        rpmRatio = _rpmRatio;
        canFreeShoot = true;
        isInSayuriUlt = true;


        if (curReloadCoroutine != null)
        {
            StopCoroutine(curReloadCoroutine);

            if (weapon.reloadType == ReloadType.MAGAZINE)
            {
                animator.Play("(3) RELOAD ANIMATION FULL", 1, 1f);
            }
            else
            {
                animator.Play("(5) RELOAD POSITION", 1, 1f);
                animator.Play("(5) Weapon_Shotgun_ReloadAnimAloop", 2, 1f);
            }

            isReloading = false;
            curReloadCoroutine = null;
        }

        curBullets = weapon.GetCurAttributes().magazineSize;
        UpdateUI();

        curSayuriUltCoroutine = StartCoroutine(ILaunchSayuriUlt(_duration, _rpmRatio));
    }

    IEnumerator ILaunchSayuriUlt(float _duration, float _rpmRatio)
    {
        yield return new WaitForSeconds(_duration);

        rpmRatio = 1f;
        canFreeShoot = false;
        isInSayuriUlt = false;
        curSayuriUltCoroutine = null;
    }


    #endregion

    #region Weapon Mesh and anims Initialization

    GameObject instantiedWH;
    KLD_WeaponHolder weaponHolder;

    void InitWeaponMesh()
    {
        if (weaponHolderParent.childCount > 3) { Destroy(weaponHolderParent.GetChild(3).gameObject); }

        //leftHandIK.enabled = false;
        //rightHandIK.enabled = false;

        instantiedWH = Instantiate(weapon.weaponHolder, Vector3.zero, Quaternion.identity, weaponHolderParent);

        //instantiedWH.name = "WeaponHolder";
        instantiedWH.name = weapon.weaponHolder.name;

        instantiedWH.transform.localPosition = weapon.weaponHolder.transform.position;
        instantiedWH.transform.localRotation = weapon.weaponHolder.transform.rotation;


        weaponHolder = instantiedWH.GetComponent<KLD_WeaponHolder>();

        rightHandIK.data.target = weaponHolder.leftHandle;
        leftHandIK.data.target = weaponHolder.rightHandle;

        canon = weaponHolder.canon;


        animator.runtimeAnimatorController = weapon.animatorOverrideController;


        animator.enabled = false;
        animator.enabled = true;

        rigBuilder.Build();

        animator.enabled = false;
        animator.enabled = true;

        //leftHandIK.enabled = true;
        //rightHandIK.enabled = true;

    }

    [ContextMenu("Rebuild Rig")]
    public void RebuildRig()
    {
        animator.enabled = false;
        animator.enabled = true;

        rigBuilder.Build();

        animator.enabled = false;
        animator.enabled = true;
    }

    #endregion
}
