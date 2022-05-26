using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KLD_TriggerAtStart : MonoBehaviour
{
    [SerializeField] UnityEvent onStart;

    void Start()
    {
        onStart.Invoke();
    }
}
