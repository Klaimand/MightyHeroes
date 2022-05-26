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

    [Header("Unlock")]
    [SerializeField] private GameObject unlockButton;
    [SerializeField] private TMP_Text unlockText;

    [Header("Upgrade")]
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private TMP_Text upgradeText;

    [Header("Select")]
    [SerializeField] private GameObject selectButton;

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

        RefreshUI();
    }

    private void RefreshUI()
    {
        CheckHasUpgrade();
        DisplayWeaponInfo();

        weaponInfos[selectedWeapon].Activate(PlayerPrefs.GetInt(weaponInfos[selectedWeapon].weaponAttributes.weaponName + "Unlocked"));
    }

    private void CheckHasUpgrade()
    {
        if (PlayerPrefs.GetInt(weaponInfos[selectedWeapon].weaponAttributes.weaponName + "Unlocked") == 0)
        {
            upgradeButton.SetActive(false);
            unlockButton.SetActive(true);
            selectButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(false);
            selectButton.SetActive(true);
        }
        if (weaponInfos[selectedWeapon].GetLevel() + 1 >= weaponInfos[selectedWeapon].weaponAttributes.weaponAttributes.Length) upgradeButton.SetActive(false);
        else upgradeButton.SetActive(true);
    }

    private void DisplayWeaponInfo()
    {
        //Initialise Text
        weaponInfos[selectedWeapon].DisplayLevel();
        damageText.text = weaponInfos[selectedWeapon].GetDamage().ToString("N0");
        rpmText.text = weaponInfos[selectedWeapon].GetRPM().ToString("N0");
        magazineSizeText.text = weaponInfos[selectedWeapon].GetMagazine().ToString("N0");
        unlockText.text = weaponInfos[selectedWeapon].weaponAttributes.unlockSoftCurrency.ToString("N0");
        upgradeText.text = weaponInfos[selectedWeapon].weaponAttributes.weaponAttributes[weaponInfos[selectedWeapon].GetLevel()].experienceToReach.ToString("N0"); //Aled

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
            //Save new level
            PlayerPrefs.SetInt(weaponInfos[selectedWeapon].weaponAttributes.weaponName, weaponInfos[selectedWeapon].GetLevel() + 1);

            //Save new currency amount
            PlayerPrefs.SetInt("SoftCurrency", PlayerPrefs.GetInt("SoftCurrency") - weaponInfos[selectedWeapon].weaponAttributes.weaponAttributes[weaponInfos[selectedWeapon].GetLevel()].experienceToReach);

            //Refresh currency overlay
            XL_MainMenu.instance.RefreshTopOverlay();

            //Increments weapon level
            weaponInfos[selectedWeapon].weaponAttributes.level++;

            CheckHasUpgrade();
            DisplayWeaponInfo();
        }
    }

    public void UnlockWeapon()
    {
        if (PlayerPrefs.GetInt("SoftCurrency") > weaponInfos[selectedWeapon].weaponAttributes.unlockSoftCurrency)
        {
            //Unlock weapon
            PlayerPrefs.SetInt(weaponInfos[selectedWeapon].weaponAttributes.weaponName + "Unlocked", 1);

            //Save new currency amount
            PlayerPrefs.SetInt("SoftCurrency", PlayerPrefs.GetInt("SoftCurrency") - weaponInfos[selectedWeapon].weaponAttributes.unlockSoftCurrency);

            //Refresh currency overlay
            XL_MainMenu.instance.RefreshTopOverlay();

            RefreshUI();
        }
    }
}
