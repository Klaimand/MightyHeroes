using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSL_Screenshackissou : MonoBehaviour
{

    [SerializeField] Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) { anim.enabled = true; } else anim.enabled = false;
    }
}
