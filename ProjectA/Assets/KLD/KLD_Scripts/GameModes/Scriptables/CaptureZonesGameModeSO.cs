using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "KLD_CaptureZoneGameModeSO", menuName = "KLD/GameModes/CaptureZoneSO", order = 0)]
public class CaptureZonesGameModeSO : KLD_GameModeSO
{



    [Header("UI")]
    [SerializeField] GameObject objectiveLine;
    List<GameObject> objectiveLines = new List<GameObject>();
    int verticalSpacing = -75;

    RectTransform curLineTransform;
    Vector2 curLinePosition;

    public override void InitGameModeUI(RectTransform topLeftCorner, int _nbLines, string[] _objectivesNames)
    {
        objectiveLines.Clear();
        for (int i = 0; i < _nbLines; i++)
        {
            objectiveLines.Add(Instantiate(objectiveLine, Vector3.zero, Quaternion.identity, topLeftCorner));
            curLineTransform = objectiveLines[i].GetComponent<RectTransform>();
            curLinePosition.x = 50;
            curLinePosition.y = -50 + verticalSpacing * i;

            curLineTransform.anchoredPosition = curLinePosition;

            curLineTransform.GetChild(1).GetComponent<TMP_Text>().text = _objectivesNames[i];
            curLineTransform.GetChild(3).gameObject.SetActive(false);
        }
    }

    public override void CompleteObjective(int _index, int _nbObjectivesCompleted, int _nbObjectives)
    {
        objectiveLines[_index].transform.GetChild(3).gameObject.SetActive(true);
    }
}
