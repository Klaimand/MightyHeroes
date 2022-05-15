using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Slide : MonoBehaviour
{
    [SerializeField] private float slideSensitivity;
    private Vector3 vec3 = new Vector3();

    void Update()
    {
        if (Input.touchCount > 0)
        {
            vec3 = transform.position;
            vec3.x += Input.touches[0].deltaPosition.x * slideSensitivity;
            transform.position = vec3;
        }
    }
}
