using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_OptionMenu : MonoBehaviour
{

    [Header("All Sounds Buttons")]
    [SerializeField] private GameObject muteAllButton;
    [SerializeField] private GameObject unmuteAllButton;

    [Header("Music Buttons")]
    [SerializeField] private GameObject muteMusicButton;
    [SerializeField] private GameObject unmuteMusicButton;

    [Header("SFX Buttons")]
    [SerializeField] private GameObject muteSFXButton;
    [SerializeField] private GameObject unmuteSFXButton;


    public void MuteAll()
    {
        muteAllButton.SetActive(false);

        KLD_MenuAudioCaller.instance.MuteVolume();
        unmuteAllButton.SetActive(true);
    }

    public void UnmuteAll()
    {
        unmuteAllButton.SetActive(false);

        KLD_MenuAudioCaller.instance.UnmuteVolume();
        muteAllButton.SetActive(true);
    }

    public void MuteMusic()
    {
        muteMusicButton.SetActive(false);

        KLD_MenuAudioCaller.instance.MuteGame();
        unmuteMusicButton.SetActive(true);
    }

    public void UnmuteMusic()
    {
        unmuteMusicButton.SetActive(false);

        KLD_MenuAudioCaller.instance.UnmuteGame();
        muteMusicButton.SetActive(true);
    }

    public void MuteSFX()
    {
        muteSFXButton.SetActive(false);

        KLD_MenuAudioCaller.instance.MuteSFX();
        unmuteSFXButton.SetActive(true);
    }

    public void UnmuteSFX()
    {
        unmuteSFXButton.SetActive(false);

        KLD_MenuAudioCaller.instance.UnmuteSFX();
        muteSFXButton.SetActive(true);
    }
}
