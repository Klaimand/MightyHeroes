using UnityEngine;

//[CreateAssetMenu(fileName = "KLD_GameModeSO", menuName = "KLD/GameModeSO", order = 0)]
public class KLD_GameModeSO : ScriptableObject
{
    public float missionMaxTime = 300f;
    [SerializeField] protected string objectiveString = "Capture the zones";

    public virtual string GetGameModeHeader(int _nbObjectivesCompleted, int _nbObjectives)
    {
        return $"{_nbObjectivesCompleted}  / {_nbObjectives}  : {objectiveString}";
    }
}
