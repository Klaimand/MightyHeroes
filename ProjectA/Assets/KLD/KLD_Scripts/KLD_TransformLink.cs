using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class KLD_TransformLink : MonoBehaviour
{
    [SerializeField] Transform linkTo = null;
    [SerializeField] Vector3 offset = Vector3.zero;

    [SerializeField] bool lockRotation = false;
    [SerializeField, ShowIf("lockRotation")] Vector3 angleOffset = Vector3.zero;
    [SerializeField, ShowIf("lockRotation")] bool lockX = false, lockY = false, lockZ = false;

    Vector3 angles = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (linkTo == null)
        {
            Debug.LogError("Transform Link missing transform");
            return;
        }

        transform.position = linkTo.position + offset;
        if (lockRotation)
        {
            angles.x = lockX ? linkTo.rotation.eulerAngles.x + angleOffset.x : transform.rotation.eulerAngles.x;
            angles.y = lockY ? linkTo.rotation.eulerAngles.y + angleOffset.y : transform.rotation.eulerAngles.y;
            angles.z = lockZ ? linkTo.rotation.eulerAngles.z + angleOffset.z : transform.rotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(angles);
        }
    }
}
