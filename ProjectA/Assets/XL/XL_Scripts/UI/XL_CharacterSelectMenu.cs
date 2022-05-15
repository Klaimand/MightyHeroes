using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_CharacterSelectMenu : MonoBehaviour
{
    [SerializeField] private XL_UICharacterInfo[] characterInfos;

    public void OnEnable()
    {
        foreach (XL_UICharacterInfo ci in characterInfos)
        {
            ci.characterAttributes.Initialize();
            ci.DisplayLevel();
        }
    }
}
