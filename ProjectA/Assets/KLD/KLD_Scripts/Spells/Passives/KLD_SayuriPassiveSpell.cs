using UnityEngine;

[CreateAssetMenu(fileName = "newBlastPassiveSpell", menuName = "KLD/Spell/Passive/Sayuri", order = 2)]
public class KLD_SayuriPassiveSpell : KLD_PassiveSpell
{
    KLD_PlayerController controller;
    int curLevel;
    Transform transform;

    //[SerializeField] GameObject trailGO;

    GameObject curTrailGO;

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
        transform = _initializer.controller.transform;
        KLD_EventsManager.instance.onEnemyHit += SpeedOnHit;

        curTrailGO = XL_Pooler.instance.PopPosition("SayuriTrailFX", transform.position, transform);
        controller.trailGO = curTrailGO;
        curTrailGO.SetActive(false);
    }

    void SpeedOnHit()
    {
        controller.AddBonusSpeedFor(levels[curLevel].bonusSpeedOnHit, levels[curLevel].bonusSpeedDuration);
        XL_Pooler.instance.PopPosition("SayuriPassiveFX", transform.position, transform);
    }
}
