using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSpellHeal", menuName = "KLD/Spell/Heal", order = 0)]
public class KLD_SpellHeal : KLD_Spell
{
    [System.Serializable]
    class HealZoneAttributes
    {
        public float duration = 5f;
        public float tickDuration = 0.2f;
        public float healingPerTick = 20f;
    }

    [SerializeField] List<HealZoneAttributes> levels = new List<HealZoneAttributes>();

    GameObject curHealZone;

    public override void ActivateSpell(Vector3 direction, Transform pos, int characterLevel)
    {
        curHealZone = XL_Pooler.instance.PopPosition("SimoHeal", pos.position, pos);

        curHealZone.GetComponent<KLD_HealZone>().SetHealZoneAttributes(
            levels[characterLevel].duration,
            levels[characterLevel].tickDuration,
            levels[characterLevel].healingPerTick
        );
    }
}
