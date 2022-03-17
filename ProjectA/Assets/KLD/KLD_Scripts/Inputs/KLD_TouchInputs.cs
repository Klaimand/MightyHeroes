using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class KLD_TouchInputs : MonoBehaviour
{
    [SerializeField] bool overrideScreenSize = false;
    [SerializeField, ShowIf("overrideScreenSize")] Vector2Int overridenScreenSize = Vector2Int.zero;

    int height;
    int width;

    [Header("UI REFERENCES")]
    [SerializeField] RectTransform L_firstTouchCircle;
    [SerializeField] RectTransform R_firstTouchCircle;
    [SerializeField] RectTransform L_touchCircle;
    [SerializeField] RectTransform R_touchCircle;
    [SerializeField] RectTransform L_stickCircle;
    [SerializeField] RectTransform R_stickCircle;


    Vector2 leftTouch_rawPosition = Vector2.zero;
    Vector2 rightTouch_rawPosition = Vector2.zero;

    Vector2 leftTouchRawVector = Vector2.zero;
    Vector2 rightTouchRawVector = Vector2.zero;

    Vector2 leftTouchRawCappedVector = Vector2.zero;
    Vector2 rightTouchRawCappedVector = Vector2.zero;

    [SerializeField, ReadOnly] Vector2 leftTouchNormalizedVector = Vector2.zero;
    [SerializeField, ReadOnly] Vector2 rightTouchNormalizedVector = Vector2.zero;


    [Header("Joysticks")]
    [SerializeField] int leftStickPadding = 200;
    [SerializeField] int rightStickPadding = 200;

    [SerializeField] float rightStickMaxTimeToShoot = 0.2f;

    bool isRightStickPressed = false;
    float curRightStickPressTime = 0f;

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();
        ProcessVectors();
    }

    void UpdateInputs()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch curTouch = Input.GetTouch(i);

                bool isLeftTouch = curTouch.position.x < width / 2;

                if (curTouch.phase == TouchPhase.Began)
                {
                    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    {
                        if (isLeftTouch)
                        {
                            leftTouch_rawPosition = curTouch.position;
                            L_firstTouchCircle.anchoredPosition = leftTouch_rawPosition;
                            L_touchCircle.anchoredPosition = curTouch.position;
                        }
                        else
                        {
                            isRightStickPressed = true;
                            rightTouch_rawPosition = curTouch.position;
                            R_firstTouchCircle.anchoredPosition = rightTouch_rawPosition;
                            R_touchCircle.anchoredPosition = curTouch.position;
                        }
                    }
                }
                else if (curTouch.phase == TouchPhase.Moved)
                {
                    if (isLeftTouch)
                    {
                        leftTouchRawVector = curTouch.position - leftTouch_rawPosition;
                        L_touchCircle.anchoredPosition = curTouch.position;
                    }
                    else
                    {
                        rightTouchRawVector = curTouch.position - rightTouch_rawPosition;
                        R_touchCircle.anchoredPosition = curTouch.position;
                    }
                }
                else if (curTouch.phase == TouchPhase.Ended)
                {
                    if (!isLeftTouch)
                    {
                        isRightStickPressed = false;
                        print("aa");
                    }
                }
            }
        }
    }

    void ProcessVectors()
    {
        //left
        leftTouchRawCappedVector = leftTouchRawVector.sqrMagnitude > leftStickPadding * leftStickPadding ?
        leftTouchRawVector.normalized * leftStickPadding :
        leftTouchRawVector;

        L_stickCircle.anchoredPosition = leftTouch_rawPosition + leftTouchRawCappedVector;

        leftTouchNormalizedVector = leftTouchRawCappedVector / leftStickPadding;


        //right
        rightTouchRawCappedVector = rightTouchRawVector.sqrMagnitude > rightStickPadding * rightStickPadding ?
        rightTouchRawVector.normalized * rightStickPadding :
        rightTouchRawVector;

        R_stickCircle.anchoredPosition = rightTouch_rawPosition + rightTouchRawCappedVector;

        rightTouchNormalizedVector = rightTouchRawCappedVector / rightStickPadding;
    }






    /*
        void ObsUpdateInputs()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x < width / 2)
                    {
                        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                        {
                            //firstTouchPos = touch.position;

                            L_firstTouchCircle.anchoredPosition = touch.rawPosition;
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    leftTouchRawVector = touch.position - touch.rawPosition;

                    L_touchCircle.anchoredPosition = touch.position;
                }

                //print(leftTouchRawVector);
            }
        }
    */
}