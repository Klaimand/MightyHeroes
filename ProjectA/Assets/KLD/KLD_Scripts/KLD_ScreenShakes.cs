using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class KLD_ScreenShakes : MonoBehaviour
{
    public static KLD_ScreenShakes instance;

    [SerializeField] CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin noise;

    CinemachineCollisionImpulseSource impulseSource;

    float shakeTime = 0f;
    float shakePower = 0f;
    float shakeFrequency = 0f;
    float shakeFadeTime = 0f;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        noise = cam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        DoShake();
    }

    void DoShake()
    {
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
            //print("Time remaining : " + shakeTime + " Power : " + shakePower);
            noise.m_AmplitudeGain = shakePower;
        }
        else
        {
            shakeTime = 0f;
            noise.m_AmplitudeGain = 0f;
        }
    }

    public void StartShake(float _lenght, float _power, float _frequency)
    {
        if (_power > shakePower)
        {
            shakePower = _power;
            //shakeFrequency = _frequency;
            noise.m_FrequencyGain = _frequency;
            if (_lenght > shakeTime)
            {
                shakeTime = _lenght;
                shakeFadeTime = _power / _lenght;
            }
        }
    }
}
