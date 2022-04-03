using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSL_ControlerAnimTest : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int locomotionState;
    [SerializeField] int weaponState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("locomotionState", locomotionState);
        animator.SetInteger("weaponState", weaponState);

        if (Input.GetKey(KeyCode.Alpha1)) { locomotionState = 1; } else { locomotionState = 0; }

        if (Input.GetKey(KeyCode.Alpha3)) { weaponState = 0; }
        if (Input.GetKey(KeyCode.Alpha2)) { weaponState = 1; }
        if (Input.GetKeyDown(KeyCode.R)) { weaponState = 3; }
        if (Input.GetMouseButton(0)) { weaponState = 2; } 
    }
}
