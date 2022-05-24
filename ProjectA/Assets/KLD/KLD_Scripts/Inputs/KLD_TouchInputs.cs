using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class KLD_TouchInputs : MonoBehaviour
{
    [System.Serializable]
    class Joystick
    {
        public bool floating = true;
        public int padding = 100;
        public int deadzone = 30;
        public Vector2 defaultOffset;
        public bool offsetFromRightCorner = false;
        public bool draggable = false;
        public enum DragMode { DELTA, LERP };
        [ShowIf("draggable")] public DragMode dragMode = DragMode.LERP;
        [ShowIf("draggable")] public float dragRatio = 0.2f;
        public CanvasGroup canvasGroup;
        public RectTransform firstTouchCircle;
        public RectTransform touchCircle;
        public RectTransform stickCircle;
        public Animator animator;
        [HideInInspector] public Vector2 rawPosition;
        [HideInInspector] public Vector2 rawVector;
        [HideInInspector] public Vector2 rawCappedVector;
        [HideInInspector] public bool drawed = false;
        [ReadOnly] public Vector2 normalizedVector;
        public bool interractable = true;
        public bool endedInterractivity = true;
    }

    [SerializeField] bool useDebugControls = false;

    [SerializeField, ReadOnly] bool useButtonForUltimate = false;
    [SerializeField] GameObject ultiButton = null;
    [SerializeField] GameObject ultiJoystick = null;
    [SerializeField, Range(0.3f, 0.8f)] float leftTouchScreenRatio = 0.4f;

    [SerializeField] long vibrationLength = 100L;

    [SerializeField] Joystick[] joysticks;

    [SerializeField] Vector2Int referenceResolution = new Vector2Int(2260, 1080);
    Vector2 resolutionRatio = Vector2.one;
    [SerializeField] bool overrideScreenSize = false;
    [SerializeField, ShowIf("overrideScreenSize")] Vector2Int overridenScreenSize = Vector2Int.zero;

    Vector2 ratioedPos = Vector2.zero;

    int height;
    int width;

    bool isPressingActiveSkillJoystick = false;

    public bool disableInputs = true;

    public event Action onActiveSkillButton;
    public event Action onReloadButton;
    public event Action<Vector2> onActiveSkillJoystickRelease;
    public event Action<Vector2> onActiveSkillJoystickDown;

    Vector2 lastRawVector;

    void Start()
    {
        if (!overrideScreenSize)
        {
            height = Screen.height;
            width = Screen.width;
        }
        else
        {
            height = overridenScreenSize.y;
            width = overridenScreenSize.x;
        }
        resolutionRatio.x = (float)referenceResolution.x / (float)width;
        resolutionRatio.y = (float)referenceResolution.y / (float)height;

        InitializeJoysticks();
        //InitializeActiveJoystickOrButton();

    }

    void InitializeJoysticks()
    {
        for (int i = 0; i < joysticks.Length; i++)
        {
            //offset.x = (joysticks[i].offsetFromRightCorner ? width : 0) + joysticks[i].defaultOffset.x;
            offset.x = (joysticks[i].offsetFromRightCorner ? referenceResolution.x : 0) + joysticks[i].defaultOffset.x; //AAA_CHANGE
            offset.y = joysticks[i].defaultOffset.y;

            joysticks[i].rawPosition = offset;
            joysticks[i].rawVector = Vector2.zero;
            joysticks[i].firstTouchCircle.anchoredPosition = joysticks[i].rawPosition;
            if (joysticks[i].animator != null && !joysticks[i].floating)
            { joysticks[i].animator.SetTrigger("active"); }
            //joysticks[joyIndex].canvasGroup.alpha = 0.3f;
            joysticks[i].touchCircle.gameObject.SetActive(false);
            joysticks[i].drawed = !joysticks[i].floating;
        }
    }

    public void InitializeActiveJoystickOrButton(bool _isButton)
    {
        useButtonForUltimate = _isButton;
        ultiButton.SetActive(useButtonForUltimate);
        ultiJoystick.SetActive(!useButtonForUltimate);
    }

    void Update()
    {
        UpdateInputs();
        ProcessVectors();
    }

    //local vars
    Touch curTouch;
    bool isLeftTouch;
    int joyIndex;
    Vector2 offset;
    Vector2 curRatioedTouchPosition;

    void UpdateInputs()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                curTouch = Input.GetTouch(i);
                curRatioedTouchPosition.x = curTouch.position.x * resolutionRatio.x;
                curRatioedTouchPosition.y = curTouch.position.y * resolutionRatio.y;
                //print($"curTouch Pos : {curTouch.position} \n ratioedPos : {curRatioedTouchPosition}");

                //isLeftTouch = curRatioedTouchPosition.x < width / 2;
                isLeftTouch = curRatioedTouchPosition.x < referenceResolution.x * leftTouchScreenRatio;

                //joyIndex = isLeftTouch ? 0 : 2;
                if (isLeftTouch) { joyIndex = 0; }
                else if (!isPressingActiveSkillJoystick) { joyIndex = 1; }
                else if (!useButtonForUltimate) { joyIndex = 2; }

                if (joyIndex == 2 && !joysticks[2].interractable)
                {
                    joyIndex = 1;
                }

                if (curTouch.phase == TouchPhase.Began && joysticks[joyIndex].interractable && !disableInputs)
                {
                    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId) || joyIndex == 2)
                    {
                        if (joysticks[joyIndex].floating)
                        {
                            joysticks[joyIndex].rawPosition = curRatioedTouchPosition; //AAA_CHANGE
                                                                                       //joysticks[joyIndex].rawPosition.x = curRatioedTouchPosition.x * resolutionRatio.x;
                                                                                       //joysticks[joyIndex].rawPosition.y = curRatioedTouchPosition.y * resolutionRatio.y;

                            joysticks[joyIndex].drawed = true;
                        }
                        else
                        {
                            CalculateOffset();

                            joysticks[joyIndex].rawPosition = offset;

                            DoRawVectorCalculation();
                        }
                        joysticks[joyIndex].touchCircle.gameObject.SetActive(true);
                        joysticks[joyIndex].touchCircle.anchoredPosition = curRatioedTouchPosition; //AAA_CHANGE
                                                                                                    //ratioedPos.x = curRatioedTouchPosition.x * resolutionRatio.x;
                                                                                                    //ratioedPos.y = curRatioedTouchPosition.y * resolutionRatio.y;
                                                                                                    //joysticks[joyIndex].touchCircle.anchoredPosition = ratioedPos;


                        if (joysticks[joyIndex].animator != null)
                        {
                            joysticks[joyIndex].animator.ResetTrigger("respawn");
                            joysticks[joyIndex].animator.SetTrigger("active");
                        }
                        //joysticks[joyIndex].canvasGroup.alpha = 1f;
                    }
                }
                else if (curTouch.phase == TouchPhase.Moved && joysticks[joyIndex].interractable && !disableInputs)
                {
                    //if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    //{
                    DoRawVectorCalculation();

                    DoDrag();
                    //}

                    if (joyIndex == 2)
                    {
                        CallActiveSkillJoystickDown(joysticks[joyIndex].normalizedVector);
                    }
                }
                else if (curTouch.phase == TouchPhase.Stationary && joysticks[joyIndex].interractable && !disableInputs)
                {
                    DoDrag();

                    if (joyIndex == 2)
                    {
                        CallActiveSkillJoystickDown(joysticks[joyIndex].normalizedVector);
                    }
                }
                else if (curTouch.phase == TouchPhase.Ended ||
                (!joysticks[joyIndex].interractable && !joysticks[joyIndex].endedInterractivity))
                {
                    if (!joysticks[joyIndex].interractable)
                    {
                        joysticks[joyIndex].endedInterractivity = true;
                    }
                    //if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    //{
                    CalculateOffset();

                    joysticks[joyIndex].rawPosition = offset;
                    joysticks[joyIndex].rawVector = Vector2.zero;
                    if (joysticks[joyIndex].animator != null && joysticks[joyIndex].floating)
                    {
                        joysticks[joyIndex].animator.SetTrigger("respawn");
                    }
                    //joysticks[joyIndex].canvasGroup.alpha = 0.3f;
                    joysticks[joyIndex].touchCircle.gameObject.SetActive(false);

                    if (joysticks[joyIndex].floating)
                    {
                        joysticks[joyIndex].drawed = false;
                    }

                    if (joyIndex == 2)
                    {
                        isPressingActiveSkillJoystick = false;
                        //ReleaseActiveSkillJoystick(joysticks[joyIndex].normalizedVector);
                        ReleaseActiveSkillJoystick(joysticks[joyIndex].normalizedVector);
                    }
                    //}
                }

                void CalculateOffset()
                {
                    //AAA_CHANGE
                    //offset.x = (joysticks[joyIndex].offsetFromRightCorner ? width : 0) + joysticks[joyIndex].defaultOffset.x;
                    //offset.y = joysticks[joyIndex].defaultOffset.y;
                    offset.x = (joysticks[joyIndex].offsetFromRightCorner ? referenceResolution.x : 0) + joysticks[joyIndex].defaultOffset.x;
                    offset.y = joysticks[joyIndex].defaultOffset.y;
                }

                void DoRawVectorCalculation()
                {
                    //ratioedPos.x = curRatioedTouchPosition.x * resolutionRatio.x;
                    //ratioedPos.y = curRatioedTouchPosition.y * resolutionRatio.y;
                    lastRawVector = joysticks[joyIndex].rawVector;

                    joysticks[joyIndex].rawVector = curRatioedTouchPosition - joysticks[joyIndex].rawPosition;
                    //joysticks[joyIndex].rawVector = ratioedPos - joysticks[joyIndex].rawPosition;

                    if (joysticks[joyIndex].rawVector.magnitude < joysticks[joyIndex].deadzone)
                    {
                        joysticks[joyIndex].rawVector = Vector2.zero;

                        if (lastRawVector != Vector2.zero)
                        {
                            Vibrate(vibrationLength);
                        }
                    }

                    joysticks[joyIndex].touchCircle.anchoredPosition = curRatioedTouchPosition;
                    //joysticks[joyIndex].touchCircle.anchoredPosition = ratioedPos;
                }

                void DoDrag()
                {
                    if (joysticks[joyIndex].draggable)
                    {
                        switch (joysticks[joyIndex].dragMode)
                        {
                            case Joystick.DragMode.DELTA:
                                joysticks[joyIndex].rawPosition += curTouch.deltaPosition * joysticks[joyIndex].dragRatio;
                                break;

                            case Joystick.DragMode.LERP:
                                Vector2 targetPos = curRatioedTouchPosition - (joysticks[joyIndex].rawCappedVector * 1.1f);
                                joysticks[joyIndex].rawPosition =
                                Vector2.Lerp(joysticks[joyIndex].rawPosition, targetPos, joysticks[joyIndex].dragRatio);
                                break;

                            default:
                                break;
                        }
                    }
                }

                joysticks[joyIndex].firstTouchCircle.anchoredPosition = joysticks[joyIndex].rawPosition;
            }
        }
    }

    void ProcessVectors()
    {
        for (int i = 0; i < joysticks.Length; i++)
        {
            if (i == 2 && useButtonForUltimate) break;

            joysticks[i].rawCappedVector =
            joysticks[i].rawVector.sqrMagnitude > joysticks[i].padding * joysticks[i].padding ?
            joysticks[i].rawVector.normalized * joysticks[i].padding :
            joysticks[i].rawVector;

            //if (joysticks[i].rawCappedVector.magnitude < joysticks[i].deadzone)
            //{
            //    joysticks[i].rawCappedVector = Vector2.zero;
            //}

            joysticks[i].stickCircle.anchoredPosition = joysticks[i].rawPosition + joysticks[i].rawCappedVector;

            joysticks[i].normalizedVector = joysticks[i].rawCappedVector / joysticks[i].padding;
        }
    }

    public Vector2 GetJoystickNormalizedVector(int _joystickID)
    {
        if (!useDebugControls)
        {
            return joysticks[_joystickID].normalizedVector;
        }
        else
        {
            if (_joystickID == 0)
            {
                return GetDebugMoveVector();
            }
            else if (_joystickID == 1)
            {
                return GetDebugAimVector();
            }
            else
            {
                return Vector2.zero;
            }
        }
    }

    public bool IsJoystickPressed(int _joystickID)
    {
        if (!useDebugControls)
        {
            if (joysticks[_joystickID].floating)
            {
                return joysticks[_joystickID].drawed;
            }
            else
            {
                return joysticks[_joystickID].normalizedVector.sqrMagnitude > 0.05f * 0.05f;
            }
        }
        else
        {
            if (_joystickID == 1)
            {
                return Input.GetKey(KeyCode.Space);
            }
            else
            {
                return true;
            }
        }
    }

    public void PressActiveSkillButton()
    {
        onActiveSkillButton?.Invoke();
    }

    public void PressReloadButton()
    {
        onReloadButton?.Invoke();
    }

    public void DetectActiveSkillJoystick()
    {
        isPressingActiveSkillJoystick = true;
    }

    void CallActiveSkillJoystickDown(Vector2 _input)
    {
        onActiveSkillJoystickDown?.Invoke(_input);
    }

    void ReleaseActiveSkillJoystick(Vector2 _input)
    {
        onActiveSkillJoystickRelease?.Invoke(_input);
    }

    public bool GetUseButtonForUltimate()
    {
        return useButtonForUltimate;
    }

    public void SetJoystickInterractable(int _joyIndex, bool _interractable)
    {
        joysticks[_joyIndex].interractable = _interractable;
        if (!_interractable)
        {
            joysticks[_joyIndex].endedInterractivity = false;
        }
    }

    void Vibrate(long _time)
    {
        //Vibration.Vibrate(_time);
        //Handheld.Vibrate();
        //StartCoroutine(VibrateCoroutine(_time));
    }

    /*
    float t;
    IEnumerator VibrateCoroutine(float _time)
    {
        t = 0f;
        while (t < _time)
        {
            Handheld.Vibrate();
            yield return null;
            t += Time.deltaTime;
        }
    }
    */

    #region Debug

    Vector2 debugMoveVector = Vector2.zero;

    Vector2 GetDebugMoveVector()
    {
        debugMoveVector.x = Input.GetAxisRaw("Horizontal");
        debugMoveVector.y = Input.GetAxisRaw("Vertical");

        if (debugMoveVector.sqrMagnitude > 1f)
        {
            debugMoveVector.Normalize();
        }

        return debugMoveVector;
    }

    Vector2 debugAimVector = Vector2.zero;

    Vector2 GetDebugAimVector()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            debugAimVector.x = Input.GetAxisRaw("AimHorizontal");
            debugAimVector.y = Input.GetAxisRaw("AimVertical");

            if (debugAimVector.sqrMagnitude > 1f)
            {
                debugAimVector.Normalize();
            }

            return debugAimVector;
        }
        else
        {
            return Vector2.zero;
        }
    }

    #endregion

}