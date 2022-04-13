using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRotateScaleTest : MonoBehaviour
{
    [SerializeField] private Transform transform;
    private float xOffset;
    private float yOffset;
    private float zOffset;

    private void Start()
    {
        xOffset = transform.position.x;
        yOffset = transform.position.y;
        zOffset = transform.position.z;
    }

    private void Update()
    {
        transform.position = new Vector3(Mathf.PingPong(Time.time, 10) + xOffset, yOffset, zOffset + Mathf.PingPong(Time.time, 10));
        transform.Rotate(new Vector3(2, 2, 2));
        transform.localScale = new Vector3(Mathf.PingPong(Time.time, 10)+2, Mathf.PingPong(Time.time, 10)+2, Mathf.PingPong(Time.time, 10)+2);
        //transform.position = new Vector3(sin(Time.time) + xOffset, yOffset, zOffset + Mathf.PingPong(3, 10));
    }
}
