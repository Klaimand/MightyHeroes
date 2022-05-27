using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_TutorialObjective : MonoBehaviour, KLD_IObjective
{
    [SerializeField] int index;

    bool completed = false;

    public void CompleteObjective()
    {
        if (!completed)
        {
            completed = true;
            XL_GameModeManager.instance.CompleteObjective(index);
        }
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
