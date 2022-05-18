using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class KLD_RotateMenuCharacter : MonoBehaviour
{
    [SerializeField] Vector2Int referenceResolution = new Vector2Int(2260, 1080);
    Vector2 resolutionRatio = Vector2.one;
    [SerializeField] bool overrideScreenSize = false;
    [SerializeField, ShowIf("overrideScreenSize")] Vector2Int overridenScreenSize = Vector2Int.zero;

    int height;
    int width;

    [Space(20)]
    [SerializeField] Vector2 horizontalBounds;
    [SerializeField] Vector2 verticalBounds;
    Vector2Int rectMin;
    Vector2Int rectMax;
    RectInt pixelsRect;

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
        resolutionRatio.x = (float)referenceResolution.x / (float)width;
        resolutionRatio.y = (float)referenceResolution.y / (float)height;


        rectMin.x = Mathf.RoundToInt(width * horizontalBounds.x);
        rectMin.y = Mathf.RoundToInt(height * verticalBounds.x);

        rectMax.x = Mathf.RoundToInt(width * horizontalBounds.y);
        rectMax.y = Mathf.RoundToInt(height * verticalBounds.y);

        pixelsRect.SetMinMax(rectMin, rectMax);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTouches();
        ProcessVel();
        curAngle += curVel;
        //curAngle = curAngle % 360f;
        objToRotate.rotation = Quaternion.Euler(0f, curAngle + 180f, 0f);
    }

    void OnValidate()
    {
        horizontalBounds.x = Mathf.Clamp01(horizontalBounds.x);
        horizontalBounds.y = Mathf.Clamp01(horizontalBounds.y);

        verticalBounds.x = Mathf.Clamp01(verticalBounds.x);
        verticalBounds.y = Mathf.Clamp01(verticalBounds.y);
    }

    Touch curTouch;
    Vector2 curRatioedTouchPosition;
    Vector2Int curIntedTouchPosition;
    bool inZone = false;

    [SerializeField] Transform objToRotate;
    [SerializeField] float sensitivity = 2f;
    float curAngle = 0f;
    float curVel = 0f;
    [SerializeField] float drag = 0.1f;
    [SerializeField] float stopVel = 3f;
    [SerializeField] float maxVel = 720f;

    void UpdateTouches()
    {
        if (Input.touchCount > 0)
        {
            curTouch = Input.GetTouch(0);
            curRatioedTouchPosition.x = curTouch.position.x * resolutionRatio.x;
            curRatioedTouchPosition.y = curTouch.position.y * resolutionRatio.y;

            curIntedTouchPosition.x = Mathf.RoundToInt(curRatioedTouchPosition.x);
            curIntedTouchPosition.y = Mathf.RoundToInt(curRatioedTouchPosition.y);

            if (curTouch.phase == TouchPhase.Began)
            {
                inZone = pixelsRect.Contains(curIntedTouchPosition);
                if (inZone)
                {
                    curVel = 0f;
                }
            }
            else if (curTouch.phase == TouchPhase.Stationary)
            {
                if (inZone)
                {
                    curVel = 0f;
                }
            }
            else if (curTouch.phase == TouchPhase.Moved)
            {
                if (inZone)
                {
                    curVel = -curTouch.deltaPosition.x * sensitivity * 0.1f;
                }
            }
            else if (curTouch.phase == TouchPhase.Ended)
            {
                inZone = false;
            }
        }
    }

    void ProcessVel()
    {
        curVel = curVel * (1f - (drag * Time.deltaTime));
        if (Mathf.Abs(curVel) < stopVel)
        {
            curVel = 0f;
        }
        if (Mathf.Abs(curVel) > maxVel)
        {
            curVel = maxVel * Mathf.Sign(curVel);
        }
    }

    public void ResetVelAndAngle()
    {
        curVel = 0f;
        curAngle = 0f;
    }
}