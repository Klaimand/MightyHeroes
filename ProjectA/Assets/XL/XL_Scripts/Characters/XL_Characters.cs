using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Characters : MonoBehaviour, XL_IDamageable
{
    [SerializeField] KLD_TouchInputs touchInputs;
    [SerializeField] protected XL_CharacterAttributesSO characterAttributes;
    private float health;

    private XL_ISpells spell;
    public float ultimateCharge;
    public bool isUltimateCharged;
    [SerializeField] private float ultimateChargeTick;

    [SerializeField] private XL_HealthBarUI characterUI;

    [SerializeField] Animator ultJoystickAnimator;
    [SerializeField] Animator ultButtonAnimator;

    enum UltState { NONE, DOWN, UP };

    UltState ultState = UltState.NONE;

    private void Awake()
    {
        characterAttributes.Initialize();
        health = characterAttributes.healthMax;
    }
    private void Start()
    {
        ultimateCharge = 0;
        spell = transform.GetComponent<XL_ISpells>();
        StartCoroutine(SpellCooldownCoroutine(ultimateChargeTick));
        StartCoroutine(OutOfCombatHealingCoroutine(1f));
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("SPELL");
            ActivateSpell(transform.forward);
        }*/

        if (ultimateCharge >= 100f)
        {
            ultState = UltState.UP;
        }
        else if (ultimateCharge > 50f)
        {
            ultState = UltState.DOWN;
        }
        else
        {
            ultState = UltState.NONE;
        }

        if (touchInputs.GetUseButtonForUltimate())
        {
            ultButtonAnimator.SetInteger("ultState", (int)ultState);
        }
        else
        {
            ultJoystickAnimator.SetInteger("ultState", (int)ultState);
        }

        touchInputs.SetJoystickInterractable(2, isUltimateCharged);

    }

    void OnEnable()
    {
        touchInputs.onActiveSkillJoystickRelease += ActivateSpell;
    }

    void OnDisable()
    {
        touchInputs.onActiveSkillJoystickRelease -= ActivateSpell;
    }

    Vector3 direction;
    public void ActivateSpell(Vector2 _direction)
    {
        if (ultimateCharge >= 100f)
        {
            direction = Vector3.zero;
            direction.x = _direction.x;
            direction.z = _direction.y;

            direction = Quaternion.Euler(0f, 45f, 0f) * direction;

            ultimateCharge = 0;
            characterUI.UpdateUltBar(ultimateCharge * 0.01f);

            isUltimateCharged = false;
            StartCoroutine(SpellCooldownCoroutine(ultimateChargeTick));
            characterAttributes.ActivateSpell(direction, transform);

            StopPassiveHeal();
            CancelInvoke("RestorePassiveHeal");
            Invoke("RestorePassiveHeal", restorePassiveHealDuration);
        }
        //else return false;
        //return true;
    }

    IEnumerator SpellCooldownCoroutine(float t)
    {

        yield return new WaitForEndOfFrame();
        ultimateCharge += characterAttributes.activeTick * Time.deltaTime;
        characterUI.UpdateUltBar(ultimateCharge * 0.01f);
        if (ultimateCharge < 100)
        {
            StartCoroutine(SpellCooldownCoroutine(t));
        }
        else isUltimateCharged = true;
    }


    public void Die()
    {
        StopAllCoroutines();
        //XL_GameManager.instance.RemovePlayer(transform.gameObject);
        transform.gameObject.SetActive(false);
    }

    [ContextMenu("Take 100 Damage")]
    public void Take100Damage() { TakeDamage(100); }

    public void TakeDamage(float damage)
    {
        if (damage > 0) // if it takes damage, then reduce the damage taken
        {
            if ((damage - characterAttributes.armor) < 1) damage = 1; //the character will always take 1 damage;
        }

        health -= damage;

        if (health < 1)
        {
            Die();
        }

        if (health > characterAttributes.healthMax)
        {
            health = characterAttributes.healthMax;
        }
        characterUI.UpdateHealthBar(health / characterAttributes.healthMax);

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

    public void InitializeCharacterStats(int _characterLevel)
    {
        characterAttributes.level = _characterLevel;
        characterAttributes.Initialize();
        health = characterAttributes.healthMax;
    }

    public float GetCharacterSpeed()
    {
        return characterAttributes.movementSpeed;
    }
}
