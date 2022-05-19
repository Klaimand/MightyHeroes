using UnityEngine;

[CreateAssetMenu(fileName = "KLD_SurviveGameModeSO", menuName = "KLD/GameModes/SurviveSO", order = 0)]
public class SurviveGameModeSO : KLD_GameModeSO
{
    public override string GetGameModeHeader(int _nbObjectivesCompleted, int _nbObjectives)
    {
        return objectiveString;
    }
}