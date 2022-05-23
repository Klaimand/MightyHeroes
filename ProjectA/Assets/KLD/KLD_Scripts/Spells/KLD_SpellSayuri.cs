using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSayuriSpell", menuName = "KLD/Spell/Sayuri", order = 3)]
public class KLD_SpellSayuri : KLD_Spell
{
    [System.Serializable]
    class DurationRatio
    {
        public float duration = 5f;
        public float ratio = 2f;
    }


    [SerializeField] List<DurationRatio> levels = new List<DurationRatio>();


    public override void ActivateSpell(Vector3 direction, Transform pos, int characterLevel)
    {
        XL_Pooler.instance.PopPosition("SayuriUltFX", pos.position, pos);

        pos.GetComponent<KLD_PlayerShoot>().LaunchSayuriUlt(
            levels[characterLevel].duration,
            levels[characterLevel].ratio);
    }
}