using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSL_AimBodyRotation : MonoBehaviour
{

    [SerializeField] Transform targetTransform;
    [SerializeField] Transform aimTransform;
    [SerializeField] Transform bone;

    [SerializeField] float angleLimit = 90.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = targetTransform.position;
        AimAtTarget(bone, targetPosition);

    }
    private void AimAtTarget(Transform bone, Vector3 targetPosition)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        bone.rotation = aimTowards * bone.rotation;
    }
}
