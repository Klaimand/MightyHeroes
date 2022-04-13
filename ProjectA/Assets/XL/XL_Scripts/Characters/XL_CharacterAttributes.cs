using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XL_CharacterAttributes 
{
    [Header("Base Values")]
    public int level;
    public float healthMax;
    public float health;
    public float movementSpeed;
    public float armor;
    public int healingTick;
    public float activeTick;

    [Header("Value growth per level : F = Flat / P = percentage")]
    [SerializeField] private float healthGrowthP;
    [SerializeField] private float movementSpeedGrowthF;
    [SerializeField] private float armorGrowthF;
    [SerializeField] private float healingTickGrowthP;
    [SerializeField] private float[] activeTickGrowthF = new float[10];

    public void Initialize()
    {
        healthMax = healthMax * Mathf.Pow(healthGrowthP, level);
        health = healthMax;
        movementSpeed = movementSpeed + movementSpeedGrowthF * level;
        armor = armor + armorGrowthF * level;
        healingTick = healingTick * (int)Mathf.Pow(healingTickGrowthP, level);
        activeTick = activeTickGrowthF[level];
    }
}
