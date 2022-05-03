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

    [Header("Spell")]
    public GameObject spellPrefab;
    private XL_Spells spell;

    [Header("Growth Value")]
    [SerializeField] private bool isPercentageHealthGrowth;
    [SerializeField] private float healthGrowth;
    [SerializeField] private bool isPercentageMovementSpeedGrowth;
    [SerializeField] private float movementSpeedGrowth;
    [SerializeField] private bool isPercentageArmorGrowth;
    [SerializeField] private float armorGrowth;
    [SerializeField] private bool isPercentageHealingTickGrowth;
    [SerializeField] private float healingTickGrowth;
    [SerializeField] private float[] activeTickGrowth = new float[10];

    public void Initialize()
    {
        
        spell = spellPrefab.GetComponent<XL_Spells>();

        if (isPercentageHealthGrowth) healthMax = healthMax * Mathf.Pow(healthGrowth, level);
        else healthMax = healthMax + healthGrowth * level;

        if (isPercentageMovementSpeedGrowth) movementSpeed = movementSpeed * Mathf.Pow(movementSpeedGrowth, level);
        else movementSpeed = movementSpeed + movementSpeedGrowth * level;

        if (isPercentageArmorGrowth) armor = armor * Mathf.Pow(armorGrowth, level);
        else armor = armor + armorGrowth * level;

        if (isPercentageHealingTickGrowth) healingTick = healingTick * Mathf.Pow(healingTickGrowth, level);
        else healingTick = healingTick + healingTickGrowth * level;

        activeTick = activeTickGrowth[level];
    }

    public void ActivateSpell(Vector3 direction, Transform pos)
    {
        spell.ActivateSpell(direction, pos);
    }

}
