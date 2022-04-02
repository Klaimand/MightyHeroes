using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XL_UICaptureZone : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Image captureCircle;
    [SerializeField] private TMP_Text captureText;

    private void Update()
    {
        captureCircle.transform.LookAt(camera.transform);
        captureText.transform.LookAt(camera.transform);
        captureText.transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    public void UpdateUI(float percentage)
    {
        captureCircle.fillAmount = percentage * 0.01f;
        captureText.text = ""+percentage;
    }
}
