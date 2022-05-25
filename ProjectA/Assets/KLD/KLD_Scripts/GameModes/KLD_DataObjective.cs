using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_DataObjective : MonoBehaviour, KLD_IObjective
{
    [SerializeField] string objectiveName = "newDataObjective";

    bool collected = false;
    int index;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            XL_GameModeManager.instance.CompleteObjective(index);

            KLD_AudioManager.Instance.PlayCharacterSound("IntelPickup", 4);

            collected = true;

            DespawnObjective();
        }
    }

    void DespawnObjective()
    {
        gameObject.SetActive(false);
    }

    #region Interface Implementation

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

    #endregion
}
