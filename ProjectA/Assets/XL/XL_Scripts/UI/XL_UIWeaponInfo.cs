using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[System.Serializable]
public class XL_UIWeaponInfo
{
    //CURRENTLY WORKS ONLY FOR LEVEL 0 

    public GameObject weaponUI;
    public TMP_Text weaponLvlUI;
    public KLD_WeaponSO weaponAttributes;

    public void Activate()
    {
        weaponUI.SetActive(true);
    }

    public void Deactivate()
    {
        weaponUI.SetActive(false);
    }

    public void DisplayLevel()
    {
        weaponLvlUI.text = (GetLevel() + 1).ToString();
    }

    public int GetDamage()
    {
        return weaponAttributes.weaponAttributes[GetLevel()].bulletDamage;
    }

    public int GetRPM()
    {
        return weaponAttributes.weaponAttributes[GetLevel()].rpm;
    }

    public int GetMagazine()
    {
        return weaponAttributes.weaponAttributes[GetLevel()].magazineSize;
    }

    public int GetLevel()
    {
        return weaponAttributes.level;
    }
}
