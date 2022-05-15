using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class YTH_Trigger : MonoBehaviour
{

    [SerializeField] UnityEvent OnPlayerEnter;
    [SerializeField] UnityEvent OnPlayerExit;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPlayerEnter.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPlayerExit.Invoke();
        }
    }
}
