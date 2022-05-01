using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XL_SpellHealSimoAttributes
{
    public int level;
    public float duration;
    public int healingAmount;
    public float healingZoneRadius;

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
