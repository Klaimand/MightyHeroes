using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XL_Slide : MonoBehaviour
{
    [SerializeField] private float slideSensitivity;
    [SerializeField] private float clampMin;
    [SerializeField] private float clampMax;
    [SerializeField] private float lerpStrength;

    [Header("Button")]
    [SerializeField] private Button[] buttons;
    private Vector3 vec3 = new Vector3();

    void Update()
    {
        if (Input.touchCount > 0)
        {
            vec3 = transform.position;
            vec3.x += Input.touches[0].deltaPosition.x * slideSensitivity;
            transform.position = vec3;
        }
        else
        {
            vec3 = transform.position;
            if (transform.position.x < clampMin)
            {
                vec3.x = Mathf.Lerp(transform.position.x, clampMin, lerpStrength);
            }
            else if (transform.position.x > clampMax)
            {
                vec3.x = Mathf.Lerp(transform.position.x, clampMax, lerpStrength);
            }
            transform.position = vec3;
        }
    }
}
