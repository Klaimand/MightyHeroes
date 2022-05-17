using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Spell Grenade Attributes", menuName = "XL/SpellGrenadeAttributesSO", order = 3)]
public class XL_SpellGrenadeAttributesSO : ScriptableObject
{
    [Header("Base Value")]
    public int level;
    public float base_throwingDistance;
    public int base_explosionDamage;
    public float base_explosionRadius;
    public float minThrowingDistance;
    public float travelTime;

    [Header("Growth Value")]
    [SerializeField] private float throwingDistanceGrowthF;
    [SerializeField] private float explosionDamageGrowthF;
    [SerializeField] private float explosionRadiusGrowthF;

    [Header("Scaled Values")]
    [ReadOnly] public float throwingDistance;
    [ReadOnly] public int explosionDamage;
    [ReadOnly] public float explosionRadius;

    public void Initialize()
    {
        throwingDistance = base_throwingDistance + level * throwingDistanceGrowthF;
        explosionDamage = (int)(base_explosionDamage + level * explosionDamageGrowthF);
        explosionRadius = base_explosionRadius + level * explosionRadiusGrowthF;
    }
}
