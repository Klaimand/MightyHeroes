using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XL_GameManager : MonoBehaviour
{
    public static XL_GameManager instance;
    public List<GameObject> players = new List<GameObject>();
    //public List<XL_Enemy> enemies = new List<XL_Enemy>();
    //public KLD_ZombieList zombieList;
    [SerializeField] float timeBetweenInitAndShowStats = 1.5f;


    [SerializeField] KLD_EndingScreen endingScreen;

    bool gameEnded = false;

    private void Awake()
    {
        instance = this;
    }

    public void WinGame()
    {
        if (!gameEnded)
        {
            gameEnded = true;

            KLD_MissionInfos.instance.RefreshMissionInfos(true);

            StartCoroutine(ChangeSceneCoroutine());
        }
    }

    public void LoseGame()
    {
        if (!gameEnded)
        {
            gameEnded = true;

            KLD_MissionInfos.instance.RefreshMissionInfos(false);

            StartCoroutine(ChangeSceneCoroutine());
        }
    }

    IEnumerator ChangeSceneCoroutine()
    {
        //disable inputs

        Time.timeScale = 0f;

        endingScreen.gameObject.SetActive(true);

        endingScreen.Initialize();

        yield return new WaitForSecondsRealtime(timeBetweenInitAndShowStats);

        endingScreen.ShowStats();

    }

    //triggered by ending screen button
    public void ChangeScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
