using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Attributes", menuName = "XL/EnemyAttributesSO", order = 0)]
public class XL_EnemyAttributesSO : ScriptableObject
{
    public float speed;
    public float maxHP;
    public float damage;
    public float attackCooldown;
}
