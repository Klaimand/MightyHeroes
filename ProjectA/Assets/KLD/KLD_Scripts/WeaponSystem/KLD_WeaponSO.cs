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

    [PropertyRange(0, "maxLevel")]
    public int level = 0;

    public int experience = 0;

    [ListDrawerSettings(ListElementLabelName = "displayedName")]
    public WeaponAttributes[] weaponAttributes;

    [Header("Animations"), Space(10)]
    public AnimationClip holdingAnim;
    public AnimationClip aimingAnim;
    public AnimationClip reloadAnim;
    public AnimationClip shootAnim;

    [Header("FX"), Space(10)]
    public GameObject muzzleFlashFX;
    public GameObject lineRendererFX;
    public GameObject impactFX;


    int maxLevel = 0;

    public WeaponAttributes GetCurWeaponAttributes()
    {
        return weaponAttributes[level];
    }

    void OnValidate()
    {
        for (int i = 0; i < weaponAttributes.Length; i++)
        {
            weaponAttributes[i].displayedName = "Level " + i;
            weaponAttributes[i].OnValidate();
        }
        ProtectValues();
    }

    void ProtectValues()
    {
        maxLevel = weaponAttributes.Length - 1;
        if (level > maxLevel) level = maxLevel;
        if (experience < 0) experience = 0;
    }
}