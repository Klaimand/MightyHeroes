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
        WeaponAnimation();

        CharacterAnimation();

    }

    void WeaponAnimation()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            animator.SetBool("Unarmed", false);
            aiming = true;
            animator.SetBool("Aiming", true);

        }
        else
        {
            aiming = false;
            animator.SetBool("Aiming", false);
        }

        if (Input.GetButton("Fire1") && aiming == true) 
        {

            shooting = true;
            animator.SetBool("Shooting", true);
        }
        else
        {
            shooting = false;
            animator.SetBool("Shooting", false);
        }

        if (Input.GetButton("Fire2"))
        {
            animator.SetBool("Unarmed", false);
            animator.SetTrigger("Reloading");
            aiming = false;
            shooting = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Blast_GrenadeThrow"))
        {
            animator.SetBool("Unarmed", true);
        }
        else animator.SetBool("Unarmed", false);
    }
    void CharacterAnimation()
    {
        if (Input.GetKey(KeyCode.Z))
            running = true;
        else running = false;
        if (running == false)
            animator.SetBool("Running", false);
        if (running == true)
            animator.SetBool("Running", true);


        if (Input.GetKeyDown(KeyCode.E))
        {
            //animator.SetBool("Unarmed", true);
            animator.SetTrigger("Grenade");
        }
        //else animator.SetBool("Unarmed", false);
    }

}
