using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_WeaponSelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponMenus;
    [SerializeField] private GameObject[] yellowButtons;
    [SerializeField] private XL_UIWeaponInfo[] weaponInfos;

    //private int weaponTypeSelected;

    public void OnEnable()
    {
        foreach (XL_UIWeaponInfo wi in weaponInfos)
        {
            wi.DisplayLevel();
            wi.Activate(PlayerPrefs.GetInt(wi.weaponAttributes.weaponName + "Unlocked"));
        }
        if (PlayerPrefs.HasKey("WeaponTypeSelected")) SwitchWeaponMenu(PlayerPrefs.GetInt("WeaponTypeSelected"));
        else SwitchWeaponMenu(0);
    }

    public void SwitchWeaponMenu(int idx)
    {
        foreach (GameObject menu in weaponMenus)
        {
            menu.SetActive(false);
        }

        foreach (GameObject yellowButton in yellowButtons)
        {
            yellowButton.SetActive(false);
        }

        yellowButtons[idx].SetActive(true);
        weaponMenus[idx].SetActive(true);
        //weaponTypeSelected = idx;
        PlayerPrefs.SetInt("WeaponTypeSelected", idx);
    }
}
