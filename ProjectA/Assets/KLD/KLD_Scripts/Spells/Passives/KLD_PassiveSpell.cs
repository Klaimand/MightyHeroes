using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KLD_PassiveSpell : ScriptableObject
{
    public abstract void Initialize(PassiveSpellInitializer _initializer, int level);
}

public class PassiveSpellInitializer
{
    public XL_Characters character;
    public KLD_PlayerController controller;
}