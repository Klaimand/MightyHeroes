using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class XL_Slide : MonoBehaviour
{

    [SerializeField] private float slideSensitivity;
    [SerializeField] private float clampMin;
    [SerializeField] private float clampMax;
    [SerializeField] private float lerpStrength;
    [SerializeField] private float deactivateThreshold;

    [Header("Button")]
    [SerializeField] private Button[] buttons;
    private Vector3 vec3 = new Vector3();

    private bool hasMoved = false;
    private Vector2 inputStartPosition;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (!hasMoved) inputStartPosition = Input.touches[0].position;
            hasMoved = true;

            vec3 = transform.position;
            vec3.x += Input.touches[0].deltaPosition.x * slideSensitivity;
            transform.position = vec3;

            if (Mathf.Abs(Input.touches[0].position.x - inputStartPosition.x) > deactivateThreshold)
            {
                foreach (Button b in buttons) 
                {
                    b.interactable = false;
                }
            }
        }
        else
        {
            hasMoved = false;

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

            foreach (Button b in buttons)
            {
                b.interactable = true;
            }
        }
    }
}
