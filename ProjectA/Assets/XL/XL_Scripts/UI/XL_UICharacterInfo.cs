using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class XL_UICharacterInfo
{
    public GameObject characterUI;
    public TMP_Text characterLvlUI;
    public XL_CharacterAttributesSO characterAttributes;

    public void Activate()
    {
        characterUI.SetActive(true);
    }

    public void Deactivate()
    {
        characterUI.SetActive(false);
    }

    public void DisplayLevel()
    {
        characterLvlUI.text = GetLevel().ToString();
    }

    public float GetHealth()
    {
        return characterAttributes.healthMax;
    }

    public float GetArmor()
    {
        return characterAttributes.armor;
    }

    public float GetRegen()
    {
        return characterAttributes.healingTick;
    }

    public int GetLevel()
    {
        return characterAttributes.level;
    }
}
