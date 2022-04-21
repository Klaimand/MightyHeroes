using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_Depoper : MonoBehaviour
{
    [SerializeField] float depopAfter = 2f;
    [SerializeField] string poolKey = "testWeapon_muzzle";

    void OnEnable()
    {
        StartCoroutine(DepopIn());
    }

    IEnumerator DepopIn()
    {
        yield return new WaitForSeconds(depopAfter);
        if (gameObject.activeSelf)
        {
            XL_Pooler.instance.DePop(poolKey, gameObject);
        }
    }
}
