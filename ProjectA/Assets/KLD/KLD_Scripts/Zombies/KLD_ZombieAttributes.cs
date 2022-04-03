using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class KLD_ZombieAttributes
{
    public int maxHealth = 100;
    [PropertyRange(0, "maxHealth")] public int health = 100;
    public Transform transform = null;
    [ReadOnly] public float score = 0f;

    public void OnValidate() //called by KLD_Zombie OnValidate
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }
}