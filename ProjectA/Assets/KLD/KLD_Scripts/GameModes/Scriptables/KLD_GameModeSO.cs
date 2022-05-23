using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "KLD_GameModeSO", menuName = "KLD/GameModeSO", order = 0)]
public abstract class KLD_GameModeSO : ScriptableObject
{
    public string gamemodeName = "CAPTURE ZONE";
    public string mapName = "MALL";
    public float missionMaxTime = 300f;
    [SerializeField] protected string objectiveString = "CAPTURE THE ZONES";
    public int maxObjectiveNb = 3;



    public virtual string GetGameModeHeader(int _nbObjectivesCompleted, int _nbObjectives)
    {
        //return $"{_nbObjectivesCompleted}  / {_nbObjectives}  : {objectiveString}";
        return objectiveString;
    }

    public abstract void InitGameModeUI(RectTransform topLeftCorner, int _nbLines, string[] _objectivesNames);

    public virtual void CompleteObjective(int _index, int _nbObjectivesCompleted, int _nbObjectives)
    {

    }

}
