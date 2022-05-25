using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public abstract class XL_Enemy : MonoBehaviour, XL_IDamageable
{
    [Header("Sound")]
    [SerializeField] Vector2 minMaxTimeBetweenChaseSounds = new Vector2(7f, 15f);
    float curTimeBetweenChaseSound = 0f;
    float curTimeSinceLastChaseSound = 0f;
    [SerializeField] protected KLD_SelfAudioManager selfAudioManager;

    [Space(10)]
    [SerializeField] Animator hitAnimator;
    [SerializeField] int hitAnimatorLayer = 0;

    [SerializeField] private KLD_ZombieAttributes attributes;
    protected float health;


    //ATTACK
    [SerializeField] protected int damage;
    public bool isAlerted; // public for tests, set to protected after tests !
    protected bool canAttack;
    protected Transform targetedPlayer;
    [SerializeField] protected float targetedPlayerUpdateRate;

    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected float speed;

    [SerializeField] XL_HealthBarUI healthBar;

    Vector3 scale = Vector3.one;

    //static GUIStyle gUIStyle;

    bool firstInit = false;
    bool didFirstDisable = false;

    protected virtual void Start()
    {
        firstInit = true;
        InitZombieList();
        Initialize();
        Alert();
    }

    protected virtual void Update()
    {
        if (curTimeSinceLastChaseSound > curTimeBetweenChaseSound)
        {
            curTimeBetweenChaseSound = Random.Range(minMaxTimeBetweenChaseSounds.x, minMaxTimeBetweenChaseSounds.y);
            curTimeBetweenChaseSound = 0f;
            selfAudioManager.PlaySound("Chase");
        }
        curTimeSinceLastChaseSound += Time.deltaTime;
    }

    protected virtual void Initialize()
    {
        //Debug.Log("Initialized");
        health = attributes.maxHealth;
        agent.speed = speed;
        canAttack = true;
        healthBar.UpdateHealthBar(health / attributes.maxHealth);
    }

    public abstract void Alert();
    public abstract void Move();
    public abstract void Attack();

    public virtual void Die()
    {
        selfAudioManager.PlaySound("Death");
        //Debug.Log("Remove enemy : " + this.name);
        //XL_GameManager.instance.RemoveEnemyAttributes(attributes);
    }

    protected int targetedPlayerIdx = 0;
    protected float targetedPlayerDistance = 0;
    protected float nextTargetedPlayerDistance = 0;

    protected IEnumerator FindTargetedPlayerCoroutine(float t)
    {
        FindNearestPlayer();

        yield return new WaitForSeconds(t);

        StartCoroutine(FindTargetedPlayerCoroutine(targetedPlayerUpdateRate));
    }


    private int i;
    private void FindNearestPlayer()
    {


        targetedPlayerIdx = 0;
        i = 0;
        targetedPlayerDistance = 100000;

        while (i < XL_GameManager.instance.players.Count)
        {
            if (i > 0)
            {
                nextTargetedPlayerDistance = (transform.position - XL_GameManager.instance.players[i].transform.position).magnitude;    // distance between player i and the enemy
                if (targetedPlayerDistance > nextTargetedPlayerDistance)
                {
                    targetedPlayerIdx = i;
                    targetedPlayerDistance = nextTargetedPlayerDistance;
                }
            }
            else targetedPlayerDistance = (transform.position - XL_GameManager.instance.players[targetedPlayerIdx].transform.position).magnitude; // distance between player i and the enemy

            i++;
        }

        if (i > 0) targetedPlayer = XL_GameManager.instance.players[targetedPlayerIdx].transform;
    }

    protected IEnumerator AttackCooldownCoroutine(float t)
    {
        canAttack = false;
        yield return new WaitForSeconds(t);
        agent.isStopped = false;
        canAttack = true;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.UpdateHealthBar(health / attributes.maxHealth);
        if (hitAnimator != null)
        {
            hitAnimator.Play("Hit", hitAnimatorLayer, 0f);
        }
        KLD_EventsManager.instance.InvokeEnemyHit();
        if (health < 1) Die();
    }

    

    protected virtual void OnEnable()
    {
        if (firstInit)
        {
            InitZombieList();
            Initialize();
            Alert();
        }
        if (hitAnimator != null)
        {
            hitAnimator.Play("Hit", hitAnimatorLayer, 1f);
        }

        //attributes.maxHealth = Random.Range(10, 101);
        curTimeBetweenChaseSound = Random.Range(minMaxTimeBetweenChaseSounds.x, minMaxTimeBetweenChaseSounds.y);
        curTimeSinceLastChaseSound = 0f;
    }

    void OnDisable()
    {
        KLD_ZombieList.Instance.RemoveZombie(attributes);
        if (didFirstDisable)
        {
        }
        else
        {
            didFirstDisable = true;
        }
    }

    void InitZombieList()
    {
        KLD_ZombieList.Instance.AddZombie(attributes);
        attributes.transform = transform;
    }


    public KLD_ZombieAttributes GetZombieAttributes()
    {
        return attributes;
    }

    /*void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.Label(transform.position + Vector3.up * 3.5f, attributes.score.ToString(), gUIStyle);
#endif
    }

    [ContextMenu("Setup GUI Style")]
    void SetupGUIStyle()
    {
        GUIStyle _gui = new GUIStyle();

        _gui.normal.textColor = Color.white;
        _gui.fontSize = 30;
        _gui.alignment = TextAnchor.UpperCenter;


        gUIStyle = _gui;
    }*/
}
