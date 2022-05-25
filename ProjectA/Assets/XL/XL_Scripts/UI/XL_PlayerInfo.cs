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
        menuData.character = 0;
        menuData.weapon = (Weapon)1;
        menuData.map = 0;
        menuData.difficulty = 0;
        menuData.missionEnergyCost = 20;
    }

    public void SelectPlayer()
    {
        Debug.Log("Changed Character");
        menuData.character = (Character)characterMenu.selectedPlayer;
        onCharacterChange?.Invoke(menuData.character);
    }

    public void SelectWeapon()
    {
        Debug.Log("Changed Weapon");
        menuData.weapon = (Weapon)weaponMenu.selectedWeapon;
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
