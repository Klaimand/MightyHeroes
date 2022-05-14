using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_VFX : MonoBehaviour
{
    [SerializeField] GameObject ult;
    void Start()
    {
        ult.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ult.SetActive(true);
            print("ulting");
            StartCoroutine(ExampleCoroutine());
        }
    }
    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(2);
        ult.SetActive(false);
    }
}
