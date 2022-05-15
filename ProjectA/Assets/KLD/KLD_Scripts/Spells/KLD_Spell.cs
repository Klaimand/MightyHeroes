using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KLD_Spell : ScriptableObject
{
    public abstract void ActivateSpell(Vector3 direction, Transform pos);
}
