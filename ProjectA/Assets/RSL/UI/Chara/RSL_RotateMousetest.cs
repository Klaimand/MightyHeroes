using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSL_RotateMousetest : MonoBehaviour
{
    [SerializeField] float rotSpeed = 1f;
    private void OnMouseDrag()
    {
        float XaxisRot = Input.GetAxis("Mouse X") * rotSpeed;
        transform.Rotate(Vector3.down, XaxisRot);
    }
}
