using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_MissionInfos : MonoBehaviour
{
    public static KLD_MissionInfos instance;

    public KLD_MissionData missionData;

    [Header("Ratios")]
    [SerializeField] float remainingTimeRatio = 1.5f;
    [SerializeField] float killedEnemiesRatio = 1f;
    [SerializeField] float difficultyRatio = 0.5f;
    [SerializeField] float difficultyOffset = 0.5f;
    [SerializeField] float softCurrencyRatio = 0.1f;
    [SerializeField] float loosedHealthRatio = 0.2f;

    void Awake()
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
    }

    // Start is called before the first frame update
    void Start()
    {
        InitialiseMissionData();
    }

    public void InitialiseMissionData()
    {
        if (missionData == null)
        {
            missionData = new KLD_MissionData();
        }

        missionData.remainingTime = 0;
        missionData.killedEnemies = 0;
        missionData.difficulty = 0;
        missionData.loosedHealth = 0f;
        missionData.succeeded = false;
        missionData.missionEnergyCost = 0;

        KLD_EventsManager.instance.onEnemyKill += AddKilledEnemy;
        KLD_EventsManager.instance.onHealthLoose += AddLoosedHealth;

        missionData.remainingTimeRatio = remainingTimeRatio;
        missionData.killedEnemiesRatio = killedEnemiesRatio;
        missionData.difficultyRatio = difficultyRatio;
        missionData.difficultyOffset = difficultyOffset;
        missionData.softCurrencyRatio = softCurrencyRatio;
        missionData.loosedHealthRatio = loosedHealthRatio;
    }


    void AddKilledEnemy(Enemy _enemy)
    {
        missionData.killedEnemies++;
    }

    void AddLoosedHealth(float loosedHealth)
    {
        missionData.loosedHealth += Mathf.Abs(loosedHealth);
    }

    public void RefreshMissionInfos(bool _succeeded)
    {
        missionData.remainingTime = Mathf.RoundToInt(XL_GameModeManager.instance.GetMissionTime());
        //enemies killed refreshed by event
        //loosed health added by event
        missionData.succeeded = _succeeded;
        if (XL_PlayerInfo.instance != null)
        {
            missionData.difficulty = (int)XL_PlayerInfo.instance.menuData.difficulty + 1;
            missionData.missionEnergyCost = XL_PlayerInfo.instance.menuData.missionEnergyCost;
        }
    }
}