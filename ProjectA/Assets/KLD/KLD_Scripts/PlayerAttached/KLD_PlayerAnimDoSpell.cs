using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_PlayerAnimDoSpell : MonoBehaviour
{
    [SerializeField] XL_Characters character;

    public void DoSpell()
    {
        character.DoSpell();
    }
}
