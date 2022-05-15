using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Projectile : MonoBehaviour
{

    [SerializeField] private XL_ProjectileSO projectileSO;
    private Vector3 startPosition;
    [SerializeField] private float refreshCheck;
    private bool destroyed;

    public void Initialize()
    {
        destroyed = false;
        startPosition = transform.position;
        StartCoroutine(CheckRangeCoroutine(refreshCheck));
    }

    public void Initialize(Vector3 startPosition)
    {
        this.startPosition = startPosition;
    }

    IEnumerator CheckRangeCoroutine(float t)
    {
        
        yield return new WaitForSeconds(t);
        if ((transform.position - startPosition).magnitude > projectileSO.range)
        {
            Debug.Log("Depoped because of range");
            XL_Pooler.instance.DePop(projectileSO.projectileName, transform.gameObject);
            StopAllCoroutines();
        }
        else if(!destroyed)
        {
            StartCoroutine(CheckRangeCoroutine(t));
        }
        
    }

    XL_IDamageable objectHit;
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Enemy")) 
        {
            destroyed = true;
            objectHit = collision.transform.GetComponent<XL_IDamageable>();
            if (objectHit != null)
            {
                objectHit.TakeDamage(projectileSO.damage);
            }
            Debug.Log("Depoped because of collision");
            StopAllCoroutines();
            XL_Pooler.instance.DePop(projectileSO.projectileName, transform.gameObject);
        }
        
    }
}
