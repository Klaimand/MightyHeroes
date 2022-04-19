using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_ProtoInput : MonoBehaviour
{
    [SerializeField] private XL_Characters character;

    private float inputRight;
    private float inputForward;

    // Update is called once per frame
    void Update()
    {

        inputRight = Input.GetAxisRaw("Horizontal");
        inputForward = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.E)) 
        {
            character.ActivateSpell(new Vector3(inputRight, 0, inputForward));
        }
    }
}
