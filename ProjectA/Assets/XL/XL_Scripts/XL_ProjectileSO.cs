using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Attributes", menuName = "XL/ProjectileSO", order = 1)]
public class XL_ProjectileSO : ScriptableObject
{
    public string projectileName; //name in XL_Pooler
    public float range;
    public int damage;
}
