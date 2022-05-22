using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface KLD_IObjective
{
    string GetObjectiveName();
    bool GetObjectiveState();
    Transform GetTransform();
    void SetIndex(int _index);
}
