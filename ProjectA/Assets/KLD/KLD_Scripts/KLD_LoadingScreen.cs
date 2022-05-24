using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_LoadingScreen : MonoBehaviour
{

    public static KLD_LoadingScreen instance;

    void Awake()
    {
        if (instance == null)
        {
            transform.parent = null;
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowLoadingScreen()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
