using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XL_CharacterDetailsMenu : MonoBehaviour
{
    [Header("Character Info")]
    [SerializeField] private XL_UICharacterInfo[] characterInfos;

    [Header("Health Bar")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Transform healthXScaler;
    [SerializeField] private float scalerHealthMax;

    [Header("Armor Bar")]
    [SerializeField] private TMP_Text armorText;
    [SerializeField] private Transform armorXScaler;
    [SerializeField] private float scalerArmorMax;

    [Header("Regen Bar")]
    [SerializeField] private TMP_Text regenText;
    [SerializeField] private Transform regenXScaler;
    [SerializeField] private float scalerRegenMax;

    [Header("Upgrade Text")]
    [SerializeField] private TMP_Text upgradeText;

    public int selectedPlayer;

    /*private void OnEnable()
    {
        foreach(XL_UICharacterInfo ci in characterInfos)
        {
            ci.characterAttributes.Initialize();
        }
    }*/

    public void Select(int idx)
    {
        foreach (XL_UICharacterInfo ci in characterInfos)
        {
            ci.characterAttributes.Initialize();
            ci.Deactivate();
        }

        selectedPlayer = idx;

        DisplayCharacterInfo(idx);

        characterInfos[idx].Activate();
    }

    private void DisplayCharacterInfo(int idx)
    {
        //initialise Text
        characterInfos[idx].DisplayLevel();
        healthText.text = characterInfos[idx].GetHealth().ToString();
        armorText.text = characterInfos[idx].GetArmor().ToString();
        regenText.text = characterInfos[idx].GetRegen().ToString();
        upgradeText.text = characterInfos[idx].characterAttributes.experienceToReach[characterInfos[idx].GetLevel()].ToString(); //Aled

        //Initialise Bar
        healthXScaler.localScale = new Vector3(characterInfos[idx].GetHealth() / scalerHealthMax, healthXScaler.localScale.y, healthXScaler.localScale.z);
        armorXScaler.localScale = new Vector3(characterInfos[idx].GetArmor() / scalerArmorMax, armorXScaler.localScale.y, armorXScaler.localScale.z);
        regenXScaler.localScale = new Vector3(characterInfos[idx].GetRegen() / scalerRegenMax, regenXScaler.localScale.y, regenXScaler.localScale.z);
    }

    public void UpgradeCharacter()
    {
        Debug.Log(characterInfos[selectedPlayer].characterAttributes.experienceToReach[characterInfos[selectedPlayer].GetLevel()]);
        if (PlayerPrefs.GetInt("SoftCurrency") > characterInfos[selectedPlayer].characterAttributes.experienceToReach[characterInfos[selectedPlayer].GetLevel()])
        {
            PlayerPrefs.SetInt(characterInfos[selectedPlayer].characterAttributes.characterName, characterInfos[selectedPlayer].GetLevel() + 1);
            characterInfos[selectedPlayer].characterAttributes.level++;
            characterInfos[selectedPlayer].characterAttributes.Initialize();
            PlayerPrefs.SetInt("SoftCurrency", PlayerPrefs.GetInt("SoftCurrency") - characterInfos[selectedPlayer].characterAttributes.experienceToReach[characterInfos[selectedPlayer].GetLevel()]);
            DisplayCharacterInfo(selectedPlayer);
        }
    }
}
