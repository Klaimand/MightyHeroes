using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class XL_GameModeManager : MonoBehaviour
{
    public static XL_GameModeManager instance;

    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [SerializeField] KLD_GameModeSO gameMode;

    [SerializeField] RectTransform topLeftCorner;

    List<KLD_IObjective> objectives = new List<KLD_IObjective>();
    List<KLD_IObjective> totalObjectives = new List<KLD_IObjective>();
    KLD_IObjective drawedObjective;
    GameObject[] temp;
    private int nbObjectives;
    private int nbObjectivesCompleted;
    int nbObjsToDraw;

    float missionTime;
    WaitForSeconds wait;

    [SerializeField] XL_UIMission uiMission;

    List<string> objectivesNames = new List<string>();

    bool initialized = false;

    private void Awake()
    {
        instance = this;

        wait = new WaitForSeconds(1);
        missionTime = gameMode.missionMaxTime;

        //InitializeObjectives();

    }

    private void Start()
    {
        InitializeObjectives();
        StartCoroutine(TimerCoroutine());
        gameMode.InitGameModeUI(topLeftCorner, nbObjectives, objectivesNames.ToArray());
        uiMission.UpdateObjective(gameMode.GetGameModeHeader(nbObjectivesCompleted, nbObjectives));
    }

    void InitializeObjectives()
    {
        if (initialized) return;

        initialized = true;

        objectivesNames.Clear();
        temp = GameObject.FindGameObjectsWithTag("Objective");
        foreach (GameObject go in temp)
        {
            totalObjectives.Add(go.GetComponent<KLD_IObjective>());
        }

        nbObjsToDraw = Mathf.Min(totalObjectives.Count, gameMode.maxObjectiveNb);
        for (int i = 0; i < nbObjsToDraw; i++)
        {
            drawedObjective = totalObjectives[Random.Range(0, totalObjectives.Count)];
            totalObjectives.Remove(drawedObjective);
            objectives.Add(drawedObjective);
        }

        for (int i = 0; i < totalObjectives.Count; i++)
        {
            totalObjectives[i].GetTransform().gameObject.SetActive(false);
        }

        //objectives = objectives.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < objectives.Count; i++)
        {
            objectives[i].SetIndex(i);
            objectivesNames.Add(objectives[i].GetObjectiveName());
        }
        nbObjectives = objectives.Count;
        nbObjectivesCompleted = 0;
    }

    Transform nearestObjective;
    float distance;
    float nextDistance;

    public Transform GetNearestObjective(Vector3 position)
    {
        if (!initialized) InitializeObjectives();
        distance = 100000;
        foreach (KLD_IObjective go in objectives)
        {
            if (!go.GetObjectiveState())
            {
                nextDistance = (position - go.GetTransform().position).sqrMagnitude;
                if (!go.GetObjectiveState() && nextDistance < distance)
                {
                    distance = nextDistance;
                    nearestObjective = go.GetTransform();
                }
            }
        }
        return nearestObjective;
    }

    public void CompleteObjective(int _index)
    {
        nbObjectivesCompleted++;

        gameMode.CompleteObjective(_index, nbObjectivesCompleted, nbObjectives);

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
