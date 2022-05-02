using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XL_UIMission : MonoBehaviour
{
    [SerializeField] private TMP_Text timer;
    int minutes;

    [SerializeField] private TMP_Text objective;

    public void UpdateTimer(float time) 
    {
        minutes = (int)Mathf.Floor(time / 60);
        timer.text = minutes + " : " + (time - (60 * minutes));
    }

    public void UpdateObjective(string objectiveString) 
    {
        objective.text = objectiveString;
    }
}
