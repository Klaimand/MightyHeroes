using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_ObjectivePointer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] RectTransform pointer;
    Transform nearestObjective;
    Vector3 playerToNearestObjective;
    Vector3 rectTransformEulerAngles = Vector3.zero;

    float angle = 0f;

    // Update is called once per frame
    void Update()
    {
        nearestObjective = XL_GameModeManager.instance.GetNearestObjective(player.position);
        playerToNearestObjective = nearestObjective.position - player.position;
        angle = Vector3.SignedAngle(player.forward, playerToNearestObjective, Vector3.up);

        angle = angle + player.rotation.eulerAngles.y + 180f;

        rectTransformEulerAngles.z = angle;
        pointer.localEulerAngles = rectTransformEulerAngles;
    }
}
