using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KLD_FPSCounter : MonoBehaviour
{
    [SerializeField] Text text;

    float t;

    void Update()
    {
        if (t > 0.1f)
        {
            text.text = (1f / Time.deltaTime).ToString("f0");
            t = 0f;
        }
        t += Time.deltaTime;
    }
}
