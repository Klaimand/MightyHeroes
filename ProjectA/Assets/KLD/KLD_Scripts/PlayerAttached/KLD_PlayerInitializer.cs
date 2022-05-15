using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_PlayerInitializer : MonoBehaviour
{
    [Header("Character & Weapon")]
    [SerializeField] Character character;
    [SerializeField, Range(0, 9)] int characterLevel = 0;
    [SerializeField] Weapon weapon;
    [SerializeField, Range(0, 5)] int weaponLevel = 0;

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
        print("touch inputs not inited (button/joystick)");

        InitPlayer(weapon, character, 0, 0);
    }

    void InitPlayer(Weapon _weapon, Character _character, int _weaponLevel, int _characterLevel)
    {
        curCharacter = charactersList[(int)_character];
        curWeapon = weaponsList[(int)_weapon];

        InitCharacterStats();

        InitCharacterMesh();

        playerShoot.Init(curWeapon, weaponLevel);
    }

    void InitCharacterStats()
    {
        xl_character.InitializeCharacter(curCharacter, characterLevel);
    }

    void InitCharacterMesh()
    {
        nbChild = transform.childCount;
        if (nbChild > 3)
        {
            nbChild = 3;
        }
        if (nbChild > 0)
        {
            for (int i = nbChild - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
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
