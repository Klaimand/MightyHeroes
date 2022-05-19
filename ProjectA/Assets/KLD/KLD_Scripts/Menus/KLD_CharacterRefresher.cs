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

    [SerializeField] XL_MainMenu mainMenu;
    [SerializeField] KLD_RotateMenuCharacter rotateMenuCharacter;

    //billboard
    [SerializeField] Renderer billboardRenderer;
    [SerializeField] Renderer auraRenderer;
    string[] characterBillboardReferences = { "_Blast", "_Simo", "_Sayuri" };

    //character
    [SerializeField] KLD_CharacterInitializer[] characters;
    int curCharIndex = 0;

    [System.Serializable]
    class CharacterAuraColor
    {
        [ColorUsage(true, true)] public Color _BaseColor;
        [ColorUsage(true, true)] public Color _FadeColor;
    }

    [SerializeField] CharacterAuraColor[] characterAuraColors;

    //weapon
    //[Header("Weapon Mesh/Anims"), Space(10)]
    int curWeaponIndex = 0;
    Animator animator;
    Transform weaponHolderParent;
    RigBuilder rigBuilder;
    TwoBoneIKConstraint leftHandIK;
    TwoBoneIKConstraint rightHandIK;
    GameObject instantiedWH;
    KLD_WeaponHolder weaponHolder;
    KLD_WeaponSO weapon;

    void Start()
    {
        curWeaponIndex = (int)Weapon.THE_CLASSIC;
        ChangeCharacter(Character.BLAST);
        //ChangeWeapon(Weapon.THE_CLASSIC);
    }

    void ChangeCharacter(Character _character) //called by event
    {
        auraRenderer.material.SetColor("_BaseColor", characterAuraColors[(int)_character]._BaseColor);
        auraRenderer.material.SetColor("_FadeColor", characterAuraColors[(int)_character]._FadeColor);

        rotateMenuCharacter.ResetVelAndAngle();
        for (int i = 0; i < characterBillboardReferences.Length; i++)
        {
            billboardRenderer.material.SetInt(characterBillboardReferences[i], (i == (int)_character) ? 1 : 0);
            characters[i].gameObject.SetActive(i == (int)_character);
        }
        curCharIndex = (int)_character;

        animator = characters[curCharIndex].animator;
        weaponHolderParent = characters[curCharIndex].weaponHolderParent;
        rigBuilder = characters[curCharIndex].rigBuilder;
        leftHandIK = characters[curCharIndex].leftHandIK;
        rightHandIK = characters[curCharIndex].rightHandIK;

        ChangeWeapon((Weapon)curWeaponIndex);
    }

    void ChangeWeapon(Weapon _weapon) //called by event
    {
        rotateMenuCharacter.ResetVelAndAngle();
        curWeaponIndex = (int)_weapon;

        if (weaponHolderParent.childCount > 3) { Destroy(weaponHolderParent.GetChild(3).gameObject); }

        weapon = mainMenu.GetWeaponSO(_weapon);

        instantiedWH = Instantiate(weapon.weaponHolder, Vector3.zero, Quaternion.identity, weaponHolderParent);

        //instantiedWH.name = "WeaponHolder";
        instantiedWH.name = weapon.weaponHolder.name;

        instantiedWH.transform.localPosition = weapon.weaponHolder.transform.position;
        instantiedWH.transform.localRotation = weapon.weaponHolder.transform.rotation;


        weaponHolder = instantiedWH.GetComponent<KLD_WeaponHolder>();

        rightHandIK.data.target = weaponHolder.leftHandle;
        leftHandIK.data.target = weaponHolder.rightHandle;

        //canon = weaponHolder.canon;


        animator.runtimeAnimatorController = weapon.animatorOverrideController;


        animator.enabled = false;
        animator.enabled = true;

        rigBuilder.Build();

        animator.enabled = false;
        animator.enabled = true;


    }
}
