using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    ASSAULT_RIFLE,
    SMG,
    SHOTGUN,
    SNIPER
}

public enum WeaponRarity
{
    COMMON,
    WOW,
    AMAZING,
    MEGA
}

[CreateAssetMenu(fileName = "newWeapon", menuName = "KLD/Weapons/New Weapon", order = 0)]
public class KLD_Weapon : ScriptableObject
{
    public string weaponName = "newWeapon";

    public WeaponType type = WeaponType.ASSAULT_RIFLE;

    [TextArea(3, 8)]
    public string description = "This is a new weapon.";

    //PASSIVE

    public WeaponRarity rarity = WeaponRarity.COMMON;

    public int bpm = 120;

    public int bulletDamage = 10;






}
