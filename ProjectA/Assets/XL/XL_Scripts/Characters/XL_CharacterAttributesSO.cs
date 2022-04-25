using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Attributes", menuName = "XL/CharacterAttributesSO", order = 2)]
public class XL_CharacterAttributesSO : ScriptableObject
{

    [Header("Base Values")]
    public int level;
    public float healthMax;
    public float movementSpeed;
    public float armor;
    public float healingTick;
    public float activeTick;

    [Header("Value growth per level : F = Flat / P = percentage")]
    [SerializeField] private bool isPercentageHealthGrowth;
    [SerializeField] private float healthGrowthP;
    [SerializeField] private bool isPercentageMovementSpeedGrowth;
    [SerializeField] private float movementSpeedGrowthF;
    [SerializeField] private bool isPercentageArmorGrowth;
    [SerializeField] private float armorGrowthF;
    [SerializeField] private bool isPercentageHealingTickGrowth;
    [SerializeField] private float healingTickGrowthP;
    [SerializeField] private float[] activeTickGrowthF = new float[10];

    public void Initialize()
    {
        if (isPercentageHealthGrowth) healthMax = healthMax * Mathf.Pow(healthGrowthP, level);
        else healthMax = healthMax + healthGrowthP * level;

        if (isPercentageMovementSpeedGrowth) movementSpeed = movementSpeed * Mathf.Pow(movementSpeedGrowthF, level);
        else movementSpeed = movementSpeed + movementSpeedGrowthF * level;

        if (isPercentageArmorGrowth) armor = armor * Mathf.Pow(armorGrowthF, level);
        else armor = armor + armorGrowthF * level;

        if (isPercentageHealingTickGrowth) healingTick = healingTick * Mathf.Pow(healingTickGrowthP, level);
        else healingTick = healingTick + healingTickGrowthP * level;

        activeTick = activeTickGrowthF[level];
    }

}
