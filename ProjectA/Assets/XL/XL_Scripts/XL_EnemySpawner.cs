using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_EnemySpawner : MonoBehaviour
{
    [SerializeField] private string enemyName;
    [SerializeField] private float spawnCooldown;

    private void Start()
    {
        StartCoroutine(SpawnCoroutine(spawnCooldown));
    }

    IEnumerator SpawnCoroutine(float t) 
    {
        yield return new WaitForSeconds(t);
        
        XL_GameManager.instance.AddEnemy(XL_Pooler.instance.PopPosition(enemyName, transform.position).GetComponent<XL_Enemy>().GetZombieAttributes());
        StartCoroutine(SpawnCoroutine(t));
    }
}
