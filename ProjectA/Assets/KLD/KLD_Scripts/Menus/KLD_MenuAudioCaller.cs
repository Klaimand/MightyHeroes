using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_MenuAudioCaller : MonoBehaviour
{
    public void PlayUIPositiveSound()
    {
        KLD_AudioManager.Instance.PlaySound("UI_Positive");
    }

    public void PlayUINegativeSound()
    {
        KLD_AudioManager.Instance.PlaySound("UI_Negative");
    }

    public void PlayCharacterBuySound()
    {
        KLD_AudioManager.Instance.PlaySound("BuyCharacter");
    }

    public void PlayWeaponBuySound()
    {
        KLD_AudioManager.Instance.PlaySound("BuyWeapon");
    }

    public void MuteGame()
    {
        KLD_AudioManager.Instance.GetAudioMixer().SetFloat("MasterVolume", -80f);
    }

    public void UnmuteGame()
    {
        KLD_AudioManager.Instance.GetAudioMixer().SetFloat("MasterVolume", 0f);
    }

    public void MuteSFX()
    {
        KLD_AudioManager.Instance.GetAudioMixer().SetFloat("SFXVolume", -80f);
    }

    public void UnmuteSFX()
    {
        KLD_AudioManager.Instance.GetAudioMixer().SetFloat("SFXVolume", 0f);
    }

    public void MuteVolume()
    {
        KLD_AudioManager.Instance.GetAudioMixer().SetFloat("SFXVolume", -80f);
    }

    public void UnmuteVolume()
    {
        KLD_AudioManager.Instance.GetAudioMixer().SetFloat("VolumeVolume", 0f);
    }
}