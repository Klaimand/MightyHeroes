using UnityEngine;

[CreateAssetMenu(fileName = "newBlastPassiveSpell", menuName = "KLD/Spell/Passive/Sayuri", order = 2)]
public class KLD_SayuriPassiveSpell : KLD_PassiveSpell
{
    KLD_PlayerController controller;

    public override void Initialize(PassiveSpellInitializer _initializer, int level)
    {
        controller = _initializer.controller;
    }
}
