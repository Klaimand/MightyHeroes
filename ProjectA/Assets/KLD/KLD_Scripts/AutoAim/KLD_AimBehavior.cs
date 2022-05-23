using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "newAimBehavior", menuName = "KLD/Aim Behavior", order = 0)]
public class KLD_AimBehavior : ScriptableObject
{
    [SerializeField] float minScoreToAim = 0.2f;

    [SerializeField] List<WeightedWeight> weights = new List<WeightedWeight>();

    float bestZombieScore = -99999f;
    int bestZombieIndex = 0;
    float curZombieScore = -99999f;

    public KLD_ZombieAttributes GetZombieToTarget(List<KLD_ZombieAttributes> _zombieAttributes, KLD_PlayerAttributes _playerAttributes)
    {
        if (_zombieAttributes.Count == 0) return null;

        bestZombieScore = -99999f;
        bestZombieIndex = 0;

        for (int i = 0; i < _zombieAttributes.Count; i++) //for each zombie
        {
            curZombieScore = 0f;

            foreach (var weightedWeight in weights)
            {
                if (weightedWeight.weight != null)
                {
                    curZombieScore +=
                    weightedWeight.weight.CalculateWeight(_zombieAttributes[i], _playerAttributes) * weightedWeight.coef;
                }
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
        Debug.Log("retuen null");
        return null;
    }

    [System.Serializable]
    class WeightedWeight
    {
        public float coef = 1f;
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        public KLD_AimWeight weight;
    }

}
