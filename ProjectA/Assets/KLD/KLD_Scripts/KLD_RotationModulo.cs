using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_RotationModulo : MonoBehaviour
{
    [SerializeField] Transform player;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, player.eulerAngles.y % 180f, 0f);
    }
}
