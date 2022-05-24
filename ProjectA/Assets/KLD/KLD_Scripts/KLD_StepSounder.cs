using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_StepSounder : MonoBehaviour
{
    int nbSteps;
    bool b;
    public void DoStepSound()
    {
        if (nbSteps % 2 == 0)
        {
            KLD_AudioManager.Instance.PlaySound("Step01");
        }
        else
        {
            KLD_AudioManager.Instance.PlaySound(b ? "Step02" : "Step03");
            b = !b;
        }
        nbSteps++;
    }
}
