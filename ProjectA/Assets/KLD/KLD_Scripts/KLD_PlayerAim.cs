using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_PlayerAim : MonoBehaviour
{

    [SerializeField] Transform target = null;

    Vector3 targetPos = Vector3.zero;

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
    }
}
