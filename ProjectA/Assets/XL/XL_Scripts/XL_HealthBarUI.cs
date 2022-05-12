using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XL_HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image ultBar;

    public void UpdateHealthBar(float fillAmount)
    {
        healthBar.fillAmount = fillAmount;
    }

    public void UpdateUltBar(float fillAmount)
    {
        ultBar.fillAmount = fillAmount;
    }
}
