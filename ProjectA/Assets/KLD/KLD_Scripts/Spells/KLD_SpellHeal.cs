using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSpellHeal", menuName = "KLD/Spell/Heal", order = 0)]
public class KLD_SpellHeal : KLD_Spell
{
    public override void ActivateSpell(Vector3 direction, Transform pos)
    {
        XL_Pooler.instance.PopPosition("SimoHeal", pos.position, pos);
    }
}
