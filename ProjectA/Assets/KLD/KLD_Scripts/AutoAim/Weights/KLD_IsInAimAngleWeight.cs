using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IsOnSightWeight", menuName = "KLD/Aim Weights/Is On Sight", order = 3)]
public class KLD_IsInAimAngleWeight : KLD_AimWeight
{

    Vector3 playerToZombie = Vector3.zero;

    Vector2 flatPlayerToZombie = Vector2.zero;

    [SerializeField] float maxAngle = 25f;

    [SerializeField] float inAngleScore = 0f;
    [SerializeField] float outOfAngleScore = -5000f;

    [SerializeField] bool drawRays = false;
    Vector3 ray1 = Vector3.zero;
    Vector3 ray2 = Vector3.zero;

    public override float CalculateWeight(KLD_ZombieAttributes _attributes, KLD_PlayerAttributes _playerAttributes)
    {
        playerToZombie = _attributes.transform.position - _playerAttributes.transform.position;

        flatPlayerToZombie.x = playerToZombie.x;
        flatPlayerToZombie.y = playerToZombie.z;

        if (drawRays) //this should not be there
        {
            ray1 = _playerAttributes.transform.forward;
            ray2 = _playerAttributes.transform.forward;

            ray1 = Quaternion.Euler(0f, maxAngle, 0f) * ray1;
            ray2 = Quaternion.Euler(0f, -maxAngle, 0f) * ray2;

        }

        return
        Vector2.Angle(_playerAttributes.worldAimDirection, flatPlayerToZombie) < maxAngle ?
        inAngleScore :
        outOfAngleScore;
    }
}
