using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class XL_MainMenu : MonoBehaviour
{
    public static XL_MainMenu instance;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject characterSelectMenu;
    [SerializeField] private GameObject characterDetailsMenu;
    [SerializeField] private GameObject weaponSelectMenu;
    [SerializeField] private GameObject weaponDetailsMenu;
    [SerializeField] private GameObject mapSelectMenu;
    [SerializeField] private GameObject shopMenu;

    [Header("Save Characters and Weapons")]
    [SerializeField] private XL_CharacterAttributesSO[] characterAttributes;
    [SerializeField] private KLD_WeaponSO[] weaponAttributes;

    [Header("Energy")]
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text energyMaxText;
    [SerializeField] private int energyMax;

    [Header("Soft Currency")]
    [SerializeField] private TMP_Text softCurrencyText;

    [Header("Hard Currency")]
    [SerializeField] private TMP_Text hardCurrencyText;

    [Header("Mission Type")]
    [SerializeField] private GameObject[] missionTypes;

    [Header("Loader Bars")]
    [SerializeField] private TMP_Text characterXP;
    [SerializeField] private TMP_Text weaponXP;

    //[Header("Loadout")]


    private void Awake()
    {
        instance = this;

        InitPlayerPrefs();
        RefreshMainMenuUI();
        RefreshTopOverlay();
    }

    private void InitPlayerPrefs()
    {
        //---DELETE PLAYERPREFS---
        PlayerPrefs.DeleteAll();

        foreach (XL_CharacterAttributesSO ca in characterAttributes)
        {
            if (!PlayerPrefs.HasKey(ca.characterName))
            {
                Debug.Log("Test");
                PlayerPrefs.SetInt(ca.characterName, 1);
            }
            ca.level = PlayerPrefs.GetInt(ca.characterName);
        }
        foreach (KLD_WeaponSO wa in weaponAttributes)
        {
            if (!PlayerPrefs.HasKey(wa.weaponName))
            {
                PlayerPrefs.SetInt(wa.weaponName, 1);
            }
            wa.level = PlayerPrefs.GetInt(wa.weaponName);
        }
        if (!PlayerPrefs.HasKey("Energy"))
        {
            PlayerPrefs.SetInt("Energy", 100);
        }
        if (!PlayerPrefs.HasKey("SoftCurrency"))
        {
            PlayerPrefs.SetInt("SoftCurrency", 20000);
        }
        if (!PlayerPrefs.HasKey("HardCurrency"))
        {
            PlayerPrefs.SetInt("HardCurrency", 1000);
        }
        if (!PlayerPrefs.HasKey("SelectedHero"))
        {
            PlayerPrefs.SetInt("SelectedHero", 0);
        }

        PlayerPrefs.Save();
    }

    private void RefreshMainMenuUI()
    {
        characterXP.text = (characterAttributes[(int)XL_PlayerInfo.instance.menuData.character].level + 1).ToString();
        weaponXP.text = (weaponAttributes[(int)XL_PlayerInfo.instance.menuData.weapon].level + 1).ToString();
    }

    public void StartMission()
    {
        SceneManager.LoadScene(1);
    }

    public void SwitchMainMenu()
    {
        RefreshMainMenuUI();
        mainMenu.SetActive(true);

        shopMenu.SetActive(false);
        characterSelectMenu.SetActive(false);
        characterDetailsMenu.SetActive(false);
        weaponSelectMenu.SetActive(false);
        weaponDetailsMenu.SetActive(false);
        mapSelectMenu.SetActive(false);
    }

    public void SwitchShopMenu()
    {
        shopMenu.SetActive(true);

        mainMenu.SetActive(false);
        characterSelectMenu.SetActive(false);
        characterDetailsMenu.SetActive(false);
        weaponSelectMenu.SetActive(false);
        weaponDetailsMenu.SetActive(false);
        mapSelectMenu.SetActive(false);
    }

    public void SwitchCharaSelectMenu()
    {
        characterSelectMenu.SetActive(true);

        mainMenu.SetActive(false);
        characterDetailsMenu.SetActive(false);
    }

    public void SwitchCharacterDetailsMenu()
    {
        characterDetailsMenu.SetActive(true);

        characterSelectMenu.SetActive(false);
    }

    public void SwitchWeaponSelectMenu()
    {
        weaponSelectMenu.SetActive(true);

        mainMenu.SetActive(false);
        weaponDetailsMenu.SetActive(false);
    }

    public void SwitchWeaponDetailsMenu()
    {
        weaponDetailsMenu.SetActive(true);

        weaponSelectMenu.SetActive(false);
    }

    public void SwitchMapSelectMenu()
    {
        mapSelectMenu.SetActive(true);

        mainMenu.SetActive(false);
    }

    public void SelectMissionType(int idx)
    {
        foreach (GameObject go in missionTypes)
        {
            go.SetActive(false);
        }

        missionTypes[idx].SetActive(true);
    }

    public void RefreshTopOverlay()
    {
        //Energy
        energyText.text = PlayerPrefs.GetInt("Energy").ToString();
        energyMaxText.text = energyMax.ToString();

        //SoftCurrency
        softCurrencyText.text = PlayerPrefs.GetInt("SoftCurrency").ToString();

        //HardCurrency
        hardCurrencyText.text = PlayerPrefs.GetInt("HardCurrency").ToString();
    }

    public KLD_WeaponSO GetWeaponSO(Weapon _weapon)
    {
        return weaponAttributes[(int)_weapon];
    }
}
