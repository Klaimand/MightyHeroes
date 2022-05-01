using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IsInAngleWeight", menuName = "KLD/Aim Weights/Is In Angle", order = 3)]
public class KLD_IsInAimAngleWeight : KLD_AimWeight
{
    [SerializeField] float deadzone = 0.2f;

    Vector3 playerToZombie = Vector3.zero;

    //Vector2 flatPlayerToZombie = Vector2.zero;

    [SerializeField] float maxAngle = 25f;
    float angle = 0f;

    [SerializeField] float inAngleScore = 0f;
    [SerializeField] float outOfAngleScore = -5000f;
    [SerializeField] float angleScoreMultiplier = 100f;

    [SerializeField, Header("Debug"), Space(10)] bool drawRays = false;
    Vector3 ray1 = Vector3.zero;
    Vector3 ray2 = Vector3.zero;
    [SerializeField] float rayLenght = 5f;

    public override float CalculateWeight(KLD_ZombieAttributes _attributes, KLD_PlayerAttributes _playerAttributes)
    {
        if (_playerAttributes.normalizedAimInput.sqrMagnitude < deadzone * deadzone)
        {
            return 0f;
        }


        playerToZombie = _attributes.transform.position - _playerAttributes.transform.position;
        playerToZombie.y = 0f;

        //flatPlayerToZombie.x = playerToZombie.x;
        //flatPlayerToZombie.y = playerToZombie.z;

        if (drawRays) //this should not be there bc it does it for each zombie (not opti)
        {
            //ray1 = _playerAttributes.transform.forward;
            //ray2 = _playerAttributes.transform.forward;
            ray1 = _playerAttributes.worldAimDirection;
            ray2 = _playerAttributes.worldAimDirection;

            ray1 = Quaternion.Euler(0f, maxAngle, 0f) * ray1;
            ray2 = Quaternion.Euler(0f, -maxAngle, 0f) * ray2;

            ray1 *= rayLenght;
            ray2 *= rayLenght;

            Debug.DrawRay(_playerAttributes.transform.position + Vector3.up, ray1, Color.green);
            Debug.DrawRay(_playerAttributes.transform.position + Vector3.up, ray2, Color.green);
        }

        //if (_attributes.transform.gameObject.name == "Zombie") Debug.Log(Vector2.Angle(_playerAttributes.worldAimDirection, flatPlayerToZombie));

        //return
        //Vector3.Angle(_playerAttributes.worldAimDirection, playerToZombie) < maxAngle ?
        //inAngleScore :
        //outOfAngleScore;

        angle = Vector3.Angle(_playerAttributes.worldAimDirection, playerToZombie);

        if (angle < maxAngle)
        {
            return inAngleScore + (1f - (angle / maxAngle)) * angleScoreMultiplier;
        }
        else
        {
            return outOfAngleScore;
        }
    }
}
