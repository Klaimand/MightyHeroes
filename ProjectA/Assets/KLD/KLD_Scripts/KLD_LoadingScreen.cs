using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KLD_LoadingScreen : MonoBehaviour
{

    public static KLD_LoadingScreen instance;

    [SerializeField] TMP_Text tipsText;

    [SerializeField] Tip[] tips;

    [System.Serializable]
    class Tip
    {
        [TextArea(3, 8)]
        public string tip;
    }

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
        tipsText.text = tips[Random.Range(0, tips.Length)].tip.ToUpper();
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
