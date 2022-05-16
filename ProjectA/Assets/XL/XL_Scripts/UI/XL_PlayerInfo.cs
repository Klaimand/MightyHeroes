using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_PlayerInfo : MonoBehaviour
{
    public static XL_PlayerInfo instance;

    public KLD_MenuData menuData;
    [SerializeField] private XL_CharacterDetailsMenu characterMenu;
    [SerializeField] private XL_WeaponDetailsMenu weaponMenu;

    public void Awake()
    {
        instance = this;

        if (menuData == null)
        {
            menuData = new KLD_MenuData();
        }
        DontDestroyOnLoad(this.gameObject);

        InitialiseMenuData();
    }

    private void InitialiseMenuData()
    {
        menuData.character = 0;
        menuData.weapon = 0;
        menuData.map = 0;
        menuData.difficulty = 0;
    }

    public void SelectPlayer()
    {
        menuData.character = (Character)characterMenu.selectedPlayer;
    }

    public void SelectWeapon()
    {
        menuData.weapon = (Weapon)weaponMenu.selectedWeapon;
    }

    public void SelectMap(int idx)
    {
        menuData.map = (Map)idx;
    }

    public void SelectDifficulty(int idx)
    {
        menuData.difficulty = (Difficulty)idx;
    }
}
