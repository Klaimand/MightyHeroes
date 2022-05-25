using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XL_CharacterDetailsMenu : MonoBehaviour
{
    public static XL_CharacterDetailsMenu instance;

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

    [Header("Unlock")]
    [SerializeField] private GameObject unlockButton;
    [SerializeField] private TMP_Text unlockText;

    [Header("Upgrade")]
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private TMP_Text upgradeText;

    public int selectedPlayer = 0;

    private void Awake()
    {
        instance = this;
    }

    public void Select(int idx)
    {
        foreach (XL_UICharacterInfo ci in characterInfos)
        {
            ci.characterAttributes.Initialize();
            ci.Deactivate();
        }

        selectedPlayer = idx;

        RefreshUI();
    }

    private void RefreshUI()
    {
        CheckHasUpgrade();
        DisplayCharacterInfo();

        characterInfos[selectedPlayer].Activate(PlayerPrefs.GetInt(characterInfos[selectedPlayer].characterAttributes.characterName + "Unlocked"));
    }

    private void CheckHasUpgrade()
    {
        Debug.Log("is unlocked : " + PlayerPrefs.GetInt(characterInfos[selectedPlayer].characterAttributes.characterName + "Unlocked"));
        if (PlayerPrefs.GetInt(characterInfos[selectedPlayer].characterAttributes.characterName + "Unlocked") == 0)
        {
            upgradeButton.SetActive(false);
            unlockButton.SetActive(true);
        }
        else unlockButton.SetActive(false);
        if (characterInfos[selectedPlayer].GetLevel() + 1 >= characterInfos[selectedPlayer].characterAttributes.experienceToReach.Length) upgradeButton.SetActive(false);
        else upgradeButton.SetActive(true);
    }

    private void DisplayCharacterInfo()
    {
        //initialise Text
        characterInfos[selectedPlayer].DisplayLevel();
        healthText.text = characterInfos[selectedPlayer].GetHealth().ToString();
        armorText.text = characterInfos[selectedPlayer].GetArmor().ToString();
        regenText.text = characterInfos[selectedPlayer].GetRegen().ToString();
        unlockText.text = characterInfos[selectedPlayer].characterAttributes.unlockSoftCurrency.ToString();
        upgradeText.text = characterInfos[selectedPlayer].characterAttributes.experienceToReach[characterInfos[selectedPlayer].GetLevel()].ToString(); //Aled

        //Initialise Bar
        healthXScaler.localScale = new Vector3(characterInfos[selectedPlayer].GetHealth() / scalerHealthMax, healthXScaler.localScale.y, healthXScaler.localScale.z);
        armorXScaler.localScale = new Vector3(characterInfos[selectedPlayer].GetArmor() / scalerArmorMax, armorXScaler.localScale.y, armorXScaler.localScale.z);
        regenXScaler.localScale = new Vector3(characterInfos[selectedPlayer].GetRegen() / scalerRegenMax, regenXScaler.localScale.y, regenXScaler.localScale.z);
    }

    public void UpgradeCharacter()
    {
        if (PlayerPrefs.GetInt("SoftCurrency") > characterInfos[selectedPlayer].characterAttributes.experienceToReach[characterInfos[selectedPlayer].GetLevel()])
        {
            //Save new level
            PlayerPrefs.SetInt(characterInfos[selectedPlayer].characterAttributes.characterName, characterInfos[selectedPlayer].GetLevel() + 1);

            //Save new currency amount
            PlayerPrefs.SetInt("SoftCurrency", PlayerPrefs.GetInt("SoftCurrency") - characterInfos[selectedPlayer].characterAttributes.experienceToReach[characterInfos[selectedPlayer].GetLevel()]);

            //Increments character level
            characterInfos[selectedPlayer].characterAttributes.level++;

            //Initialise stats for the current level
            characterInfos[selectedPlayer].characterAttributes.Initialize();

            //Refresh currency overlay
            XL_MainMenu.instance.RefreshTopOverlay();

            RefreshUI();
        }
    }

    public void UnlockCharacter()
    {
        if (PlayerPrefs.GetInt("SoftCurrency") > characterInfos[selectedPlayer].characterAttributes.unlockSoftCurrency)
        {
            //Save new level
            PlayerPrefs.SetInt(characterInfos[selectedPlayer].characterAttributes.characterName + "Unlocked", 1);

            //Save new currency amount
            PlayerPrefs.SetInt("SoftCurrency", PlayerPrefs.GetInt("SoftCurrency") - characterInfos[selectedPlayer].characterAttributes.unlockSoftCurrency);

            //Refresh currency overlay
            XL_MainMenu.instance.RefreshTopOverlay();

            RefreshUI();
        }
    }
}
