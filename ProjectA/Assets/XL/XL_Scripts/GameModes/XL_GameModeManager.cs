using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class XL_GameModeManager : MonoBehaviour
{
    public static XL_GameModeManager instance;

    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [SerializeField] KLD_GameModeSO gameMode;

    [SerializeField] RectTransform topLeftCorner;

    List<KLD_IObjective> objectives = new List<KLD_IObjective>();
    GameObject[] temp;
    private int nbObjectives;
    private int nbObjectivesCompleted;

    float missionTime;
    WaitForSeconds wait;

    [SerializeField] XL_UIMission uiMission;

    private void Awake()
    {
        instance = this;

        wait = new WaitForSeconds(1);
        missionTime = gameMode.missionMaxTime;

    }

    private void Start()
    {
        InitializeObjectives();
        StartCoroutine(TimerCoroutine());
        gameMode.InitGameModeUI(topLeftCorner, nbObjectives);
        uiMission.UpdateObjective(gameMode.GetGameModeHeader(nbObjectivesCompleted, nbObjectives));
    }

    void InitializeObjectives()
    {
        temp = GameObject.FindGameObjectsWithTag("Objective");
        foreach (GameObject go in temp)
        {
            objectives.Add(go.GetComponent<KLD_IObjective>());
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
        uiMission.UpdateObjective(gameMode.GetGameModeHeader(nbObjectivesCompleted, nbObjectives));
        if (nbObjectivesCompleted >= nbObjectives) XL_GameManager.instance.WinGame();
    }

    IEnumerator TimerCoroutine()
    {
        while (true)
        {
            missionTime--;
            uiMission.UpdateTimer(missionTime);
            yield return wait;
        }
    }

    public float GetMissionTime()
    {
        return this.missionTime;
    }
}
