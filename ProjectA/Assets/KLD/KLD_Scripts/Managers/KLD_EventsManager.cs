using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Enemy
{
    SWARMER,
    KAMIKAZE,
    ALPHA
}

public class KLD_EventsManager : MonoBehaviour
{
    public static KLD_EventsManager instance;

    void Awake()
    {
        instance = this;
    }


    public Action<Enemy> onEnemyKill;

    public void InvokeEnemyKill(Enemy _enemy)
    {
        onEnemyKill?.Invoke(_enemy);
    }


    public Action onEnemyHit;

    public void InvokeEnemyHit()
    {
        onEnemyHit?.Invoke();
    }
}
