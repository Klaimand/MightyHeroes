using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XL_SpellGrenadeAttributes
{
    public int level;
    public float throwingDistance;
    public float explosionDamage;
    public float explosionRadius;

    [SerializeField] private float throwingDistanceGrowthF;
    [SerializeField] private float explosionDamageGrowthF;
    [SerializeField] private float explosionRadiusGrowthF;

    public void Initialize()
    {
        throwingDistance = throwingDistance + level * throwingDistanceGrowthF;
        explosionDamage = explosionDamage + level * explosionDamageGrowthF;
        explosionRadius = explosionRadiusGrowthF + level * explosionRadiusGrowthF;
    }
}
