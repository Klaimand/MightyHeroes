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

    public int selectedWeapon;

    public void Select(int idx)
    {
        foreach (XL_UIWeaponInfo wi in weaponInfos)
        {
            wi.Deactivate();
        }

        selectedWeapon = idx;

        //Initialise Text
        weaponInfos[idx].DisplayLevel();
        damageText.text = weaponInfos[idx].GetDamage().ToString();
        rpmText.text = weaponInfos[idx].GetRPM().ToString();
        magazineSizeText.text = weaponInfos[idx].GetMagazine().ToString();

        //Initialise Bar
        damageXScaler.localScale = new Vector3(weaponInfos[idx].GetDamage() / scalerDamageMax, damageXScaler.localScale.y, damageXScaler.localScale.z);
        rpmXScaler.localScale = new Vector3(weaponInfos[idx].GetRPM() / scalerRPMMax, rpmXScaler.localScale.y, rpmXScaler.localScale.z);
        magazineSizeXScaler.localScale = new Vector3(weaponInfos[idx].GetMagazine() / scalerMagazineSizeMax, magazineSizeXScaler.localScale.y, magazineSizeXScaler.localScale.z);

        weaponInfos[idx].Activate();

    }
}
