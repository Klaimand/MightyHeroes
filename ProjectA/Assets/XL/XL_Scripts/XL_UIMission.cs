using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XL_UIMission : MonoBehaviour
{
    [SerializeField] private TMP_Text timerSeconds;
    [SerializeField] private TMP_Text timerMinutes;
    int minutes;

    [SerializeField] private TMP_Text objective;

    public void UpdateTimer(float time)
    {
        minutes = (int)Mathf.Floor(time / 60);
        timerSeconds.text = (time - (60 * minutes)).ToString("00");
        timerMinutes.text = minutes.ToString("00");
    }

    public void UpdateObjective(string objectiveString)
    {
        objective.text = objectiveString;
    }
}
