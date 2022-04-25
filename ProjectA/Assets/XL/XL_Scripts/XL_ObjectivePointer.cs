using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_ObjectivePointer : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] RectTransform pointerRectTransform;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Objective").transform;
    }

    Vector3 toPosition;
    Vector3 fromPosition;
    Vector3 dir;
    float angle;
    Vector3 pointerAngle = new Vector3();

    private void Update()
    {
        toPosition = Camera.main.WorldToScreenPoint(target.transform.position);
        Debug.Log(toPosition + " -> toPosition ScreenSpace");
        toPosition.z = 0f;
        fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        dir = (toPosition - fromPosition.normalized);
        angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360;
        pointerAngle.z = angle;
        pointerRectTransform.localEulerAngles = pointerAngle;
        //pointerRectTransform.rotation = Quaternion.FromToRotation(fromPosition, toPosition);
        //pointerRectTransform.eulerAngles = new Vector3(0, 0, pointerRectTransform.rotation.z);
    }
}
