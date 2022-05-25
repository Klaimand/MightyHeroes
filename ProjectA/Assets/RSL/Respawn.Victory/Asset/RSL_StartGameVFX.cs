using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSL_StartGameVFX : MonoBehaviour
{
    [SerializeField] GameObject vfxPrefab;

    public void salutatouscestsqueezie()
    {

        Instantiate(vfxPrefab);

    }

}
