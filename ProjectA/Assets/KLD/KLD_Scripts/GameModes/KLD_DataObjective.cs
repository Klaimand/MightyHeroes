using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_DataObjective : MonoBehaviour, KLD_IObjective
{
    bool collected = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool GetObjectiveState()
    {
        return collected;
    }

    public Transform GetTransform()
    {
        return transform;
    }

}
