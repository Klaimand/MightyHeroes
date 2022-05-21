using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XL_WeaponDetailsMenu : MonoBehaviour
{
    public static XL_WeaponDetailsMenu instance;

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

    [Header("Upgrade")]
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private TMP_Text upgradeText;

    public int selectedWeapon = 0;

    private void Awake()
    {
        instance = this;
    }

    public void Select(int idx)
    {
        foreach (XL_UIWeaponInfo wi in weaponInfos)
        {
            wi.Deactivate();
        }

        selectedWeapon = idx;

        CheckHasUpgrade();
        DisplayWeaponInfo();

        weaponInfos[idx].Activate();
    }

    private void CheckHasUpgrade()
    {
        if (weaponInfos[selectedWeapon].GetLevel() + 1 >= weaponInfos[selectedWeapon].weaponAttributes.weaponAttributes.Length) upgradeButton.SetActive(false);
        else upgradeButton.SetActive(true);
    }

    private void DisplayWeaponInfo()
    {
        //Initialise Text
        weaponInfos[selectedWeapon].DisplayLevel();
        damageText.text = weaponInfos[selectedWeapon].GetDamage().ToString();
        rpmText.text = weaponInfos[selectedWeapon].GetRPM().ToString();
        magazineSizeText.text = weaponInfos[selectedWeapon].GetMagazine().ToString();
        upgradeText.text = weaponInfos[selectedWeapon].weaponAttributes.weaponAttributes[weaponInfos[selectedWeapon].GetLevel()].experienceToReach.ToString(); //Aled

        //Initialise Bar
        damageXScaler.localScale = new Vector3(weaponInfos[selectedWeapon].GetDamage() / scalerDamageMax, damageXScaler.localScale.y, damageXScaler.localScale.z);
        rpmXScaler.localScale = new Vector3(weaponInfos[selectedWeapon].GetRPM() / scalerRPMMax, rpmXScaler.localScale.y, rpmXScaler.localScale.z);
        magazineSizeXScaler.localScale = new Vector3(weaponInfos[selectedWeapon].GetMagazine() / scalerMagazineSizeMax, magazineSizeXScaler.localScale.y, magazineSizeXScaler.localScale.z);

    }

    public void UpgradeWeapon() 
    {
        Debug.Log(weaponInfos[selectedWeapon].weaponAttributes.weaponAttributes[weaponInfos[selectedWeapon].GetLevel()].experienceToReach);
        if (PlayerPrefs.GetInt("SoftCurrency") > weaponInfos[selectedWeapon].weaponAttributes.weaponAttributes[weaponInfos[selectedWeapon].GetLevel()].experienceToReach) 
        {
            PlayerPrefs.SetInt(weaponInfos[selectedWeapon].weaponAttributes.weaponName, weaponInfos[selectedWeapon].GetLevel() + 1);
            PlayerPrefs.SetInt("SoftCurrency", PlayerPrefs.GetInt("SoftCurrency") - weaponInfos[selectedWeapon].weaponAttributes.weaponAttributes[weaponInfos[selectedWeapon].GetLevel()].experienceToReach);
            XL_MainMenu.instance.RefreshTopOverlay();
            weaponInfos[selectedWeapon].weaponAttributes.level++;

            CheckHasUpgrade();
            DisplayWeaponInfo();
        }
    }
}
