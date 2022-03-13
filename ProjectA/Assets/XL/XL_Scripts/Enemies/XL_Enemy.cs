using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class XL_Enemy : MonoBehaviour, XL_IDamageable
{
    [SerializeField] private KLD_ZombieAttributes attributes;
    protected float health;

    //ATTACK
    public bool isAlerted; // public for tests, set to protected after tests !
    protected bool canAttack;
    protected Transform targetedPlayer;
    [SerializeField] protected float targetedPlayerUpdateRate;

    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected float speed;

    [Header("Pooler")]
    [SerializeField] protected XL_Pooler pooler;

    [SerializeField] Transform healthBar;

    Vector3 scale = Vector3.one;

    private void Start()
    {
        Initialize();
        Alert();
    }

    protected void Initialize()
    {
        //Debug.Log("Initialized");
        health = attributes.maxHealth;
        agent.speed = speed;
        canAttack = true;
    }

    public abstract void Alert();
    public abstract void Move();
    public abstract void Attack();
    public abstract void Die();


    protected int targetedPlayerIdx = 0;
    protected float targetedPlayerDistance = 0;
    protected float nextTargetedPlayerDistance = 0;

    protected IEnumerator FindTargetedPlayerCoroutine(float t)
    {
        FindNearestPlayer();

        yield return new WaitForSeconds(t);

        StartCoroutine(FindTargetedPlayerCoroutine(targetedPlayerUpdateRate));
    }

    private void FindNearestPlayer()
    {
        targetedPlayerIdx = 0;
        //Debug.Log("player 0 : " + XL_GameManager.instance.GetPlayers());
        targetedPlayerDistance = (transform.position - XL_GameManager.instance.players[targetedPlayerIdx].transform.position).magnitude;    // distance between player 0 and the enemy
        

        for (int i = 1; i < XL_GameManager.instance.players.Length; i++)
        {
            nextTargetedPlayerDistance = (transform.position - XL_GameManager.instance.players[i].transform.position).magnitude;    // distance between player i and the enemy
            if (targetedPlayerDistance > nextTargetedPlayerDistance)
            {
                targetedPlayerIdx = i;
                targetedPlayerDistance = nextTargetedPlayerDistance;
            }
        }

        targetedPlayer = XL_GameManager.instance.players[targetedPlayerIdx].transform;
    }

    protected IEnumerator AttackCooldownCoroutine(float t) 
    {
        canAttack = false;
        yield return new WaitForSeconds(t);
        agent.isStopped = false;
        canAttack = true;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 1) Die();
    }

    void OnEnable()
    {
        //"ZombieList" things here

        attributes.maxHealth = Random.Range(10, 101);
        UpdateHealthBar();
    }

    void OnDisable()
    {
        //"ZombieList" things here
    }

    void OnValidate()
    {
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        scale.x = (health / 100f);

        healthBar.localScale = scale;
    }
}
