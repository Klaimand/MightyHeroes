using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_Vsync : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

}
