using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_PlayerInfo : MonoBehaviour
{
    public KLD_MenuData menuData;
    [SerializeField] private XL_CharacterDetailsMenu characterMenu;
    [SerializeField] private XL_WeaponDetailsMenu weaponMenu;

    public void Awake()
    {
        if (menuData == null)
        {
            menuData = new KLD_MenuData();
        }
        DontDestroyOnLoad(this.gameObject);
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
