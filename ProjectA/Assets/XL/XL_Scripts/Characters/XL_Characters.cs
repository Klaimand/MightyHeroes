using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Characters : MonoBehaviour, XL_IDamageable
{
    [SerializeField] protected XL_CharacterAttributes characterAttributes;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected KLD_PlayerAim playerAim;
    [SerializeField] protected LayerMask layer;

    [SerializeField] protected float fireRate;
    private bool canFire;

    private void Start()
    {
        canFire = true;
    }

    private void Update()
    {
        //Debug.Log("targetPos : " + playerAim.GetTargetPos());
    }

    public void Move(Vector3 direction)
    {
        rb.velocity = direction * characterAttributes.movementSpeed;
    }

    private RaycastHit hit;
    private XL_IDamageable target;
    public void Shoot() 
    {
        if (canFire) 
        {
            if (Physics.Raycast(transform.position, playerAim.GetTargetPos() - transform.position, out hit, 50/*, layer*/))
            {
                StartCoroutine(FireRateCooldown(fireRate));
                if ((target = hit.transform.GetComponent<XL_IDamageable>()) != null) target.TakeDamage(10);
            }
        }
    }

    IEnumerator FireRateCooldown(float t) 
    {
        canFire = false;
        yield return new WaitForSeconds(t);
        canFire = true;
    }

    public void Die()
    {
        StopAllCoroutines();
        XL_GameManager.instance.removePlayer(transform.gameObject);
        transform.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        characterAttributes.health -= damage;
        if (characterAttributes.health < 1)
        {
            Die();
        }
    }
}
