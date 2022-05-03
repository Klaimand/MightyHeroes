using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_PlayerInitializer : MonoBehaviour
{

    [SerializeField] XL_Characters xl_character;

    void Start()
    {
        InitPlayer(Weapon.THE_CLASSIC, Character.BLAST, 0, 0);
    }

    void InitPlayer(Weapon weapon, Character character, int weaponLevel, int characterLevel)
    {
        xl_character.InitializeCharacterStats(characterLevel);
    }
}
