using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KLD_TutorialGameModeSO", menuName = "KLD/GameModes/TutorialSO", order = 0)]
public class TutorialGameModeSO : KLD_GameModeSO
{
    [SerializeField] GameObject tutoUI;

    [SerializeField] float firstDialogDelay = 3f;

    RectTransform rectTransform;

    KLD_TutoDialogParent dialogsParent;

    public override void InitGameModeUI(RectTransform topLeftCorner, int _nbLines, string[] _objectivesNames)
    {
        topLeftCorner.parent.GetChild(0).gameObject.SetActive(false);
        topLeftCorner.parent.GetChild(1).gameObject.SetActive(false);

        rectTransform = Instantiate(tutoUI, Vector3.zero, Quaternion.identity, topLeftCorner).GetComponent<RectTransform>();

        rectTransform.anchoredPosition = Vector2.zero;

        dialogsParent = rectTransform.GetChild(0).GetChild(1).gameObject.GetComponent<KLD_TutoDialogParent>();

        dialogsParent.ShowDialog(0, firstDialogDelay);
    }

    public override void CompleteObjective(int _index, int _nbObjectivesCompleted, int _nbObjectives)
    {
        if (_index != _nbObjectives - 1)
        {
            dialogsParent.NextDialog(_index);
        }
        else
        {
            dialogsParent.HideDialog(_index);
        }
    }
}
