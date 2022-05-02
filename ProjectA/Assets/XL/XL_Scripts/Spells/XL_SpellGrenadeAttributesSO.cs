using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell Grenade Attributes", menuName = "XL/SpellGrenadeAttributesSO", order = 3)]
public class XL_SpellGrenadeAttributesSO : ScriptableObject
{
    [Header("Base Value")]
    public int level;
    public float throwingDistance;
    public int explosionDamage;
    public float explosionRadius;
    public float minThrowingDistance;
    public float travelTime;

    [Header("Growth Value")]
    [SerializeField] private float throwingDistanceGrowthF;
    [SerializeField] private float explosionDamageGrowthF;
    [SerializeField] private float explosionRadiusGrowthF;

    public void Initialize()
    {
        throwingDistance = throwingDistance + level * throwingDistanceGrowthF;
        explosionDamage = (int)(explosionDamage + level * explosionDamageGrowthF);
        explosionRadius = explosionRadius + level * explosionRadiusGrowthF;
    }
}
