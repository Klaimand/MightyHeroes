using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_GameModeManager : MonoBehaviour
{
    public static XL_GameModeManager instance;

    List<XL_GameMode> objectives = new List<XL_GameMode>();
    GameObject[] temp;
    private int nbObjectives;
    private int nbObjectivesCompleted;

    private void Awake()
    {
        instance = this;

        temp = GameObject.FindGameObjectsWithTag("Objective");
        foreach (GameObject go in temp) 
        {
            objectives.Add(go.GetComponent<XL_GameMode>());
        }
        nbObjectives = objectives.Count;
        nbObjectivesCompleted = 0;
    }

    Transform nearestObjective;
    float distance;
    float nextDistance;

    public Transform GetNearestObjective(Vector3 position) 
    {
        distance = 100000;
        foreach (XL_GameMode go in objectives) 
        {
            if (!go.GetObjectiveState())
            {
                nextDistance = (position - go.GetTransform().position).sqrMagnitude;
                if (nextDistance < distance)
                {
                    distance = nextDistance;
                    nearestObjective = go.GetTransform();
                }
            }
        }
        return nearestObjective;
    }

    public void CompleteObjective() 
    {
        nbObjectivesCompleted++;
        if (nbObjectivesCompleted >= nbObjectives) XL_GameManager.instance.WinGame();
    }
}
