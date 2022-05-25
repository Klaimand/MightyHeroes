using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class XL_PlayerSession : MonoBehaviour
{
    public static XL_PlayerSession instance;

    [SerializeField] private float timeToRecoverEnergy;
    private WaitForSeconds wait;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        wait = new WaitForSeconds(timeToRecoverEnergy);

        instance.StartCoroutine(InitializeCoroutine());
    }

    private int _energy;

    IEnumerator InitializeCoroutine()
    {
        yield return new WaitForEndOfFrame();

        if (PlayerPrefs.HasKey("DateQuit"))
        {
            if (XL_MainMenu.instance != null)
            {
                _energy = (int)((DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("DateQuit"))).TotalSeconds / timeToRecoverEnergy) + PlayerPrefs.GetInt("Energy");
                if (_energy > XL_MainMenu.instance.GetEnergyMaxAmount())
                {
                    PlayerPrefs.SetInt("Energy", XL_MainMenu.instance.GetEnergyMaxAmount());
                }
                else
                {
                    PlayerPrefs.SetInt("Energy", _energy);
                }
                XL_MainMenu.instance.RefreshTopOverlay();
            }
        }

        instance.StopAllCoroutines(); //it will cancel energy coroutine at any time -> find a better way to not have multiple coroutines running at the same time

        instance.StartCoroutine(EnergyCoroutine());
    }

    public IEnumerator EnergyCoroutine()
    {
        if (XL_MainMenu.instance != null)
        {
            if (XL_MainMenu.instance.GetEnergyMaxAmount() <= PlayerPrefs.GetInt("Energy"))
            {
                PlayerPrefs.SetInt("Energy", XL_MainMenu.instance.GetEnergyMaxAmount());
                instance.StopAllCoroutines();
            }
            XL_MainMenu.instance.RefreshTopOverlay();
            XL_MainMenu.instance.RefreshGOButton();
        }

        yield return wait;

        PlayerPrefs.SetInt("Energy", PlayerPrefs.GetInt("Energy") + 1);
        instance.StartCoroutine(EnergyCoroutine());
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("DateQuit", DateTime.Now.ToString());
    }
}
