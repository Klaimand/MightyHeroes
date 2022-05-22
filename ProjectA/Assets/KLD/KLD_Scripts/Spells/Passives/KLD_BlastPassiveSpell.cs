using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newBlastPassiveSpell", menuName = "KLD/Spell/Passive/Blast", order = 0)]
public class KLD_BlastPassiveSpell : KLD_PassiveSpell
{
    XL_Characters character;
    int curLevel;
    Transform transform;

    [System.Serializable]
    class BlastPassiveLevels
    {
        public float healPerKill = 10;
    }

    [SerializeField] BlastPassiveLevels[] levels;

    public override void Initialize(PassiveSpellInitializer _initializer, int level)
    {
        character = _initializer.character;
        curLevel = level;
        transform = _initializer.character.transform;
        KLD_EventsManager.instance.onEnemyKill += HealOnKill;
    }

    void HealOnKill(Enemy enemy)
    {
        character.TakeDamage(-levels[curLevel].healPerKill);
        XL_Pooler.instance.PopPosition("BlastPassiveFX", transform.position, transform);
    }
}
