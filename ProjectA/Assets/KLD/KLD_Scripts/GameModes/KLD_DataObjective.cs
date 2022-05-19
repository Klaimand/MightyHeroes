using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_DataObjective : MonoBehaviour, XL_GameMode
{
    [SerializeField] static string objectiveString = "Collect the data";
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

    public string GetObjectiveString()
    {
        return objectiveString;
    }

    public Transform GetTransform()
    {
        return transform;
    }

}
