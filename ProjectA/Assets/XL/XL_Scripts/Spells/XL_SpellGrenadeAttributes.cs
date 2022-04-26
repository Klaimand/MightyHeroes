using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XL_SpellGrenadeAttributes
{
    public int level;
    public float throwingDistance;
    public int explosionDamage;
    public float explosionRadius;

    [SerializeField] private float throwingDistanceGrowthF;
    [SerializeField] private float explosionDamageGrowthF;
    [SerializeField] private float explosionRadiusGrowthF;

    public void Initialize()
    {
        throwingDistance = throwingDistance + level * throwingDistanceGrowthF;
        explosionDamage =(int)(explosionDamage + level * explosionDamageGrowthF);
        explosionRadius = explosionRadius + level * explosionRadiusGrowthF;
    }
}
