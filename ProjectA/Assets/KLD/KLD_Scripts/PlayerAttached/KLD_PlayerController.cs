using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class KLD_PlayerController : MonoBehaviour
{
    //refs
    [SerializeField] KLD_TouchInputs inputs;
    KLD_PlayerAim playerAim;
    Rigidbody rb;
    [SerializeField] Transform scaler = null;
    [SerializeField] Transform lookAtTransform = null;

    //axis
    Vector2 rawAxis = Vector2.zero;
    [SerializeField, ReadOnly] Vector2 timedAxis = Vector2.zero;

    //controller
    public Transform refTransform = null;
    [SerializeField] float speed = 10f;
    [SerializeField] float axisDeadzone = 0.1f;

    [SerializeField] float accelerationTime = 0.3f;
    [SerializeField] float decelerationTime = 0.3f;
    //[SerializeField] float timedAxisZeroingDeadzone = 0.05f;
    float timedMagnitude = 0f;

    [SerializeField, Header("Animation")] Animator animator;
    enum LocomotionState { IDLE, RUNNING, DIE, RESPAWNING };
    LocomotionState locomotionState = LocomotionState.IDLE;

    [SerializeField] float rbVelocityDead = 0.05f;
    [SerializeField, ReadOnly]
    float forwardToAimAngle = 0f;
    float absoluteForwardToAimAngle = 0f;
    Vector3 playerToLookAtTransform = Vector3.zero;
    Quaternion scalerRotation = Quaternion.identity;



    Vector3 worldLookAtPos = Vector3.zero;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAim = GetComponent<KLD_PlayerAim>();
    }

    void Start()
    {

    }

    void Update()
    {
        ProcessAxis();

        //rb.velocity = (refTransform.right * rawAxis.x + refTransform.forward * rawAxis.y) * speed;
        rb.velocity = (refTransform.right * timedAxis.x + refTransform.forward * timedAxis.y) * speed;

        DoFeetRotation();

        AnimateLocomotionState();
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

    void DoFeetRotation()
    {
        if (rb.velocity.sqrMagnitude > rbVelocityDead * rbVelocityDead)
        {
            worldLookAtPos = rb.velocity;
            worldLookAtPos.y = 0f;
            worldLookAtPos.Normalize(); //may be useless
            transform.LookAt(transform.position + worldLookAtPos);

            playerToLookAtTransform = lookAtTransform.position - transform.position;

            forwardToAimAngle = Vector3.Angle(transform.forward, playerToLookAtTransform);
            //if its > 90 change scaler direction
            if (forwardToAimAngle > 90f)
            {
                scaler.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                scaler.localRotation = Quaternion.identity;
            }
            //scaler.rotation = scalerRotation;
        }
        else //we are not moving
        {
            //forwardToAimAngle = Vector3.SignedAngle(transform.forward, playerAim.GetPlayerAttributes().worldAimDirection, Vector3.up);
            //if (!inputs.IsJoystickPressed(1))
            //{
            //forwardToAimAngle = 0f;
            //}
            //absoluteForwardToAimAngle = Mathf.Abs(forwardToAimAngle);

            if (absoluteForwardToAimAngle > 90f)
            {

            }
        }
    }

    void AnimateLocomotionState()
    {
        if (timedAxis == Vector2.zero)
        {
            locomotionState = LocomotionState.IDLE;
        }
        else if (timedAxis != Vector2.zero)
        {
            locomotionState = LocomotionState.RUNNING;
        }

        animator.SetInteger("locomotionState", (int)locomotionState);
    }


}
