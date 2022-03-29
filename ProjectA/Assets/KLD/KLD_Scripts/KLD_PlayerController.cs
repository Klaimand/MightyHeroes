using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class KLD_PlayerController : MonoBehaviour
{
    [SerializeField] KLD_TouchInputs inputs;
    //refs
    Rigidbody rb;

    //axis
    Vector2 rawAxis = Vector2.zero;
    [SerializeField, ReadOnly] Vector2 timedAxis = Vector2.zero;

    //controller
    [SerializeField] Transform refTransform = null;
    [SerializeField] float speed = 10f;
    [SerializeField] float axisDeadzone = 0.1f;

    [SerializeField] float accelerationTime = 0.3f;
    [SerializeField] float decelerationTime = 0.3f;
    //[SerializeField] float timedAxisZeroingDeadzone = 0.05f;
    float timedMagnitude = 0f;

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

        //rb.velocity = (refTransform.right * rawAxis.x + refTransform.forward * rawAxis.y) * speed;
        rb.velocity = (refTransform.right * timedAxis.x + refTransform.forward * timedAxis.y) * speed;
    }

    void ProcessAxis()
    {
        //rawAxis.x = Input.GetAxisRaw("Horizontal");
        //rawAxis.y = Input.GetAxisRaw("Vertical");
        rawAxis.x = inputs.GetJoystickNormalizedVector(0).x;
        rawAxis.y = inputs.GetJoystickNormalizedVector(0).y;

        rawAxis = rawAxis.sqrMagnitude > axisDeadzone * axisDeadzone ? rawAxis.normalized : Vector2.zero;

        timedMagnitude += 1f / (rawAxis != Vector2.zero ? accelerationTime : -decelerationTime) * Time.deltaTime;

        timedMagnitude = Mathf.Clamp01(timedMagnitude);

        timedAxis = rawAxis != Vector2.zero ?
         rawAxis.normalized * timedMagnitude :
         timedAxis.normalized * timedMagnitude;
    }
}
