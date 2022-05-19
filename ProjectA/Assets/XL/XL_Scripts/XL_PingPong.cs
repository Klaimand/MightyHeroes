using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_PingPong : MonoBehaviour
{
    [SerializeField] private Vector3 move;
    [SerializeField] private float moveSpeed;

    [Header("Material")]
    [SerializeField] private bool hasMaterial;
    [SerializeField] private Material material;
    [SerializeField] private float alphaFade;

    private Vector3 startPosition;
    private Vector3 vec3 = new Vector3();

    private Vector4 vec4;
    private float a;

    private void Awake()
    {
        startPosition = transform.position;
        if (hasMaterial) 
        {
            a = material.color.a;
            vec4 = material.color;
        }
        
    }

    void Update()
    {
        if (hasMaterial) 
        {
            vec4.w = a - Mathf.Sin(Time.time * moveSpeed + Mathf.PI) * alphaFade;
            material.color = vec4;
        }
        vec3.x = startPosition.x + Mathf.Sin(Time.time * moveSpeed) * move.x;
        vec3.y = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * move.y;
        vec3.z = startPosition.z + Mathf.Sin(Time.time * moveSpeed) * move.z;
        transform.position = vec3;
    }
}
