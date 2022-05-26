using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XL_Characters : MonoBehaviour, XL_IDamageable
{
    [SerializeField] KLD_TouchInputs touchInputs;
    [SerializeField] KLD_PlayerShoot playerShoot;
    [SerializeField] KLD_PlayerController controller;
    protected XL_CharacterAttributesSO characterAttributes;
    private float health;

    //private XL_ISpells spell;
    public float ultimateCharge;
    public bool isUltimateCharged;
    bool ultimateLaunched = false;


    enum UltState { NONE, DOWN, UP, PRESSED };

    UltState ultState = UltState.NONE;


    [SerializeField] float outOfCombatTime = 5f;
    bool outOfCombat = true;
    float curOutOfCombatTime = 0f;

    PassiveSpellInitializer passiveSpellInitializer;

    Animator animator;

    [Header("Screen Shakes")]
    [SerializeField] float takeDamage_shakeLenght = 1f;
    [SerializeField] float takeDamage_shakePower = 8f;
    [SerializeField] float takeDamage_shakeFrequency = 3f;

    [Header("UI")]
    [SerializeField] private XL_HealthBarUI characterUI;

    [SerializeField] Animator ultJoystickAnimator;
    [SerializeField] Image ultCircleImage;
    [SerializeField] Animator ultButtonAnimator;
    [SerializeField] Button ultButton;

    [SerializeField] Image[] ultButtonImages;


    bool cantCharge = false;
    float chargeRatio = 1f;

    bool tutorialUnkillable = false;

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

        passiveSpellInitializer = new PassiveSpellInitializer();
        passiveSpellInitializer.character = this;
        passiveSpellInitializer.controller = controller;

        characterAttributes.Initialize(passiveSpellInitializer);

        controller.SetBaseSpeed(characterAttributes.movementSpeed);

        health = characterAttributes.healthMax;

        ultimateCharge = 0f;
        //StartCoroutine(SpellCooldownCoroutine(ultimateChargeTick));

        curOutOfCombatTime = 99f;

        //playerShoot.canUseUltimateWhenReloading = characterAttributes.canUseSpellWhenReloading;

        //StartCoroutine(OutOfCombatHealingCoroutine(1f));
    }


    private void Update()
    {
        DoSpellCoolDown();
        DoOutOfCombatHealing();

        if (ultimateLaunched)
        {
            characterAttributes.CallOnUltLaunched(transform);
        }

        if (ultimateCharge >= 100f)
        {
            if (touchInputs.IsJoystickPressed(2))
            {
                ultState = UltState.PRESSED;
            }
            else
            {
                ultState = UltState.UP;
            }
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
            foreach (var image in ultButtonImages)
            {
                image.fillAmount = ultimateCharge * 0.01f;
            }
        }
        else
        {
            ultJoystickAnimator.SetInteger("ultState", (int)ultState);
            ultCircleImage.fillAmount = ultimateCharge * 0.01f;
        }

        if (touchInputs.GetUseButtonForUltimate())
        {
            ultButton.interactable = isUltimateCharged && (!playerShoot.isReloading || characterAttributes.canUseSpellWhenReloading);
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

        KLD_EventsManager.instance.onEnemyKill += AddUltChargeOnEnemyKill;

        //KLD_EventsManager.instance.onEnemyHit += StopPassiveHeal;
    }

    void OnDisable()
    {
        touchInputs.onActiveSkillJoystickRelease -= ActivateSpell;
        touchInputs.onActiveSkillButton -= ActivateButtonSpell;

        touchInputs.onActiveSkillJoystickDown -= CallSpellJoystickDown;

        KLD_EventsManager.instance.onEnemyKill -= AddUltChargeOnEnemyKill;

        //KLD_EventsManager.instance.onEnemyHit -= StopPassiveHeal;
    }

    Vector3 direction;

    Vector2 spellDirection;

    void DoSpellCoolDown()
    {
        if (!isUltimateCharged && !cantCharge)
        {
            ultimateCharge += Time.deltaTime * characterAttributes.activeTick * chargeRatio;

            if (ultimateCharge >= 100f)
            {
                ultimateCharge = 100f;
                isUltimateCharged = true;
            }

            characterUI.UpdateUltBar(ultimateCharge * 0.01f);
        }
    }

    void DoOutOfCombatHealing()
    {
        if (!outOfCombat)
        {
            curOutOfCombatTime += Time.deltaTime;
            if (curOutOfCombatTime > outOfCombatTime)
            {
                outOfCombat = true;
                KLD_AudioManager.Instance.PlayCharacterSound("Healing", 6);
            }
        }
        else
        {
            TakeDamage(-characterAttributes.healingTick * Time.deltaTime);
        }
    }

    void ActivateButtonSpell()
    {
        ActivateSpell(Vector2.zero);
    }

    public void ActivateSpell(Vector2 _direction)
    {
        if (ultimateCharge >= 100f)
        {
            KLD_AudioManager.Instance.PlayCharacterSound("ActiveAbility", 2);

            ultimateLaunched = true;
            playerShoot.UseUltimate(characterAttributes.spellLaunchDuration);
            spellDirection = _direction;

            ultimateCharge = 0;
            //characterUI.UpdateUltBar(ultimateCharge * 0.01f);

            isUltimateCharged = false;
            //StartCoroutine(SpellCooldownCoroutine(ultimateChargeTick));
        }
    }

    public void DoSpell() //Activated by anim
    {
        KLD_AudioManager.Instance.PlaySound("UseUltimate");
        ultimateLaunched = false;
        characterAttributes.CallOnSpellLaunch();

        characterUI.UpdateUltBar(ultimateCharge * 0.01f);

        direction = Vector3.zero;
        direction.x = spellDirection.x;
        direction.z = spellDirection.y;

        //direction = Quaternion.Euler(0f, 45f, 0f) * direction;

        //ultimateCharge = 0;
        //characterUI.UpdateUltBar(ultimateCharge * 0.01f);

        //isUltimateCharged = false;
        //StartCoroutine(SpellCooldownCoroutine(ultimateChargeTick));
        characterAttributes.ActivateSpell(direction, transform);

        StopPassiveHeal();
        //CancelInvoke("RestorePassiveHeal");
        //Invoke("RestorePassiveHeal", restorePassiveHealDuration);
    }

    IEnumerator SpellCooldownCoroutine(float t)
    {
        Debug.LogWarning("THIS SHOULD NOT BE CALLED");
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
        //transform.gameObject.SetActive(false);
        XL_GameManager.instance.LoseGame(true);
    }

    [ContextMenu("Take 100 Damage")]
    public void Take100Damage() { TakeDamage(100); }

    public void TakeDamage(float damage)
    {
        if (damage > 0f) // if it takes damage, then reduce the damage taken
        {
            damage = Mathf.Max(damage - characterAttributes.armor, 1f);
            //if ((damage - characterAttributes.armor) < 1) damage = 1; //the character will always take 1 damage;
            StopPassiveHeal();
            animator?.Play("Hit", 3, 0f);

            //tuto debug
            if (tutorialUnkillable && health < characterAttributes.healthMax / 3f
            || tutorialUnkillable && health - damage < 0f)
            {
                health = characterAttributes.healthMax / 3f - 1f;
                damage = 0f;
            }

            KLD_EventsManager.instance.InvokeLooseHealth(damage);

            KLD_AudioManager.Instance.PlayCharacterSound("TakeDamage", 5);

            KLD_ScreenShakes.instance.StartShake(takeDamage_shakeLenght, takeDamage_shakePower, takeDamage_shakeFrequency);
        }

        health -= damage;

        if (health < 1)
        {
            KLD_AudioManager.Instance.PlayCharacterSound("Death", 7);

            Die();
        }

        if (health > characterAttributes.healthMax)
        {
            health = characterAttributes.healthMax;
        }
        characterUI.UpdateHealthBar(health / characterAttributes.healthMax);

        //CancelInvoke("RestorePassiveHeal");
        //Invoke("RestorePassiveHeal", restorePassiveHealDuration);
    }

    //void RestorePassiveHeal()
    //{
    //    passiveHealEnabled = true;
    //}

    void StopPassiveHeal()
    {
        //passiveHealEnabled = false;
        outOfCombat = false;
        curOutOfCombatTime = 0f;
    }

    /*
IEnumerator OutOfCombatHealingCoroutine(float t)
{
    Debug.LogWarning("THIS SHOULD NOT BE CALLED (OOCHEALINGCOROUTINE)");
while (true)
{
    yield return new WaitForSeconds(t);

    if (passiveHealEnabled) TakeDamage(-characterAttributes.healingTick);
}
}
*/

    public float GetCharacterSpeed()
    {
        return characterAttributes.movementSpeed;
    }

    void CallSpellJoystickDown(Vector2 _joyDirection)
    {
        characterAttributes.CallUltJoystickDown(_joyDirection, transform);
    }


    public void AddUltChargeOnEnemyKill(Enemy _enemy)
    {
        if (!isUltimateCharged && !cantCharge)
        {
            ultimateCharge += playerShoot.GetWeaponUltChargeOnKill();
        }
    }

    public void SetAnimator(Animator _animator)
    {
        animator = _animator;
    }

    public string GetCharacterSoundPrefix()
    {
        return characterAttributes.characterSoundPrefix;
    }

    public int GetCharacterSoundIndex()
    {
        return characterAttributes.characterSoundIndex;
    }

    public void SetCantCharge(bool _cantCharge)
    {
        cantCharge = _cantCharge;
    }

    public void SetChargeRatio(float _ratio)
    {
        chargeRatio = _ratio;
    }

    public void SetTutorialUnkillable(bool _unkillable)
    {
        tutorialUnkillable = _unkillable;
    }
}
