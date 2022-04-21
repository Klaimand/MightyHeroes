using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSL_VFXosefscript : MonoBehaviour
{
    [SerializeField] GameObject vfxPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            vfxPrefab.SetActive(true);
        }
        else vfxPrefab.SetActive(false);
    }
}
