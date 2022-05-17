using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class KLD_CharacterRefresher : MonoBehaviour
{
    #region Event subscription
    void OnEnable()
    {
        XL_PlayerInfo.instance.onCharacterChange += ChangeCharacter;
        XL_PlayerInfo.instance.onWeaponChange += ChangeWeapon;
    }

    void OnDisable()
    {
        XL_PlayerInfo.instance.onCharacterChange -= ChangeCharacter;
        XL_PlayerInfo.instance.onWeaponChange -= ChangeWeapon;
    }
    #endregion

    //billboard
    [SerializeField] Renderer billboardRenderer;
    string[] characterBillboardReferences = { "_Blast", "_Simo", "_Sayuri" };

    //character
    [SerializeField] KLD_CharacterInitializer[] characters;
    int curCharIndex = 0;

    //weapon
    [Header("Weapon Mesh/Anims"), Space(10)]
    Transform weaponHolderParent;
    RigBuilder rigBuilder;
    TwoBoneIKConstraint leftHandIK;
    TwoBoneIKConstraint rightHandIK;

    void Start()
    {
        ChangeCharacter(Character.BLAST);
    }

    void ChangeCharacter(Character _character) //called by event
    {
        for (int i = 0; i < characterBillboardReferences.Length; i++)
        {
            billboardRenderer.material.SetInt(characterBillboardReferences[i], (i == (int)_character) ? 1 : 0);
            characters[i].gameObject.SetActive(i == (int)_character);
        }
        curCharIndex = (int)_character;
    }

    void ChangeWeapon(Weapon _weapon) //called by event
    {



    }
}
