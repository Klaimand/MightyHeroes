using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_Zombie : MonoBehaviour
{
    KLD_ZombieList zombieList;

    //[Range(0, 100), SerializeField] int health = 100;

    [SerializeField] KLD_ZombieAttributes attributes;

    [SerializeField] Transform healthBar;

    Vector3 scale = Vector3.one;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateHealthBar()
    {
        scale.x = (attributes.health / 100f);

        healthBar.localScale = scale;
    }
}

[System.Serializable]
public class KLD_ZombieAttributes
{
    [Range(0, 100)] public int health = 100;
    public Transform transform = null;
}