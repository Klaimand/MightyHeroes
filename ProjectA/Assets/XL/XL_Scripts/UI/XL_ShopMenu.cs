using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_ShopMenu : MonoBehaviour
{

    public void BuySoftCurrency(int amount) 
    {
        PlayerPrefs.SetInt("SoftCurrency", PlayerPrefs.GetInt("SoftCurrency") + amount);
        XL_MainMenu.instance.RefreshTopOverlay();
    }

    public void BuyHardCurrency(int amount) 
    {
        PlayerPrefs.SetInt("HardCurrency", PlayerPrefs.GetInt("HardCurrency") + amount);
        XL_MainMenu.instance.RefreshTopOverlay();
    }

    public void BuyEnergy(int amount) 
    {
        PlayerPrefs.SetInt("Energy", PlayerPrefs.GetInt("Energy") + amount);
        XL_MainMenu.instance.RefreshTopOverlay();
    }
}
