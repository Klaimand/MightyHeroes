using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_AudioCaller : MonoBehaviour
{
    public void Play(string _key)
    {
        KLD_AudioManager.Instance.PlaySound(_key);
    }
}
