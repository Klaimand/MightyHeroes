using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_PlayerController : MonoBehaviour
{
    //refs
    Rigidbody rb;

    //axis
    Vector2 rawAxis = Vector2.zero;

    //controller
    [SerializeField] Transform refTransform = null;
    [SerializeField] float speed = 10f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    void Update()
    {
        ProcessAxis();

        rb.velocity = (refTransform.right * rawAxis.x + refTransform.forward * rawAxis.y) * speed;
    }

    void ProcessAxis()
    {
        rawAxis.x = Input.GetAxisRaw("Horizontal");
        rawAxis.y = Input.GetAxisRaw("Vertical");

        if (rawAxis.sqrMagnitude > 1f)
        {
            rawAxis.Normalize();
        }
    }
}
