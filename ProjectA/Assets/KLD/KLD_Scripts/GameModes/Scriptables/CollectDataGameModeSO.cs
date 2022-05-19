using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "KLD_CollectDataGameModeSO", menuName = "KLD/GameModes/CollectDataSO", order = 0)]
public class CollectDataGameModeSO : KLD_GameModeSO
{
    [Header("UI")]
    [SerializeField] GameObject dataTextParent;
    RectTransform rectTransform;
    TMP_Text text;

    public override void InitGameModeUI(RectTransform topLeftCorner, int _nbLines, string[] _objectivesNames)
    {
        rectTransform = Instantiate(dataTextParent, Vector3.zero, Quaternion.identity, topLeftCorner).GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
        text = rectTransform.GetChild(0).GetComponent<TMP_Text>();
        text.text = $"0 / {_nbLines}";
    }

    public override void CompleteObjective(int _index, int _nbObjectivesCompleted, int _nbObjectives)
    {
        text.text = $"{_nbObjectivesCompleted} / {_nbObjectives}";
    }
}
