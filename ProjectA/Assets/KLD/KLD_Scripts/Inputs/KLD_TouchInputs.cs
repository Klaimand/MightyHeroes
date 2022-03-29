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
        [ReadOnly] public Vector2 normalizedVector;
    }

    [SerializeField] bool useButtonForUltimate = false;
    [SerializeField] GameObject ultiButton = null;
    [SerializeField] GameObject ultiJoystick = null;

    [SerializeField] Joystick[] joysticks;

    [SerializeField] bool overrideScreenSize = false;
    [SerializeField, ShowIf("overrideScreenSize")] Vector2Int overridenScreenSize = Vector2Int.zero;

    int height;
    int width;

    bool isPressingActiveSkillJoystick = false;

    public event Action onActiveSkillButton;
    public event Action<Vector2> onActiveSkillJoystickRelease;

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

        InitializeJoysticks();
        InitializeActiveJoystickOrButton();
    }

    void InitializeJoysticks()
    {
        for (int i = 0; i < joysticks.Length; i++)
        {
            offset.x = (joysticks[i].offsetFromRightCorner ? width : 0) + joysticks[i].defaultOffset.x;
            offset.y = joysticks[i].defaultOffset.y;

            joysticks[i].rawPosition = offset;
            joysticks[i].rawVector = Vector2.zero;
            joysticks[i].firstTouchCircle.anchoredPosition = joysticks[i].rawPosition;
            if (joysticks[i].animator != null && !joysticks[i].floating)
            { joysticks[i].animator.SetTrigger("active"); }
            //joysticks[joyIndex].canvasGroup.alpha = 0.3f;
            joysticks[i].touchCircle.gameObject.SetActive(false);
        }
    }

    void InitializeActiveJoystickOrButton()
    {
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

    void UpdateInputs()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                curTouch = Input.GetTouch(i);

                isLeftTouch = curTouch.position.x < width / 2;

                //joyIndex = isLeftTouch ? 0 : 2;
                if (isLeftTouch) { joyIndex = 0; }
                else if (!isPressingActiveSkillJoystick) { joyIndex = 1; }
                else if (!useButtonForUltimate) { joyIndex = 2; }

                if (curTouch.phase == TouchPhase.Began)
                {
                    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId) || joyIndex == 2)
                    {
                        if (joysticks[joyIndex].floating)
                        {
                            joysticks[joyIndex].rawPosition = curTouch.position;
                        }
                        else
                        {
                            CalculateOffset();

                            joysticks[joyIndex].rawPosition = offset;

                            DoRawVectorCalculation();
                        }
                        joysticks[joyIndex].touchCircle.gameObject.SetActive(true);
                        joysticks[joyIndex].touchCircle.anchoredPosition = curTouch.position;

                        if (joysticks[joyIndex].animator != null) joysticks[joyIndex].animator.SetTrigger("active");
                        //joysticks[joyIndex].canvasGroup.alpha = 1f;
                    }
                }
                else if (curTouch.phase == TouchPhase.Moved)
                {
                    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    {
                        DoRawVectorCalculation();

                        DoDrag();
                    }
                }
                else if (curTouch.phase == TouchPhase.Stationary)
                {
                    DoDrag();
                }
                else if (curTouch.phase == TouchPhase.Ended)
                {
                    CalculateOffset();

                    joysticks[joyIndex].rawPosition = offset;
                    joysticks[joyIndex].rawVector = Vector2.zero;
                    if (joysticks[joyIndex].animator != null && joysticks[joyIndex].floating)
                    { joysticks[joyIndex].animator.SetTrigger("respawn"); }
                    //joysticks[joyIndex].canvasGroup.alpha = 0.3f;
                    joysticks[joyIndex].touchCircle.gameObject.SetActive(false);

                    if (joyIndex == 2)
                    {
                        isPressingActiveSkillJoystick = false;
                        ReleaseActiveSkillJoystick(joysticks[joyIndex].normalizedVector);
                    }
                }

                void CalculateOffset()
                {
                    offset.x = (joysticks[joyIndex].offsetFromRightCorner ? width : 0) + joysticks[joyIndex].defaultOffset.x;
                    offset.y = joysticks[joyIndex].defaultOffset.y;
                }

                void DoRawVectorCalculation()
                {
                    joysticks[joyIndex].rawVector = curTouch.position - joysticks[joyIndex].rawPosition;

                    if (joysticks[joyIndex].rawVector.magnitude < joysticks[joyIndex].deadzone)
                    {
                        joysticks[joyIndex].rawVector = Vector2.zero;
                    }

                    joysticks[joyIndex].touchCircle.anchoredPosition = curTouch.position;
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
                                Vector2 targetPos = curTouch.position - (joysticks[joyIndex].rawCappedVector * 1.1f);
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
        return joysticks[_joystickID].normalizedVector;
    }

    public void PressActiveSkillButton()
    {
        onActiveSkillButton?.Invoke();
    }

    public void DetectActiveSkillJoystick()
    {
        isPressingActiveSkillJoystick = true;
    }

    void ReleaseActiveSkillJoystick(Vector2 _input)
    {
        onActiveSkillJoystickRelease?.Invoke(_input);
    }



}