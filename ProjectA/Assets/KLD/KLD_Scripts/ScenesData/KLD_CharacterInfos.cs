using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_CharacterInfos : MonoBehaviour
{
    public static KLD_CharacterInfos instance;

    [SerializeField] XL_Characters xl_Characters;

    void Awake()
    {
        instance = this;
    }

    public string GetCharacterSoundPrefix()
    {
        return xl_Characters.GetCharacterSoundPrefix();
    }

    public int GetCharacterSoundIndex()
    {
        return xl_Characters.GetCharacterSoundIndex();
    }
}
