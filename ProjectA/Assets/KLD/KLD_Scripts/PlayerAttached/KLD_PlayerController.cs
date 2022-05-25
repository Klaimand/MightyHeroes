using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Animations.Rigging;

public enum PlayerState { DEFAULT, WIN, LOOSE, DEAD };

public class KLD_PlayerController : MonoBehaviour
{
    //refs
    [SerializeField] KLD_TouchInputs inputs;
    [SerializeField] KLD_PlayerShoot playerShoot;
    KLD_PlayerAim playerAim;
    Rigidbody rb;
    [SerializeField] Transform scaler = null;
    [SerializeField] Transform lookAtTransform = null;

    //axis
    Vector2 rawAxis = Vector2.zero;
    [SerializeField, ReadOnly] Vector2 timedAxis = Vector2.zero;

    //controller
    public Transform refTransform = null;
    //[SerializeField] float speed = 10f;
    [SerializeField] float axisDeadzone = 0.1f;

    [SerializeField] float accelerationTime = 0.3f;
    [SerializeField] float decelerationTime = 0.3f;
    //[SerializeField] float timedAxisZeroingDeadzone = 0.05f;
    float timedMagnitude = 0f;

    [SerializeField, Header("Animation")] Animator animator;
    enum LocomotionState { IDLE, RUNNING, DIE, RESPAWNING, RUNNING_BACKWARD, WIN };
    LocomotionState locomotionState = LocomotionState.IDLE;

    [SerializeField] float rbVelocityDead = 0.05f;
    [SerializeField, ReadOnly]
    float forwardToAimAngle = 0f;
    [SerializeField, ReadOnly]
    float absoluteForwardToAimAngle = 0f;
    Vector3 playerToLookAtTransform = Vector3.zero;
    bool runningBackward = false;
    Vector3 eulerRotationToDo = Vector3.zero;

    Vector3 worldLookAtPos = Vector3.zero;

    float realSpeed = 0f;
    float baseSpeed = 0f;
    float bonusSpeed = 0f;
    float speedRatio = 0f;

    [HideInInspector] public float curAngleOffset = 0f;

    //[HideInInspector] public GameObject trailGO;
    ParticleSystem trailPS;

    //RigBuilder builder;
    //MultiAimConstraint multiAimConstraint;

    bool spawning = true;

    PlayerState playerState = PlayerState.DEFAULT;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAim = GetComponent<KLD_PlayerAim>();
    }

    void Start()
    {
        spawning = true;
    }

    void Update()
    {
        ProcessAxis();

        ProcessBonusSpeed();

        if (playerState == PlayerState.DEFAULT)
        {
            rb.velocity = (refTransform.right * rawAxis.x + refTransform.forward * rawAxis.y) * realSpeed;
        }
        else
        {
            if (playerState == PlayerState.WIN)
            {
                transform.rotation = Quaternion.identity;
            }
            rb.velocity = Vector3.zero;
        }
        //rb.velocity = (refTransform.right * timedAxis.x + refTransform.forward * timedAxis.y) * speed *
        //(runningBackward ? -1f : 1f);

        //DoFeetRotation();

        AnimateLocomotionState();
    }

    void FixedUpdate()
    {
        DoFeetRotation();
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
        playerToLookAtTransform = lookAtTransform.position - transform.position;

        //Debug.DrawRay(transform.position + Vector3.up, playerToLookAtTransform, Color.cyan);
        //Debug.DrawRay(transform.position + Vector3.up, transform.forward * 5f, Color.magenta);

        forwardToAimAngle = Vector3.SignedAngle(transform.forward, playerToLookAtTransform, Vector3.up);
        //forwardToAimAngle -= 30f;
        forwardToAimAngle -= curAngleOffset;
        absoluteForwardToAimAngle = Mathf.Abs(forwardToAimAngle); //+ curAngleOffset;

        //if (forwardToAimAngle > 0f)
        //{
        //    absoluteForwardToAimAngle -= curAngleOffset;
        //}

        if (rb.velocity.sqrMagnitude > rbVelocityDead * rbVelocityDead)
        {
            #region old buggy scaler
            //if its > 90 change scaler direction
            /*
            if (absoluteForwardToAimAngle > 90f)
            {
                runningBackward = true;
                scaler.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                runningBackward = false;
                scaler.localRotation = Quaternion.identity;
            }

            worldLookAtPos = rb.velocity;
            worldLookAtPos.y = 0f;
            worldLookAtPos.Normalize(); //may be useless

            transform.LookAt((transform.position + worldLookAtPos)); //* (runningBackward ? -1f : 1f));

            //scaler.rotation = scalerRotation;
            */
            #endregion


            if (playerShoot.isAiming)
            {
                transform.LookAt(playerAim.GetTargetPos());
            }
            else
            {
                transform.LookAt(transform.position + rb.velocity);
            }

            if (Vector3.Angle(transform.forward, rb.velocity) > 90f)
            {
                runningBackward = true;
            }
            else
            {
                runningBackward = false;
            }
        }
        else //we are not moving
        {
            runningBackward = false;
            //scaler.localRotation = Quaternion.identity;

            if (absoluteForwardToAimAngle > 60f)
            {
                eulerRotationToDo.x = 0f;
                eulerRotationToDo.y = ((absoluteForwardToAimAngle - 60f) * Mathf.Sign(forwardToAimAngle));
                eulerRotationToDo.z = 0f;
                transform.Rotate(eulerRotationToDo);
            }
            runningBackward = false;
        }
    }

    void AnimateLocomotionState()
    {
        if (spawning)
        {
            locomotionState = LocomotionState.RESPAWNING;
        }
        else if (playerState != PlayerState.DEFAULT)
        {
            if (playerState == PlayerState.DEAD)
            {
                locomotionState = LocomotionState.DIE;
            }
            else if (playerState == PlayerState.LOOSE)
            {
                locomotionState = LocomotionState.IDLE;
            }
            else if (playerState == PlayerState.WIN)
            {
                locomotionState = LocomotionState.WIN;
            }
        }
        else if (timedAxis == Vector2.zero)
        {
            locomotionState = LocomotionState.IDLE;
        }
        else if (timedAxis != Vector2.zero)
        {
            locomotionState = runningBackward ? LocomotionState.RUNNING_BACKWARD : LocomotionState.RUNNING;
        }

        animator.SetInteger("locomotionState", (int)locomotionState);
    }

    public bool IsRunning()
    {
        return rawAxis != Vector2.zero;
    }

    public void SetCharacterMeshComponents(Animator _animator, Transform _scaler)
    {
        animator = _animator;
        scaler = _scaler;
    }

    void CalculateRealSpeed()
    {
        realSpeed = ((baseSpeed + bonusSpeed) * speedRatio) / 3.34f;
        //realSpeed = ((baseSpeed * speedRatio) + bonusSpeed) / 3.34f;
    }

    public void SetBaseSpeed(float _baseSpeed)
    {
        baseSpeed = _baseSpeed;
        CalculateRealSpeed();
    }

    //public void SetBonusSpeed(float _bonusSpeed)
    //{
    //    bonusSpeed = _bonusSpeed;
    //    CalculateRealSpeed();
    //}

    public void SetSpeedRatio(float _speedRatio)
    {
        if (_speedRatio != speedRatio)
        {
            speedRatio = _speedRatio;
            CalculateRealSpeed();
        }
    }

    float bonusSpeedDuration = 0f;
    bool isBonusSpeeded = false;
    public void AddBonusSpeedFor(float _bonusSpeed, float _duration)
    {
        //if (trailGO != null) { trailGO.SetActive(true); }
        if (!isBonusSpeeded && trailPS != null)
        {
            //print("started PS");
            trailPS.Play();
        }
        isBonusSpeeded = true;
        bonusSpeed = _bonusSpeed;
        bonusSpeedDuration = _duration;
        CalculateRealSpeed();
    }

    void ProcessBonusSpeed()
    {
        if (isBonusSpeeded)
        {
            bonusSpeedDuration -= Time.deltaTime;
            if (bonusSpeedDuration <= 0f)
            {
                //if (trailGO != null) { trailGO.SetActive(false); }
                if (trailPS != null)
                {
                    //print("stopped PS");
                    trailPS.Stop();
                }
                isBonusSpeeded = false;
                bonusSpeed = 0f;
                CalculateRealSpeed();
            }
        }
    }

    public void SetTrail(GameObject _trail)
    {
        trailPS = _trail.transform.GetChild(0).GetComponent<ParticleSystem>();
        //trailPS.Stop();
    }

    public void DoSpawnAnimation()
    {
        spawning = false;
        //if (animator != null)
        //{
        //    animator.SetTrigger("spawn");
        //}
    }

    public void SetPlayerState(PlayerState _playerState)
    {
        playerState = _playerState;
    }
}
