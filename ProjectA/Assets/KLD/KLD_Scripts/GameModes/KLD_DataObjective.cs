using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_DataObjective : MonoBehaviour, KLD_IObjective
{
    [SerializeField] string objectiveName = "newDataObjective";

    bool collected = false;
    int index;

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

    string KLD_IObjective.GetObjectiveName()
    {
        return objectiveName;
    }

    void KLD_IObjective.SetIndex(int _index)
    {
        index = _index;
    }
}
