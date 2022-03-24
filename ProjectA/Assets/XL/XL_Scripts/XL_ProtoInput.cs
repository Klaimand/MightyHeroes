using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_ProtoInput : MonoBehaviour
{
    [SerializeField] private XL_Characters character;
    [SerializeField] private Transform camera;

    private float inputRight;
    private float inputForward;

    // Update is called once per frame
    void Update()
    {
        inputRight = Input.GetAxisRaw("Horizontal");
        inputForward = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Fire1")) 
        {
            character.Shoot();
        }
        if (Input.GetButton("Fire2")) 
        {
            character.ActivateSpell(new Vector3(inputRight, 0, inputForward));
        }

        character.Move((camera.right * inputRight + camera.forward * inputForward).normalized);
    }
}
