using UnityEngine;

[CreateAssetMenu(fileName = "newBlastPassiveSpell", menuName = "KLD/Spell/Passive/Sayuri", order = 2)]
public class KLD_SayuriPassiveSpell : KLD_PassiveSpell
{
    KLD_PlayerController controller;
    int curLevel;

    [System.Serializable]
    class SayuriPassiveSpellValues
    {
        public float bonusSpeedOnHit = 8f;
        public float bonusSpeedDuration = 1f;
    }

    [SerializeField] SayuriPassiveSpellValues[] levels;

    public override void Initialize(PassiveSpellInitializer _initializer, int level)
    {
        controller = _initializer.controller;
        curLevel = level;
        KLD_EventsManager.instance.onEnemyHit += SpeedOnHit;
    }

    void SpeedOnHit()
    {
        controller.AddBonusSpeedFor(levels[curLevel].bonusSpeedOnHit, levels[curLevel].bonusSpeedDuration);
    }
}
