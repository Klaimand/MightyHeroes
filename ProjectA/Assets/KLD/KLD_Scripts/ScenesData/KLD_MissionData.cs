using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KLD_MissionData
{
    //Entry values
    public int remainingTime = 0;

    public int killedEnemies = 0;

    public int difficulty = 0;

    public float loosedHealth = 0f;

    public bool succeeded = false;

    public int missionEnergyCost = 0;

    //ratios
    public float remainingTimeRatio = 1.5f;
    public float killedEnemiesRatio = 1f;
    public float difficultyRatio = 0.5f;
    public float difficultyOffset = 0.5f;
    public float softCurrencyRatio = 0.1f;
    public float loosedHealthRatio = 0.2f;

    //Exit values
    public int GetSoftCurrency()
    {
        return Mathf.RoundToInt(
            (remainingTime * remainingTimeRatio + killedEnemies * killedEnemiesRatio) * (difficulty * difficultyRatio + difficultyOffset)
            ) * 2;
    }

    int hardCurrencyToReturn;

    public int GetHardCurrency()
    {
        hardCurrencyToReturn = Mathf.RoundToInt(
            GetSoftCurrency() * softCurrencyRatio - loosedHealth * loosedHealthRatio
        );

        if (hardCurrencyToReturn < 0) hardCurrencyToReturn = 0;

        return hardCurrencyToReturn;
    }

    public int GetEnergy()
    {
        return Mathf.RoundToInt(
            missionEnergyCost * (succeeded ? 1f : 0.5f)
        );
    }
}
