using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_ObjectivePointer : MonoBehaviour
{
    [SerializeField] Transform player;
    Transform target;
    [SerializeField] Transform refTransform;

    [SerializeField] RectTransform pointerRectTransform;
    [SerializeField] private float distanceAround;

    [SerializeField] private float refreshTime;
    private WaitForSeconds wait;

    private void Awake()
    {
        wait = new WaitForSeconds(refreshTime);
    }

    private void Start()
    {
        StartCoroutine(FindNearestCoroutine());
        player = XL_GameManager.instance.players[0].transform;
    }

    Vector3 toPosition;
    Vector3 fromPosition;
    Vector3 dir;
    float angle;
    Vector3 pointerAngle = new Vector3();

    Vector3 flattedEuler;

    private void Update()
    {
        dir = Quaternion.Euler(refTransform.rotation.eulerAngles.x, -refTransform.rotation.eulerAngles.y, refTransform.rotation.eulerAngles.z) * (target.position - player.position);
        dir.y = dir.z;
        angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360;
        pointerAngle.z = angle;

        flattedEuler.x = 0f;
        flattedEuler.y = pointerAngle.z;
        flattedEuler.z = 0f;

        pointerRectTransform.eulerAngles = pointerAngle;


        pointerRectTransform.localPosition = dir.normalized * distanceAround;
    }

    IEnumerator FindNearestCoroutine()
    {
        target = XL_GameModeManager.instance.GetNearestObjective(player.position);

        yield return wait;
        StartCoroutine(FindNearestCoroutine());
    }
}
