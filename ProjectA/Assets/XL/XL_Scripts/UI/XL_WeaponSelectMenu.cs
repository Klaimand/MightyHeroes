using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_WeaponSelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponMenus;
    [SerializeField] private XL_UIWeaponInfo[] weaponInfos;

    public void OnEnable()
    {
        foreach (XL_UIWeaponInfo wi in weaponInfos)
        {
            wi.DisplayLevel();
        }
    }

    public void SwitchWeaponMenu(int idx)
    {
        foreach (GameObject menu in weaponMenus)
        {
            menu.SetActive(false);
        }

        weaponMenus[idx].SetActive(true);
    }
}
