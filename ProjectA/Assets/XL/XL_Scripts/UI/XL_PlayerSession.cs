using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class XL_PlayerSession : MonoBehaviour
{
    public static XL_PlayerSession instance;

    [SerializeField] private float timeToRecoverEnergy;
    private WaitForSeconds wait;
    private float timeElapsed;

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

        if (PlayerPrefs.HasKey("DateQuit"))
        {
            //Debug.Log(DateTime.Parse(PlayerPrefs.GetString("DateQuit")).ToString("MM/dd/yyyy HH:mm"));
            Debug.Log((DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("DateQuit"))).TotalSeconds);
            PlayerPrefs.SetInt("Energy", (int)((DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("DateQuit"))).TotalSeconds / timeToRecoverEnergy) + PlayerPrefs.GetInt("Energy"));
            if (XL_MainMenu.instance != null)
            {
                XL_MainMenu.instance.RefreshTopOverlay();
            }
        }

        instance.StartCoroutine(EnergyCoroutine());
        
    }

    public IEnumerator EnergyCoroutine()
    {
        if (XL_MainMenu.instance != null)
        {
            if (XL_MainMenu.instance.GetEnergyMaxAmount() <= PlayerPrefs.GetInt("Energy")) StopAllCoroutines();
        }

        yield return wait;

        PlayerPrefs.SetInt("Energy", PlayerPrefs.GetInt("Energy") + 1);
        instance.StartCoroutine(EnergyCoroutine());
        if (XL_MainMenu.instance != null)
        {
            XL_MainMenu.instance.RefreshTopOverlay();
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Quit");
        PlayerPrefs.SetString("DateQuit", DateTime.Now.ToString());
    }
}
