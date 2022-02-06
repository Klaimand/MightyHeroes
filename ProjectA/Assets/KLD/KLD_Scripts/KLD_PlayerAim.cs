using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_PlayerAim : MonoBehaviour
{

    [SerializeField] Transform target = null;

    Vector3 targetPos = Vector3.zero;

    Vector3 debugPosa = Vector3.zero;
    Vector3 debugPosb = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        targetPos = target.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos, Vector3.up);

        DrawSelectedLine();
    }

    void DrawSelectedLine()
    {
        debugPosa = transform.position;
        debugPosb = targetPos;
        debugPosa.y = 1f;
        debugPosb.y = 1f;
        Debug.DrawLine(debugPosa, debugPosb, Color.red);
    }
}
