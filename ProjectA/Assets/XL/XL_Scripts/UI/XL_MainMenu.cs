using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class XL_MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject characterSelectMenu;
    [SerializeField] private GameObject characterDetailsMenu;
    [SerializeField] private GameObject weaponSelectMenu;
    [SerializeField] private GameObject weaponDetailsMenu;

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

    //[Header("Loadout")]


    private void Awake()
    {
        InitPlayerPrefs();
        RefreshMainMenuUI();
    }

    private void InitPlayerPrefs()
    {
        //DELETE PLAYERPREFS
        PlayerPrefs.DeleteAll();

        foreach (XL_CharacterAttributesSO ca in characterAttributes)
        {
            if (!PlayerPrefs.HasKey(ca.characterName))
            {
                Debug.Log("Test");
                PlayerPrefs.SetInt(ca.characterName, 0);
            }
            ca.level = PlayerPrefs.GetInt(ca.characterName);
        }
        foreach (KLD_WeaponSO wa in weaponAttributes)
        {
            if (!PlayerPrefs.HasKey(wa.weaponName))
            {
                PlayerPrefs.SetInt(wa.weaponName, 0);
            }
            wa.level = PlayerPrefs.GetInt(wa.weaponName);
        }
        if (!PlayerPrefs.HasKey("Energy"))
        {
            PlayerPrefs.SetInt("Energy", 100);
        }
        if (!PlayerPrefs.HasKey("SoftCurrency"))
        {
            PlayerPrefs.SetInt("SoftCurrency", 0);
        }
        if (!PlayerPrefs.HasKey("HardCurrency"))
        {
            PlayerPrefs.SetInt("HardCurrency", 0);
        }
        if (!PlayerPrefs.HasKey("SelectedHero"))
        {
            PlayerPrefs.SetInt("SelectedHero", 0);
        }

        Debug.Log(PlayerPrefs.GetInt("Blast"));

        PlayerPrefs.Save();
    }

    private void RefreshMainMenuUI()
    {
        //Energy
        energyText.text = PlayerPrefs.GetInt("Energy").ToString();
        energyMaxText.text = energyMax.ToString();

        //SoftCurrency
        softCurrencyText.text = PlayerPrefs.GetInt("SoftCurrency").ToString();

        //HardCurrency
        hardCurrencyText.text = PlayerPrefs.GetInt("HardCurrency").ToString();
    } 

    public void StartMission()
    {
        SceneManager.LoadScene(1);
    }

    public void SwitchMainMenu()
    {
        mainMenu.SetActive(true);

        characterSelectMenu.SetActive(false);
        characterDetailsMenu.SetActive(false);
        weaponSelectMenu.SetActive(false);
        weaponDetailsMenu.SetActive(false);
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
}
