using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "newAimBehavior", menuName = "KLD/Aim Behavior", order = 0)]
public class KLD_AimBehavior : ScriptableObject
{
    [SerializeField] float minScoreToAim = 0.2f;

    [SerializeField] List<WeightedWeight> weights = new List<WeightedWeight>();

    float bestZombieScore = 0;
    int bestZombieIndex = 0;
    float curZombieScore = 0;

    public KLD_ZombieAttributes GetZombieToTarget(List<KLD_ZombieAttributes> _zombieAttributes, Transform _player)
    {
        if (_zombieAttributes.Count == 0) return null;

        bestZombieScore = 0;
        bestZombieIndex = 0;

        for (int i = 0; i < _zombieAttributes.Count; i++) //for each zombie
        {
            curZombieScore = 0;

            foreach (var weightedWeight in weights)
            {
                curZombieScore +=
                weightedWeight.weight.CalculateWeight(_zombieAttributes[i], _player) * weightedWeight.coef;
            }

            _zombieAttributes[i].score = curZombieScore;

            if (curZombieScore > bestZombieScore)
            {
                bestZombieScore = curZombieScore;
                bestZombieIndex = i;
            }
        }

        if (bestZombieScore > minScoreToAim)
        {
            return _zombieAttributes[bestZombieIndex];
        }
        return null;
    }

    [System.Serializable]
    class WeightedWeight
    {
        public float coef = 1f;
        public KLD_AimWeight weight;
    }

}
