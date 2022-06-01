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
        attributes.transform = transform;
        attributes.health = attributes.maxHealth; //Random.Range(10, 101);
        KLD_ZombieList.Instance.AddZombie(attributes);

        //UpdateHealthBar();
    }

    void OnDisable()
    {
        KLD_ZombieList.Instance.RemoveZombie(attributes);
    }

    void OnValidate()
    {
        //UpdateHealthBar();
        attributes.OnValidate();

        //if (gUIStyle == null) SetupGUIStyle();
    }
    /*
        void OnDrawGizmos()
        {
    #if UNITY_EDITOR
            Handles.Label(transform.position + Vector3.up * 3.5f, attributes.score.ToString("F1"), gUIStyle);
    #endif
        }

        // Update is called once per frame
        void Update()
        {

        }
*/
    void UpdateHealthBar()
    {
        scale.x = (attributes.health / (float)attributes.maxHealth);

        if (scale.x == Mathf.Infinity) scale.x = 1f; //debug to avoid error in console

        healthBar.localScale = scale;
    }
    /*
            [ContextMenu("Setup GUI Style")]
            void SetupGUIStyle()
            {
                GUIStyle _gui = new GUIStyle();

                _gui.normal.textColor = Color.white;
                _gui.fontSize = 30;
                _gui.alignment = TextAnchor.UpperCenter;


                gUIStyle = _gui;
            }
            */
}