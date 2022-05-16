using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Character Attributes", menuName = "XL/CharacterAttributesSO", order = 2)]
public class XL_CharacterAttributesSO : ScriptableObject
{
    [Header("Name")]
    [SerializeField] public string characterName;

    [Header("Base Values")]
    [SerializeField] float base_healthMax;
    [SerializeField] float base_movementSpeed;
    [SerializeField] float base_armor;
    [SerializeField] float base_healingTick;
    //[SerializeField] float base_activeTick;
    [SerializeField] private float[] activeTickGrowth = new float[10];

    [Header("Level")]
    public int level;

    [Header("Scaled Values")]
    [ReadOnly] public float healthMax;
    [ReadOnly] public float movementSpeed;
    [ReadOnly] public float armor;
    [ReadOnly] public float healingTick;
    [ReadOnly] public float activeTick;

    [Header("Growth Value")]
    [SerializeField] private bool isPercentageHealthGrowth;
    [SerializeField] private float healthGrowth;
    [SerializeField] private bool isPercentageMovementSpeedGrowth;
    [SerializeField] private float movementSpeedGrowth;
    [SerializeField] private bool isPercentageArmorGrowth;
    [SerializeField] private float armorGrowth;
    [SerializeField] private bool isPercentageHealingTickGrowth;
    [SerializeField] private float healingTickGrowth;

    [Header("Spell")]
    //public GameObject spellPrefab;
    //private XL_Spells spell;
    [SerializeField] KLD_Spell spellSO;
    public float spellLaunchDuration = 1.2f;
    public bool spellIsButton = true;
    public bool canUseSpellWhenReloading = false;

    [Header("Mesh")]
    public GameObject characterMesh;

    public void Initialize()
    {
        if (isPercentageHealthGrowth) healthMax = Mathf.Round(base_healthMax * Mathf.Pow(healthGrowth, level));
        else healthMax = base_healthMax + healthGrowth * level;

        if (isPercentageMovementSpeedGrowth) movementSpeed = Mathf.Round(base_movementSpeed * Mathf.Pow(movementSpeedGrowth, level));
        else movementSpeed = base_movementSpeed + movementSpeedGrowth * level;

        if (isPercentageArmorGrowth) armor = Mathf.Round(base_armor * Mathf.Pow(armorGrowth, level));
        else armor = base_armor + armorGrowth * level;

        if (isPercentageHealingTickGrowth) healingTick = Mathf.Round(base_healingTick * Mathf.Pow(healingTickGrowth, level));
        else healingTick = base_healingTick + healingTickGrowth * level;

        activeTick = activeTickGrowth[level];
    }

    public void ActivateSpell(Vector3 direction, Transform pos)
    {
        spellSO.ActivateSpell(direction, pos, level);
    }

    public void CallOnSpellLaunch()
    {
        spellSO.OnSpellLaunch();
    }

    public void CallUltJoystickDown(Vector2 _joyDirection, Transform _player)
    {
        spellSO.OnUltJoystickDown(_joyDirection, _player);
    }
}
