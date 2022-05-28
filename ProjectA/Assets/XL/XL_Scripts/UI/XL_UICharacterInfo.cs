using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class XL_UICharacterInfo
{
    public GameObject characterUI;
    public GameObject lockedSprite;
    public GameObject unlockedSprite;
    public TMP_Text characterLvlUI;
    public XL_CharacterAttributesSO characterAttributes;

    public void Activate(int i)
    {
        characterUI.SetActive(true);
        if (i == 0)
        {
            unlockedSprite.SetActive(false);
            lockedSprite.SetActive(true);            
        }
        else
        {
            lockedSprite.SetActive(false);
            unlockedSprite.SetActive(true);            
        }
        
    }

    public void Deactivate()
    {
        characterUI.SetActive(false);
        lockedSprite.SetActive(false);
        unlockedSprite.SetActive(false);
    }

    public void DisplayLevel()
    {
        characterLvlUI.text = (GetLevel() + 1).ToString("N0");
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
