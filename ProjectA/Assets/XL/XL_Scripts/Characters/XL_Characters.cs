using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Characters : MonoBehaviour, XL_IDamageable
{
    [SerializeField] protected XL_CharacterAttributes characterAttributes;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected KLD_PlayerAim playerAim;
    [SerializeField] protected Transform shootDirection;

    [SerializeField] protected float fireRate;
    private bool canFire;

    [SerializeField] private XL_ISpells spell;
    public float ultimateCharge;
    public bool isUltimateCharged;
    [SerializeField] private float ultimateChargeTick;

    //[SerializeField] private XL_HealthBarUI characterUI;

    private void Awake()
    {
        characterAttributes.Initialize();
    }
    private void Start()
    {
        canFire = true;
        ultimateCharge = 100;
        spell = transform.GetComponent<XL_ISpells>();
        StartCoroutine(OutOfCombatHealingCoroutine(1f));
    }

    private void Update()
    {
        if (playerAim.isShooting) 
        {
            Shoot();
        }
    }

    private RaycastHit hit;
    private XL_IDamageable target;
    public void Shoot() 
    {
        if (canFire) 
        {
            Debug.Log("Shoot");
            if (Physics.Raycast(transform.position + Vector3.up * 1f, playerAim.GetSelectedZombie().transform.position - transform.position, out hit, 50/*, layer*/))
            {
                StartCoroutine(FireRateCooldown(fireRate));
                if ((target = hit.transform.GetComponent<XL_IDamageable>()) != null) target.TakeDamage(10);
            }
            Debug.DrawRay(transform.position, playerAim.GetSelectedZombie().transform.position - transform.position, Color.black);

            StopPassiveHeal();
            CancelInvoke("RestorePassiveHeal");
            Invoke("RestorePassiveHeal", restorePassiveHealDuration);
        }
    }

    IEnumerator FireRateCooldown(float t) 
    {
        canFire = false;
        yield return new WaitForSeconds(t);
        canFire = true;
    }

    public void ActivateSpell(Vector3 direction)
    {
        if (ultimateCharge == 100) 
        {
            ultimateCharge = 0;
            isUltimateCharged = false;
            StartCoroutine(SpellCooldownCoroutine(ultimateChargeTick));
            spell.ActivateSpell(direction);

            StopPassiveHeal();
            CancelInvoke("RestorePassiveHeal");
            Invoke("RestorePassiveHeal", restorePassiveHealDuration);
        }
    }

    IEnumerator SpellCooldownCoroutine(float t) 
    {

        yield return new WaitForSeconds(t);

        ultimateCharge += characterAttributes.activeTick;
        //characterUI.UpdateUltBar(ultimateCharge * 0.01f);
        if (ultimateCharge < 100)
        {
            StartCoroutine(SpellCooldownCoroutine(t));
        }
        else isUltimateCharged = true;
    }


    public void Die()
    {
        StopAllCoroutines();
        XL_GameManager.instance.RemovePlayer(transform.gameObject);
        transform.gameObject.SetActive(false);
    }

    [ContextMenu("Take 1 Damage")]
    public void Take100Damage() { TakeDamage(100); }

    public void TakeDamage(int damage)
    {
        if (damage > 0) // if it takes damage, then reduce the damage taken
        {
            if ((damage - characterAttributes.armor) < 1) damage = 1; //the character will always take 1 damage;
        }

        characterAttributes.health -= damage;

        if (characterAttributes.health < 1)
        {
            Die();
        }

        if (characterAttributes.health > characterAttributes.healthMax) 
        {
            characterAttributes.health = characterAttributes.healthMax;
        }
        //characterUI.UpdateHealthBar(characterAttributes.health / characterAttributes.healthMax);
        Debug.Log(gameObject.name + " : " + characterAttributes.health);

        StopPassiveHeal();
        CancelInvoke("RestorePassiveHeal");
        Invoke("RestorePassiveHeal", restorePassiveHealDuration);
    }

    public bool passiveHealEnabled = true;
    public float restorePassiveHealDuration = 5f;

    private void RestorePassiveHeal()
    {
        passiveHealEnabled = true;
    }

    private void StopPassiveHeal()
    {
        passiveHealEnabled = false;
    }

    IEnumerator OutOfCombatHealingCoroutine(float t) 
    {
        while (true) 
        {
            yield return new WaitForSeconds(t);

            if (passiveHealEnabled) TakeDamage(-characterAttributes.healingTick);
        }
    }
}
