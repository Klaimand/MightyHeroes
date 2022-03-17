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
        public int padding = 100;
        public CanvasGroup canvasGroup;
        public RectTransform firstTouchCircle;
        public RectTransform touchCircle;
        public RectTransform stickCircle;
        [HideInInspector] public Vector2 rawPosition;
        [HideInInspector] public Vector2 rawVector;
        [HideInInspector] public Vector2 rawCappedVector;
        [ReadOnly] public Vector2 normalizedVector;
    }

    [SerializeField] Joystick[] joysticks;

    [SerializeField] bool overrideScreenSize = false;
    [SerializeField, ShowIf("overrideScreenSize")] Vector2Int overridenScreenSize = Vector2Int.zero;

    int height;
    int width;

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

    void UpdateInputs()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                curTouch = Input.GetTouch(i);

                isLeftTouch = curTouch.position.x < width / 2;

                joyIndex = isLeftTouch ? 0 : 1;

                if (curTouch.phase == TouchPhase.Began)
                {
                    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    {
                        joysticks[joyIndex].rawPosition = curTouch.position;
                        joysticks[joyIndex].firstTouchCircle.anchoredPosition = joysticks[joyIndex].rawPosition;
                        joysticks[joyIndex].touchCircle.anchoredPosition = curTouch.position;

                        joysticks[joyIndex].canvasGroup.alpha = 1f;
                    }
                }
                else if (curTouch.phase == TouchPhase.Moved)
                {
                    joysticks[joyIndex].rawVector = curTouch.position - joysticks[joyIndex].rawPosition;
                    joysticks[joyIndex].touchCircle.anchoredPosition = curTouch.position;
                }
                else if (curTouch.phase == TouchPhase.Ended)
                {
                    joysticks[joyIndex].rawPosition = Vector2.zero;
                    joysticks[joyIndex].rawVector = Vector2.zero;
                    joysticks[joyIndex].canvasGroup.alpha = 0f;
                }
            }
        }
    }

    void ProcessVectors()
    {
        for (int i = 0; i < joysticks.Length; i++)
        {
            joysticks[i].rawCappedVector =
            joysticks[i].rawVector.sqrMagnitude > joysticks[i].padding * joysticks[i].padding ?
            joysticks[i].rawVector.normalized * joysticks[i].padding :
            joysticks[i].rawVector;

            joysticks[i].stickCircle.anchoredPosition = joysticks[i].rawPosition + joysticks[i].rawCappedVector;

            joysticks[i].normalizedVector = joysticks[i].rawCappedVector / joysticks[i].padding;
        }
    }

}