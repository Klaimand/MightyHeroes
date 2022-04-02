using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_CaptureZone : MonoBehaviour, XL_GameMode
{
    [SerializeField] private XL_GameManager manager;
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
        if (manager != null)
        {
            if (GetGameState()) 
            {
                manager.WinGame();
                Debug.Log("test");
                enabled = false;
            } 
        }
    }

    IEnumerator CaptureZoneCoroutine(float t) 
    {
        while (true) 
        {
            capturePercentage += playersInside.Count;
            ui.UpdateUI(capturePercentage);
            Debug.Log("Capture percentage : " + capturePercentage);
            yield return new WaitForSeconds(t);
        }
    }

    //Might cause problems if a player dies inside capture zone

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player")) 
        {
            Debug.Log("Player : " + other.name + " has entered capture zone");
            playersInside.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("Player : " + other.name + " has left capture zone");
            playersInside.Remove(other.gameObject);
        }
    }

    public bool GetGameState()
    {
        if (capturePercentage < 100) return false;
        else return true;
    }
}
