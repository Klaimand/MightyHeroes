using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KLD_CaptureZoneGameModeSO", menuName = "KLD/GameModes/CaptureZoneSO", order = 0)]
public class CaptureZonesGameModeSO : KLD_GameModeSO
{



    [Header("UI")]
    [SerializeField] GameObject objectiveLine;
    List<GameObject> objectiveLines;
    int verticalSpacing = -75;


    RectTransform curLineTransform;
    Vector2 curLinePosition;

    public override void InitGameModeUI(RectTransform topLeftCorner, int _nbLines)
    {
        for (int i = 0; i < _nbLines; i++)
        {
            curLineTransform = Instantiate(objectiveLine, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
            curLinePosition.x = 50;
            curLinePosition.y = 50 + verticalSpacing * i;

            curLineTransform.anchoredPosition = curLinePosition;
        }
    }
}
