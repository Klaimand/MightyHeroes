using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class KLD_Spawner : MonoBehaviour
{
    [SerializeField] float spawnRadius = 3f;
    [SerializeField] float playerMinDist = 10f;
    [SerializeField] KLD_SpawnerStep[] steps;

    float nextSpawnTime = 0f;
    int curStepIndex = 0;
    float spawnAngle = 0f;
    Vector3 spawnVector = Vector3.zero;
    Quaternion q;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (KLD_SpawnersManager.Instance.levelTime >= nextSpawnTime)
        {
            curStepIndex = GetCurStepIndex();

            nextSpawnTime = KLD_SpawnersManager.Instance.levelTime +
            Random.Range(steps[curStepIndex].minMaxTimeBetweenSpawns.x, steps[curStepIndex].minMaxTimeBetweenSpawns.y);

            SpawnEnemies(steps[curStepIndex].GetRandomProba());
        }
    }

    void SpawnEnemies(KLD_EnemyProba _proba)
    {
        if (_proba.enemyPerSpawn == 0) return;

        spawnAngle = 360f / _proba.enemyPerSpawn;
        spawnVector = Vector3.right * spawnRadius;
        q = Quaternion.Euler(0f, spawnAngle, 0f);
        for (int i = 0; i < _proba.enemyPerSpawn; i++)
        {
            if (KLD_SpawnersManager.Instance.CanSpawn(transform.position, playerMinDist))
            {
                XL_Pooler.instance.PopPosition(_proba.enemy, transform.position + spawnVector);
                spawnVector = q * spawnVector;
            }
        }
    }

    int GetCurStepIndex()
    {
        for (int i = steps.Length - 1; i >= 0; i--)
        {
            if (KLD_SpawnersManager.Instance.levelTime >= steps[i].timeToActivate)
            {
                return i;
            }
        }
        return 0;
    }

    void OnValidate()
    {
        if (steps == null || steps.Length == 0) return;

        steps[0].timeToActivate = 0f;

        foreach (var step in steps)
        {
            if (step.enemiesProbas == null || step.enemiesProbas.Length == 0) continue;

            step.enemiesProbas[step.enemiesProbas.Length - 1].proba = 1f;

            if (step.timeToActivate < 0f) step.timeToActivate = 0f;

            step.OnValidate();
        }
    }
}

[System.Serializable]
public class KLD_SpawnerStep
{
    public float timeToActivate = 0f;

    public Vector2 minMaxTimeBetweenSpawns = new Vector2(10f, 15f);

    public KLD_EnemyProba[] enemiesProbas;

    float randomValue = 0f;

    public KLD_EnemyProba GetRandomProba()
    {
        randomValue = Random.value;

        for (int i = 0; i < enemiesProbas.Length; i++)
        {
            if (randomValue <= enemiesProbas[i].proba)
            {
                return enemiesProbas[i];
            }
        }

        return enemiesProbas[0];
    }

    public void OnValidate()
    {
        for (int y = 0; y < 2; y++)
        {
            for (int i = enemiesProbas.Length - 1; i >= 0; i--)
            {
                float minValue = i == 0 ? 0f : enemiesProbas[i - 1].proba - 0.05f;
                float maxValue = i >= enemiesProbas.Length - 1 ? 1f : enemiesProbas[i + 1].proba;

                enemiesProbas[i].proba = Mathf.Clamp(enemiesProbas[i].proba, minValue, maxValue);
            }
        }
    }
}

[System.Serializable]
public class KLD_EnemyProba
{
    [Range(0f, 1f)] public float proba = 0.5f;

    [ValueDropdown("EnemiesKeys")]
    public string enemy = "Swarmer";

    public int enemyPerSpawn = 3;

    string[] EnemiesKeys = { "Swarmer", "Kamikaze", "Alpha" };

}
