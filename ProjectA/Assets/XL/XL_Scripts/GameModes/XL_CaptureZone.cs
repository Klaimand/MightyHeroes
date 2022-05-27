using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XL_CaptureZone : MonoBehaviour, KLD_IObjective
{
    [SerializeField] string objectiveName = "newCaptureZone";
    [SerializeField] private XL_UICaptureZone ui;

    [SerializeField] private float tickTime;
    private int capturePercentage;

    [SerializeField] private LayerMask layer;
    List<GameObject> playersInside = new List<GameObject>();

    [SerializeField] ParticleSystem captureFX;
    [SerializeField] ParticleSystem captureEndFX;
    [SerializeField] Renderer zoneRenderer;
    [SerializeField] Renderer eggsRenderer;

    [SerializeField] float fadeTime = 2f;


    [SerializeField] UnityEvent onZoneCaptured;

    [Header("Shadows")]
    [SerializeField] Color white = Color.white;
    [SerializeField] Color noAlpha = new Color(1f, 1f, 1f, 0f);
    [SerializeField] SpriteRenderer[] eggsShadows;

    bool captured = false;

    private void Start()
    {
        KLD_AudioManager.Instance.GetSound("TakingZone").GetSource().loop = true;

        zoneRenderer.sharedMaterial.SetFloat("_Dissolve", 0.15f);
        eggsRenderer.sharedMaterial.SetFloat("_Dissolve", 0f);

        capturePercentage = 0;
        playersInside.Clear();
        StartCoroutine(CaptureZoneCoroutine(tickTime));
    }

    private void Update()
    {
        if (!captured)
        {
            if (playersInside.Count > 0)
            {
                if (!captureFX.isPlaying)
                {
                    KLD_AudioManager.Instance.PlaySound("TakingZone");
                    captureFX.Play();
                }
            }
            else
            {
                if (captureFX.isPlaying)
                {
                    KLD_AudioManager.Instance.GetSound("TakingZone").GetSource().Stop();
                    captureFX.Stop();
                }
            }
        }

        if (GetObjectiveState() && !captured)
        {
            captured = true;
            StopAllCoroutines();
            XL_GameModeManager.instance.CompleteObjective(index);
            onZoneCaptured.Invoke();

            KLD_AudioManager.Instance.PlayCharacterSound("CaptureZone", 3);

            KLD_AudioManager.Instance.GetSound("TakingZone").GetSource().Stop();
            KLD_AudioManager.Instance.PlaySound("ZoneTaken");

            //Debug.Log("test");
            captureFX.Stop();
            captureEndFX.Play();
            //enabled = false;

            StartCoroutine(LerpMaterialValue(0.15f, -0.1f, fadeTime, zoneRenderer, "_Dissolve"));

            //for (int i = 0; i < eggsRenderers.Length; i++)
            //{
            StartCoroutine(LerpMaterialValue(0f, 1f, fadeTime, eggsRenderer, "_Dissolve"));

            foreach (var rd in eggsShadows)
            {
                StartCoroutine(FadeEgg(fadeTime, rd));
            }
            //}
        }
    }

    IEnumerator CaptureZoneCoroutine(float t)
    {
        while (true)
        {
            capturePercentage += playersInside.Count;
            ui.UpdateUI(capturePercentage, playersInside.Count);
            yield return new WaitForSeconds(t);
        }
    }

    //Might cause problems if a player dies inside capture zone

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //Debug.Log("Player : " + other.name + " has entered capture zone");
            playersInside.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //Debug.Log("Player : " + other.name + " has left capture zone");
            playersInside.Remove(other.gameObject);
        }
    }


    IEnumerator LerpMaterialValue(float _startValue, float _endValue, float _time, Renderer _renderer, string _reference)
    {
        float t = 0f;
        while (t < _time)
        {
            _renderer.sharedMaterial.SetFloat(_reference, Mathf.Lerp(_startValue, _endValue, t / _time));

            yield return null;
            t += Time.deltaTime;
        }
    }

    IEnumerator FadeEgg(float _time, SpriteRenderer _image)
    {
        float t = 0f;

        while (t < _time)
        {
            _image.color = Color.Lerp(white, noAlpha, t / _time);
            yield return null;
            t += Time.deltaTime;
        }
    }


    public bool GetObjectiveState()
    {
        return !(capturePercentage < 100);
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    public string GetObjectiveName()
    {
        return objectiveName;
    }

    int index = 0;

    public void SetIndex(int _index)
    {
        index = _index;
    }
}
