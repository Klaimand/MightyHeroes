using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KLD_ObjectivePointer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] RectTransform pointer;
    [SerializeField] RectTransform pointerImage;
    [SerializeField] Image image;
    Transform nearestObjective;
    Vector3 playerToNearestObjective;
    Vector3 rectTransformEulerAngles = Vector3.zero;

    [SerializeField] Vector2 minMaxPointerDistanceFromPlayer = new Vector2(90f, 150f);

    [SerializeField] Vector2 minMaxDistanceFromObjective = new Vector2(1f, 10f);

    [SerializeField] Vector2 minMaxAlpha = new Vector2(0f, 1f);
    [SerializeField] AnimationCurve alphaCurve;

    float angle = 0f;

    float t;
    Vector3 imageLocalPos;
    Color color = Color.white;
    float a;
    // Update is called once per frame
    void Update()
    {
        nearestObjective = XL_GameModeManager.instance.GetNearestObjective(player.position);
        playerToNearestObjective = nearestObjective.position - player.position;
        angle = Vector3.SignedAngle(player.forward, playerToNearestObjective, -Vector3.up);

        angle = angle - player.rotation.eulerAngles.y;// + 180f;

        rectTransformEulerAngles.z = angle;
        pointer.localEulerAngles = rectTransformEulerAngles;

        t = Mathf.InverseLerp(minMaxDistanceFromObjective.x, minMaxDistanceFromObjective.y, playerToNearestObjective.magnitude);

        imageLocalPos = pointerImage.localPosition;
        imageLocalPos.y = Mathf.Lerp(minMaxPointerDistanceFromPlayer.x, minMaxPointerDistanceFromPlayer.y, t);
        pointerImage.localPosition = imageLocalPos;

        color.a = Mathf.Lerp(minMaxAlpha.x, minMaxAlpha.y, alphaCurve.Evaluate(t));

        image.color = color;
    }
}
