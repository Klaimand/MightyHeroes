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
}
