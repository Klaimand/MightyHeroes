using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KLD_Spell : ScriptableObject
{
    public abstract void ActivateSpell(Vector3 direction, Transform pos);

    public virtual void OnUltJoystickDown(Vector2 _joyDirection, Transform _player)
    {

    }

    public virtual void OnSpellLaunch()
    {

    }
}
