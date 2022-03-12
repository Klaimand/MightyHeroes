using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class KLD_TouchInputs : MonoBehaviour
{
    Vector2 leftTouchRawVector;
    Vector2 rightTouchRawVector;


    [SerializeField] bool overrideScreenSize = false;
    [SerializeField, ShowIf("overrideScreenSize")] Vector2Int overridenScreenSize = Vector2Int.zero;

    int height;
    int width;

    [Header("UI REFERENCES")]
    [SerializeField] RectTransform L_firstTouchCircle;
    [SerializeField] RectTransform L_touchCircle;
    [SerializeField] RectTransform R_firstTouchCircle;
    [SerializeField] RectTransform R_touchCircle;

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
        ObsUpdateInputs();
    }

    void UpdateInputs()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch curTouch = Input.GetTouch(i);

                bool isLeftTouch = curTouch.rawPosition.x < width / 2;
                print(curTouch.rawPosition);

                if (curTouch.phase == TouchPhase.Began)
                {
                    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        if (isLeftTouch)
                        {
                            L_firstTouchCircle.anchoredPosition = curTouch.rawPosition;
                        }
                        else
                        {
                            R_firstTouchCircle.anchoredPosition = curTouch.rawPosition;
                        }
                    }
                }
                else if (curTouch.phase == TouchPhase.Moved)
                {
                    if (isLeftTouch)
                    {
                        leftTouchRawVector = curTouch.position - curTouch.rawPosition;
                        L_touchCircle.anchoredPosition = curTouch.position;
                    }
                    else
                    {
                        rightTouchRawVector = curTouch.position - curTouch.rawPosition;
                        R_touchCircle.anchoredPosition = curTouch.position;
                    }
                }
            }
        }
    }

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
}
