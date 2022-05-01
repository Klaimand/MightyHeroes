using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSL_CapingAnimSpeedControler : MonoBehaviour
{
    [SerializeField] Animator animator;
    float animSpeedVar = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            animSpeedVar = animSpeedVar * -1f;
            animator.SetFloat("animSpeed", animSpeedVar);
        }
    }
}
