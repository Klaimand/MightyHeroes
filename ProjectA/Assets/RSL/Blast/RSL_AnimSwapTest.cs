using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSL_AnimSwapTest : MonoBehaviour
{
    Animator animator;
    bool running;
    bool holding;
    bool aiming;
    bool shooting;
    bool reload;
    bool unarmed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        running = false;
        holding = true;
        aiming = false;
        shooting = false;
        reload = false;
        unarmed = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
            running = true;

        else running = false;

        if (running == false)
            animator.SetBool("Running", false);
        if (running == true)
            animator.SetBool("Running", true);


        if (Input.GetKey(KeyCode.Alpha1))
            aiming = true;

        else aiming = false;
        if (aiming == false)
            animator.SetBool("Aiming", false);
        if (aiming == true)
            animator.SetBool("Aiming", true);


        if (Input.GetKey(KeyCode.Alpha2))
            shooting = true;

        else shooting = false;
        if (shooting == false)
            animator.SetBool("Shooting", false);
        if (shooting == true)
            animator.SetBool("Shooting", true);

        if (Input.GetKeyDown(KeyCode.R))
            animator.SetTrigger("Reloading");
        if (Input.GetKeyDown(KeyCode.E))
            animator.SetTrigger("Grenade");
    }
}
