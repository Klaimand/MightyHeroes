using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSL_PourJeanne : MonoBehaviour
{
    [SerializeField] Animator animator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetFloat("Blend", 1f);
        }
        if (Input.GetKey(KeyCode.E))
        {
            animator.SetFloat("Blend", 0f);
        }
    }
}
