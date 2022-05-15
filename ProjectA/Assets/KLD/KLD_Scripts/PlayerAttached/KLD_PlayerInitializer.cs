using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_PlayerInitializer : MonoBehaviour
{
    [Header("Character & Weapon")]
    [SerializeField] Character character;
    [SerializeField, Range(0, 9)] int seri_characterLevel = 0;
    [SerializeField] Weapon weapon;
    [SerializeField, Range(0, 5)] int seri_weaponLevel = 0;

    [SerializeField] List<XL_CharacterAttributesSO> charactersList = new List<XL_CharacterAttributesSO>();
    [SerializeField] List<KLD_WeaponSO> weaponsList = new List<KLD_WeaponSO>();

    XL_CharacterAttributesSO curCharacter;
    KLD_WeaponSO curWeapon;

    [Header("In Scene")]
    [SerializeField] Transform targetPosSmooth;
    [SerializeField] KLD_TouchInputs touchInputs;

    [Header("On Object")]
    [SerializeField] KLD_PlayerController controller;
    [SerializeField] KLD_PlayerShoot playerShoot;
    [SerializeField] XL_Characters xl_character;

    GameObject instanciatedCharacterMesh;
    KLD_CharacterInitializer charIniter;

    int nbChild = 0;

    void Start()
    {
        InitPlayer(weapon, character, seri_weaponLevel, seri_characterLevel);
    }

    void InitPlayer(Weapon _weapon, Character _character, int _weaponLevel, int _characterLevel)
    {
        curCharacter = charactersList[(int)_character];
        curWeapon = weaponsList[(int)_weapon];

        InitCharacterStats(_characterLevel);

        touchInputs.InitializeActiveJoystickOrButton(curCharacter.spellIsButton);

        InitCharacterMesh();

        playerShoot.Init(curWeapon, _weaponLevel);
    }

    void InitCharacterStats(int _characterLevel)
    {
        xl_character.InitializeCharacter(curCharacter, _characterLevel);
    }

    void InitCharacterMesh()
    {
        if (transform.childCount > 1)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        instanciatedCharacterMesh = Instantiate(curCharacter.characterMesh, transform.position, Quaternion.identity, transform);
        instanciatedCharacterMesh.transform.localRotation = Quaternion.identity;
        charIniter = instanciatedCharacterMesh.GetComponent<KLD_CharacterInitializer>();

        charIniter.Init(xl_character, targetPosSmooth);

        controller.SetCharacterMeshComponents(charIniter.animator, charIniter.scaler);

        playerShoot.SetCharacterMeshComponents(
            charIniter.animator,
            charIniter.weaponHolderParent,
            charIniter.rigBuilder,
            charIniter.leftHandIK,
            charIniter.rightHandIK);

    }
}
