using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XL_UICaptureZone : MonoBehaviour
{
    [SerializeField] private Image captureCircle;
    [SerializeField] private TMP_Text captureText;

    int ticks = 0;
    int points = 0;

    public void UpdateUI(float percentage, int _playersInside)
    {
        captureCircle.fillAmount = percentage * 0.01f;
        if (percentage >= 100f)
        {
            captureText.text = "CAPTURED";
        }
        else
        {
            if (_playersInside == 0)
            {
                captureText.text = "";
            }
            else
            {
                captureText.text = "CAPTURING" + GetPoints();
            }
        }
        ticks++;
        if (ticks > 8)
        {
            ticks = 0;
            points++;
        }
    }

    string returnString = "";
    string GetPoints()
    {
        returnString = "";
        for (int i = 0; i < points % 4; i++)
        {
            returnString += '.';
        }
        return returnString;
    }
}
