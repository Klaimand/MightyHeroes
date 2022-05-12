using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XL_UICaptureZone : MonoBehaviour
{
    [SerializeField] private Image captureCircle;
    [SerializeField] private TMP_Text captureText;

    public void UpdateUI(float percentage)
    {
        captureCircle.fillAmount = percentage * 0.01f;
        captureText.text = ""+percentage;
    }
}
