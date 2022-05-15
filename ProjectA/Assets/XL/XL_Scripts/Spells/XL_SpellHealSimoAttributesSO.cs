using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell Heal Simo Attributes", menuName = "XL/SpellHealSimoAttributesSO", order = 4)]
public class XL_SpellHealSimoAttributesSO : ScriptableObject
{
    [Header("Base Value")]
    public int level;
    public float duration;
    public int healingAmount;
    public float healingZoneRadius;

    [Header("Growth value")]
    [SerializeField] private float[] durationGrowth = new float[10];
    [SerializeField] private int healingAmountGrowthF;
    [SerializeField] private float healingZoneRadiusGrowthF;

    public void Initialize()
    {
        duration = durationGrowth[level];
        healingAmount = healingAmount + healingAmountGrowthF * level;
        healingZoneRadius = healingZoneRadius + healingZoneRadiusGrowthF * level;
    }
}
