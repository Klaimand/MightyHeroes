using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSL_DummyController : MonoBehaviour
{
    Animator animator;
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
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        ProcessAxis();    
    }

    private void FixedUpdate()
    {
        rb.velocity = (refTransform.right * rawAxis.x + refTransform.forward * rawAxis.y) * speed;

        if (rb.velocity == new Vector3(0, 0, 0))
        {
            animator.Play("Iddle");
        }
        else
        {
            animator.Play("Base Layer.WalkCycle");
        }
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
