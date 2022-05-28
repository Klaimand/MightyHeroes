using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class XL_PlayerInfo : MonoBehaviour
{
    public static XL_PlayerInfo instance;

    public KLD_MenuData menuData;
    [SerializeField] private XL_CharacterDetailsMenu characterMenu;
    [SerializeField] private XL_WeaponDetailsMenu weaponMenu;

    public Action<Weapon> onWeaponChange;
    public Action<Character> onCharacterChange;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (menuData == null)
        {
            menuData = new KLD_MenuData();
        }

        //InitialiseMenuData();
    }

    void Start()
    {
        InitialiseMenuData();
    }

    public void Initialise()
    {
        if (XL_CharacterDetailsMenu.instance != null)
        {
            characterMenu = XL_CharacterDetailsMenu.instance;
        }
        if (XL_WeaponDetailsMenu.instance != null)
        {
            weaponMenu = XL_WeaponDetailsMenu.instance;
        }
    }

    private void InitialiseMenuData()
    {
        if (!PlayerPrefs.HasKey("SelectedCharacter"))
        {
            menuData.character = 0;
        }
        else
        {
            menuData.character = (Character)PlayerPrefs.GetInt("SelectedCharacter");
        }

        if (!PlayerPrefs.HasKey("SelectedWeapon"))
        {
            menuData.weapon = (Weapon)1;
        }
        else
        {
            menuData.weapon = (Weapon)PlayerPrefs.GetInt("SelectedWeapon");
        }

        StartCoroutine(WaitAndRefreshChara());
        //onCharacterChange?.Invoke(menuData.character);
        //onWeaponChange?.Invoke(menuData.weapon);

        menuData.map = 0;
        menuData.difficulty = 0;
        menuData.missionEnergyCost = 20;
    }

    public void CallSceneRestart()
    {
        StartCoroutine(WaitAndRefreshChara());
    }

    IEnumerator WaitAndRefreshChara()
    {
        yield return null;
        onCharacterChange?.Invoke(menuData.character);
        onWeaponChange?.Invoke(menuData.weapon);
    }

    public void SelectPlayer()
    {
        Debug.Log("Changed Character");
        menuData.character = (Character)characterMenu.selectedPlayer;
        PlayerPrefs.SetInt("SelectedCharacter", characterMenu.selectedPlayer);
        print(PlayerPrefs.GetInt("SelectedCharacter"));
        onCharacterChange?.Invoke(menuData.character);
    }

    public void SelectWeapon()
    {
        Debug.Log("Changed Weapon");
        menuData.weapon = (Weapon)weaponMenu.selectedWeapon;
        PlayerPrefs.SetInt("SelectedWeapon", weaponMenu.selectedWeapon);
        print(PlayerPrefs.GetInt("SelectedWeapon"));
        onWeaponChange?.Invoke(menuData.weapon);
    }

    public void SelectMap(int idx)
    {
        menuData.map = (Map)idx;
    }

    public void SelectDifficulty(int idx)
    {
        menuData.difficulty = (Difficulty)idx;
    }

    public void SetEnergyCost(int energyCost)
    {
        menuData.missionEnergyCost = energyCost;
    }
}
