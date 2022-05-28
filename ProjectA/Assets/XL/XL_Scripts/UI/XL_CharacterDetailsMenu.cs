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

    [Header("Select")]
    [SerializeField] private GameObject selectButton;

    public int selectedPlayer = 0;

    private void Awake()
    {
        instance = this;
        selectedPlayer = PlayerPrefs.GetInt("SelectedHero");
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

    public void CallCharacterPickupAudio()
    {
        KLD_AudioManager.Instance.PlayCharacterSound("PickCharacter", 9, selectedPlayer);
    }

    private void RefreshUI()
    {
        CheckHasUpgrade();
        DisplayCharacterInfo();

        characterInfos[selectedPlayer].Activate(PlayerPrefs.GetInt(characterInfos[selectedPlayer].characterAttributes.characterName + "Unlocked"));
    }

    private void CheckHasUpgrade()
    {
        if (PlayerPrefs.GetInt(characterInfos[selectedPlayer].characterAttributes.characterName + "Unlocked") == 0)
        {
            upgradeButton.SetActive(false);
            unlockButton.SetActive(true);
            selectButton.SetActive(false);
        }
        else
        {
            unlockButton.SetActive(false);
            selectButton.SetActive(true);
        }
        if (characterInfos[selectedPlayer].GetLevel() + 1 >= characterInfos[selectedPlayer].characterAttributes.experienceToReach.Length) upgradeButton.SetActive(false);
        else upgradeButton.SetActive(true);
    }

    private void DisplayCharacterInfo()
    {
        //initialise Text
        characterInfos[selectedPlayer].DisplayLevel();
        healthText.text = characterInfos[selectedPlayer].GetHealth().ToString("N0");
        armorText.text = characterInfos[selectedPlayer].GetArmor().ToString("N0");
        regenText.text = characterInfos[selectedPlayer].GetRegen().ToString("N0");
        unlockText.text = characterInfos[selectedPlayer].characterAttributes.unlockSoftCurrency.ToString("N0");
        upgradeText.text = characterInfos[selectedPlayer].characterAttributes.experienceToReach[characterInfos[selectedPlayer].GetLevel()].ToString("N0"); //Aled

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

            KLD_MenuAudioCaller.instance.PlayCharacterBuySound();

            RefreshUI();
        }
        else
        {
            KLD_MenuAudioCaller.instance.PlayUINegativeSound();
        }
    }

    public void UnlockCharacter()
    {
        if (PlayerPrefs.GetInt("SoftCurrency") > characterInfos[selectedPlayer].characterAttributes.unlockSoftCurrency)
        {
            //Unlock Character
            PlayerPrefs.SetInt(characterInfos[selectedPlayer].characterAttributes.characterName + "Unlocked", 1);

            //Save new currency amount
            PlayerPrefs.SetInt("SoftCurrency", PlayerPrefs.GetInt("SoftCurrency") - characterInfos[selectedPlayer].characterAttributes.unlockSoftCurrency);

            //Refresh currency overlay
            XL_MainMenu.instance.RefreshTopOverlay();

            KLD_MenuAudioCaller.instance.PlayCharacterBuySound();

            RefreshUI();
        }
        else
        {
            KLD_MenuAudioCaller.instance.PlayUINegativeSound();
        }
    }
}
