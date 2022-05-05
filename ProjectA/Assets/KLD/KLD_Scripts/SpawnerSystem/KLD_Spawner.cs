using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class KLD_Spawner : MonoBehaviour
{
    [SerializeField] KLD_SpawnerStep[] steps;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

    public KLD_EnemyProba[] enemiesProbas;

    public void OnValidate()
    {
        for (int y = 0; y < 2; y++)
        {
            for (int i = enemiesProbas.Length - 1; i >= 0; i--)
            {
                float minValue = i == 0 ? 0f : enemiesProbas[i - 1].proba - 0.05f;
                float maxValue = i >= enemiesProbas.Length - 1 ? 1f : enemiesProbas[i + 1].proba;

                enemiesProbas[i].proba = Mathf.Clamp(enemiesProbas[i].proba, minValue, maxValue);
                /*
                if (i != 0 && enemiesProbas[i].proba < enemiesProbas[i - 1].proba - 0.03f)
                {
                    enemiesProbas[i].proba = enemiesProbas[i - 1].proba;
                }
                if (i < enemiesProbas.Length - 1 && enemiesProbas[i].proba > enemiesProbas[i + 1].proba)
                {
                    enemiesProbas[i].proba = enemiesProbas[i + 1].proba;
                }
                */
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
