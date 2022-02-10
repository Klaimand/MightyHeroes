using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class KLD_Zombie : MonoBehaviour
{
    KLD_ZombieList zombieList;

    //[Range(0, 100), SerializeField] int health = 100;

    [SerializeField] KLD_ZombieAttributes attributes;

    [SerializeField] Transform healthBar;

    Vector3 scale = Vector3.one;

    static GUIStyle gUIStyle;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        if (zombieList == null)
        {
            zombieList = GameObject.Find("ZombieManager").GetComponent<KLD_ZombieList>();
        }
        if (attributes.transform == null)
        {
            attributes.transform = transform;
        }

        zombieList.AddZombie(attributes);

        attributes.health = Random.Range(10, 101);
        UpdateHealthBar();
    }

    void OnDisable()
    {
        zombieList.RemoveZombie(attributes);
    }

    void OnValidate()
    {
        UpdateHealthBar();
        attributes.OnValidate();

        if (gUIStyle == null) SetupGUIStyle();
    }

    void OnDrawGizmos()
    {
        Handles.Label(transform.position + Vector3.up * 3.5f, attributes.score.ToString(), gUIStyle);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateHealthBar()
    {
        scale.x = (attributes.health / (float)attributes.maxHealth);

        if (scale.x == Mathf.Infinity) scale.x = 1f; //debug to avoid error in console

        healthBar.localScale = scale;
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