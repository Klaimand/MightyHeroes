using UnityEngine;

[CreateAssetMenu(fileName = "KLD_GameModeSO", menuName = "KLD/GameModeSO", order = 0)]
public class KLD_GameModeSO : ScriptableObject
{
    public float missionMaxTime = 300f;
    public string objectiveString = "Capture the zones";
}