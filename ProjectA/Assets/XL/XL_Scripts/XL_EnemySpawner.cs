using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_EnemySpawner : MonoBehaviour
{
    [SerializeField] private string enemyName;
    [SerializeField] private float spawnCooldown;
    private GameObject enemy;

    private void Start()
    {
        StartCoroutine(SpawnCoroutine(spawnCooldown));
    }

    IEnumerator SpawnCoroutine(float t)
    {
        yield return new WaitForSeconds(t);

        enemy = XL_Pooler.instance.PopPosition(enemyName, transform.position);
        enemy.transform.LookAt(XL_GameManager.instance.players[0].transform);
        //XL_GameManager.instance.AddEnemy(enemy.GetComponent<XL_Enemy>());
        StartCoroutine(SpawnCoroutine(t));
    }
}
