using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_ObjectivePointer : MonoBehaviour
{
    [SerializeField] Transform player;
    Transform target;
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
    }

    Vector3 toPosition;
    Vector3 fromPosition;
    Vector3 dir;
    float angle;
    Vector3 pointerAngle = new Vector3();

    private void Update()
    {
        toPosition = Camera.main.WorldToScreenPoint(target.transform.position);
        toPosition.x -= 740; //Screen resolution X
        toPosition.y -= 339; //Screen resultion Y
        toPosition.z = 0f;
        fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        dir = (toPosition - fromPosition.normalized);
        angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360;
        pointerAngle.z = angle;
        pointerRectTransform.localEulerAngles = pointerAngle;

        pointerRectTransform.localPosition = dir.normalized * distanceAround;
    }

    IEnumerator FindNearestCoroutine() 
    {
        target = XL_GameModeManager.instance.GetNearestObjective(player.position);
        
        yield return wait;
        StartCoroutine(FindNearestCoroutine());
    }
}
