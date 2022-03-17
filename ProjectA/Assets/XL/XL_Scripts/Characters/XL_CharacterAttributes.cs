using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XL_CharacterAttributes
{
    [Header("Base Values")]
    public int level;
    public float health;
    public float movementSpeed;
    public float armor;
    public float healingTick;
    public float activeTick;

    [Header("Value growth per level : F = Flat / P = percentage")]
    [SerializeField] private float healthGrowthP;
    [SerializeField] private float movementSpeedGrowthF;
    [SerializeField] private float armorGrowthF;
    [SerializeField] private float healingTickGrowthP;
    [SerializeField] private float[] activeTickGrowthF = new float[10];

    private void Awake()
    {
        health = health * Mathf.Pow(healthGrowthP, level);
        movementSpeed = movementSpeed + movementSpeedGrowthF * level;
        armor = armor + armorGrowthF * level;
        healingTick = healingTick * Mathf.Pow(healingTickGrowthP, level);
        activeTick = activeTickGrowthF[level];
    }
}
