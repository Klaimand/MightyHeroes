using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_SceneAudioManagerInst : MonoBehaviour
{
    void OnEnable()
    {
        KLD_EventsManager.instance.onGameEnd += StopAllLoopingSounds;
    }

    void OnDisable()
    {
        KLD_EventsManager.instance.onGameEnd -= StopAllLoopingSounds;
    }


    void StopAllLoopingSounds()
    {
        KLD_AudioManager.Instance.StopAllLoopingSounds();
    }
}
