using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public abstract class XL_Enemy : MonoBehaviour, XL_IDamageable
{
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

    [Header("Pooler")]
    [SerializeField] protected XL_Pooler pooler;

    [SerializeField] Transform healthBar;

    Vector3 scale = Vector3.one;

    static GUIStyle gUIStyle;

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
        UpdateHealthBar();
    }

    public abstract void Alert();
    public abstract void Move();
    public abstract void Attack();

    public virtual void Die() 
    {
        Debug.Log("Remove enemy : " + this.name);
        XL_GameManager.instance.RemoveEnemyAttributes(attributes);
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

    private void FindNearestPlayer()
    {
        targetedPlayerIdx = 0;
        //Debug.Log("player 0 : " + XL_GameManager.instance.GetPlayers());
        targetedPlayerDistance = (transform.position - XL_GameManager.instance.players[targetedPlayerIdx].transform.position).magnitude;    // distance between player 0 and the enemy
        

        for (int i = 1; i < XL_GameManager.instance.players.Count; i++)
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
        UpdateHealthBar();
        if (health < 1) Die();
    }

    void OnEnable()
    {
        //"ZombieList" things here

        attributes.maxHealth = Random.Range(10, 101);
        Initialize();
    }

    void OnDisable()
    {
        //"ZombieList" things here
    }

    void OnValidate()
    {
        UpdateHealthBar();

        if (gUIStyle == null) SetupGUIStyle();
    }

    void UpdateHealthBar()
    {
        scale.x = (health / 100f);

        healthBar.localScale = scale;
    }

    public KLD_ZombieAttributes GetZombieAttributes() 
    {
        return attributes;
    }

    void OnDrawGizmos()
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
    }
}
