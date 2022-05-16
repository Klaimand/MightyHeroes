using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XL_WeaponDetailsMenu : MonoBehaviour
{
    [Header("Weapon Info")]
    [SerializeField] private XL_UIWeaponInfo[] weaponInfos;

    [Header("Damage Bar")]
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private Transform damageXScaler;
    [SerializeField] private float scalerDamageMax;

    [Header("RPM Bar")]
    [SerializeField] private TMP_Text rpmText;
    [SerializeField] private Transform rpmXScaler;
    [SerializeField] private float scalerRPMMax;

    [Header("Magazine Size Bar")]
    [SerializeField] private TMP_Text magazineSizeText;
    [SerializeField] private Transform magazineSizeXScaler;
    [SerializeField] private float scalerMagazineSizeMax;

    [Header("Upgrade Button")]
    [SerializeField] private TMP_Text upgradeText;

    public int selectedWeapon;

    public void Select(int idx)
    {
        foreach (XL_UIWeaponInfo wi in weaponInfos)
        {
            wi.Deactivate();
        }

        selectedWeapon = idx;

        DisplayWeaponInfo(idx);

        weaponInfos[idx].Activate();
    }

    private void DisplayWeaponInfo(int idx)
    {
        //Initialise Text
        weaponInfos[idx].DisplayLevel();
        damageText.text = weaponInfos[idx].GetDamage().ToString();
        rpmText.text = weaponInfos[idx].GetRPM().ToString();
        magazineSizeText.text = weaponInfos[idx].GetMagazine().ToString();
        upgradeText.text = weaponInfos[idx].weaponAttributes.weaponAttributes[weaponInfos[idx].GetLevel()].experienceToReach.ToString(); //Aled

        //Initialise Bar
        damageXScaler.localScale = new Vector3(weaponInfos[idx].GetDamage() / scalerDamageMax, damageXScaler.localScale.y, damageXScaler.localScale.z);
        rpmXScaler.localScale = new Vector3(weaponInfos[idx].GetRPM() / scalerRPMMax, rpmXScaler.localScale.y, rpmXScaler.localScale.z);
        magazineSizeXScaler.localScale = new Vector3(weaponInfos[idx].GetMagazine() / scalerMagazineSizeMax, magazineSizeXScaler.localScale.y, magazineSizeXScaler.localScale.z);

    }

    public void UpgradeWeapon() 
    {
        if (PlayerPrefs.GetInt("SoftCurrency") > weaponInfos[selectedWeapon].weaponAttributes.weaponAttributes[weaponInfos[selectedWeapon].GetLevel()].experienceToReach) 
        {
            PlayerPrefs.SetInt(weaponInfos[selectedWeapon].weaponAttributes.weaponName, weaponInfos[selectedWeapon].weaponAttributes.level + 1);
            weaponInfos[selectedWeapon].weaponAttributes.level++;
            PlayerPrefs.SetInt("SoftCurrency", PlayerPrefs.GetInt("SoftCurrency") - weaponInfos[selectedWeapon].weaponAttributes.weaponAttributes[weaponInfos[selectedWeapon].GetLevel()].experienceToReach);
            Debug.Log("Currency left : " + PlayerPrefs.GetInt("SoftCurrency"));
            DisplayWeaponInfo(selectedWeapon);
        }
    }
}
