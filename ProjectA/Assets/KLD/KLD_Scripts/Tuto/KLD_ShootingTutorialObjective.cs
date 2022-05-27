using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KLD_ShootingTutorialObjective : MonoBehaviour, KLD_IObjective
{
    [SerializeField] int index = 2;
    [SerializeField] int mobsToKill = 3;

    [SerializeField] UnityEvent onObjectiveComplete;

    int mobsKilled = 0;

    bool completed = false;

    void OnEnable()
    {
        KLD_EventsManager.instance.onEnemyKill += KillMob;
    }

    void OnDisable()
    {
        KLD_EventsManager.instance.onEnemyKill -= KillMob;
    }

    void KillMob(Enemy enemy)
    {
        mobsKilled++;

        if (!completed && mobsKilled >= mobsToKill)
        {
            onObjectiveComplete.Invoke();
            completed = true;
            XL_GameModeManager.instance.CompleteObjective(index);
        }
    }

    public void Reset()
    {
        mobsKilled = 0;
    }


    string KLD_IObjective.GetObjectiveName()
    {
        return "";
    }

    bool KLD_IObjective.GetObjectiveState()
    {
        return completed;
    }

    Transform KLD_IObjective.GetTransform()
    {
        return transform;
    }

    void KLD_IObjective.SetIndex(int _index)
    {

    }
}
