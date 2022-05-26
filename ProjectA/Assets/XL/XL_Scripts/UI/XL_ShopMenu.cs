using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_ShopMenu : MonoBehaviour
{

    public void BuySoftCurrency(int amount) 
    {
        PlayerPrefs.SetInt("SoftCurrency", PlayerPrefs.GetInt("SoftCurrency") + amount);
        KLD_MenuAudioCaller.instance.PlayUIPositiveSound();
        XL_MainMenu.instance.RefreshTopOverlay();
    }

    public void BuyHardCurrency(int amount) 
    {
        PlayerPrefs.SetInt("HardCurrency", PlayerPrefs.GetInt("HardCurrency") + amount);
        KLD_MenuAudioCaller.instance.PlayUIPositiveSound();
        XL_MainMenu.instance.RefreshTopOverlay();
    }

    public void BuyEnergy(int amount) 
    {
        PlayerPrefs.SetInt("Energy", PlayerPrefs.GetInt("Energy") + amount);
        KLD_MenuAudioCaller.instance.PlayUIPositiveSound();
        XL_MainMenu.instance.RefreshTopOverlay();
    }
}
