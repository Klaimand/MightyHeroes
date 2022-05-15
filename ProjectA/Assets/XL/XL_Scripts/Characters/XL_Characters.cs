using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XL_Characters : MonoBehaviour, XL_IDamageable
{
    [SerializeField] KLD_TouchInputs touchInputs;
    [SerializeField] KLD_PlayerShoot playerShoot;
    protected XL_CharacterAttributesSO characterAttributes;
    private float health;

    //private XL_ISpells spell;
    public float ultimateCharge;
    public bool isUltimateCharged;
    [SerializeField] private float ultimateChargeTick;

    [SerializeField] private XL_HealthBarUI characterUI;

    [SerializeField] Animator ultJoystickAnimator;
    [SerializeField] Animator ultButtonAnimator;
    [SerializeField] Button ultButton;

    enum UltState { NONE, DOWN, UP };

    UltState ultState = UltState.NONE;

    /*
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
    */

    public void InitializeCharacter(XL_CharacterAttributesSO _character, int _characterLevel)
    {
        characterAttributes = _character;
        characterAttributes.level = _characterLevel;

        characterAttributes.Initialize();
        health = characterAttributes.healthMax;

        ultimateCharge = 0f;
        StartCoroutine(SpellCooldownCoroutine(ultimateChargeTick));
        StartCoroutine(OutOfCombatHealingCoroutine(1f));
    }


    private void Update()
    {
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

        if (touchInputs.GetUseButtonForUltimate())
        {
            ultButton.interactable = isUltimateCharged && !playerShoot.isReloading;
        }
        else
        {
            touchInputs.SetJoystickInterractable(2, isUltimateCharged && !playerShoot.isReloading);
        }

    }

    void OnEnable()
    {
        touchInputs.onActiveSkillJoystickRelease += ActivateSpell;
        touchInputs.onActiveSkillButton += ActivateButtonSpell;

        touchInputs.onActiveSkillJoystickDown += CallSpellJoystickDown;
    }

    void OnDisable()
    {
        touchInputs.onActiveSkillJoystickRelease -= ActivateSpell;
        touchInputs.onActiveSkillButton -= ActivateButtonSpell;

        touchInputs.onActiveSkillJoystickDown -= CallSpellJoystickDown;
    }

    Vector3 direction;

    Vector2 spellDirection;

    void ActivateButtonSpell()
    {
        ActivateSpell(Vector2.zero);
    }

    public void ActivateSpell(Vector2 _direction)
    {
        if (ultimateCharge >= 100f)
        {
            playerShoot.UseUltimate(characterAttributes.spellLaunchDuration);
            spellDirection = _direction;

            ultimateCharge = 0;
            //characterUI.UpdateUltBar(ultimateCharge * 0.01f);

            isUltimateCharged = false;
            StartCoroutine(SpellCooldownCoroutine(ultimateChargeTick));
        }
    }

    public void DoSpell() //Activated by anim
    {
        characterUI.UpdateUltBar(ultimateCharge * 0.01f);

        direction = Vector3.zero;
        direction.x = spellDirection.x;
        direction.z = spellDirection.y;

        direction = Quaternion.Euler(0f, 45f, 0f) * direction;

        //ultimateCharge = 0;
        //characterUI.UpdateUltBar(ultimateCharge * 0.01f);

        //isUltimateCharged = false;
        //StartCoroutine(SpellCooldownCoroutine(ultimateChargeTick));
        characterAttributes.ActivateSpell(direction, transform);

        StopPassiveHeal();
        CancelInvoke("RestorePassiveHeal");
        Invoke("RestorePassiveHeal", restorePassiveHealDuration);
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

    public float GetCharacterSpeed()
    {
        return characterAttributes.movementSpeed;
    }

    void CallSpellJoystickDown(Vector2 _joyDirection)
    {
        characterAttributes.CallUltJoystickDown(_joyDirection, transform);
    }
}
