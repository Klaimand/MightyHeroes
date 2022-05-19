using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_CaptureZone : MonoBehaviour, KLD_IObjective
{
    [SerializeField] string objectiveName = "newCaptureZone";
    [SerializeField] private XL_UICaptureZone ui;

    [SerializeField] private float tickTime;
    private int capturePercentage;

    [SerializeField] private LayerMask layer;
    List<GameObject> playersInside = new List<GameObject>();

    private void Start()
    {
        capturePercentage = 0;
        playersInside.Clear();
        StartCoroutine(CaptureZoneCoroutine(tickTime));
    }

    private void Update()
    {
        if (GetObjectiveState())
        {
            StopAllCoroutines();
            XL_GameModeManager.instance.CompleteObjective(index);
            //Debug.Log("test");
            enabled = false;
        }
    }

    IEnumerator CaptureZoneCoroutine(float t)
    {
        while (true)
        {
            capturePercentage += playersInside.Count;
            ui.UpdateUI(capturePercentage);
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
