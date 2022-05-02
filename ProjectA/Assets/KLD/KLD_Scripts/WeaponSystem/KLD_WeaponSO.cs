using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#region Weapon Enums

public enum WeaponType
{
    ASSAULT_RIFLE,
    SMG,
    SHOTGUN,
    SNIPER
}

public enum WeaponRarity
{
    BASE,
    WOW,
    AMAZING,
    MEGA
}

public enum ReloadType
{
    MAGAZINE,
    BULLET_PER_BULLET
}

#endregion

[CreateAssetMenu(fileName = "newWeapon", menuName = "KLD/Weapons/New Weapon", order = 0)]
public class KLD_WeaponSO : ScriptableObject
{
    public string weaponName = "newWeapon";

    [TextArea(3, 8)]
    public string description = "This is a new weapon.";

    public WeaponType type = WeaponType.ASSAULT_RIFLE;

    public WeaponRarity rarity = WeaponRarity.BASE;

    public ReloadType reloadType = ReloadType.MAGAZINE;

    public KLD_Bullet bullet;

    [PropertyRange(0, "maxLevel")]
    public int level = 0;

    public int experience = 0;

    [ListDrawerSettings(ListElementLabelName = "displayedName")]
    public WeaponAttributes[] weaponAttributes;

    [HideInInspector] public float shootDelay = 0f;

    [Header("Animations"), Space(10)]
    public GameObject weaponHolder;
    //public AnimationClip holdingAnim;
    //public AnimationClip aimingAnim;
    //public AnimationClip reloadAnim;
    //public AnimationClip shootAnim;
    public AnimatorOverrideController animatorOverrideController;
    public float shootAnimDelay = 0.3f;
    [Range(0f, 30f)] public float angleOffset = 0f;

    [Header("FX"), Space(10)]
    public GameObject muzzleFlashFX;
    public GameObject lineRendererFX;
    public GameObject impactFX;
    public GameObject wallImpactFX;

    [Header("Pooling")]
    public int fxPoolSize = 9;


    int maxLevel = 0;

    public WeaponAttributes GetCurAttributes()
    {
        return weaponAttributes[level];
    }

    void OnValidate()
    {
        ValidateValues();
    }

    public void ValidateValues()
    {
        for (int i = 0; i < weaponAttributes.Length; i++)
        {
            weaponAttributes[i].displayedName = "Level " + i;
            weaponAttributes[i].isBPBReloading = reloadType == ReloadType.BULLET_PER_BULLET;
            weaponAttributes[i].OnValidate();
        }
        ProtectValues();
        shootDelay = RpmToDelay(GetCurAttributes().rpm);
    }

    void ProtectValues()
    {
        maxLevel = weaponAttributes.Length - 1;
        if (level > maxLevel) level = maxLevel;
        if (experience < 0) experience = 0;
    }

    float RpmToDelay(int rpm)
    {
        return 1f / ((float)rpm / 60f);
    }

    public void PoolBullets()
    {
        bullet.PoolBullets(this);
    }
}