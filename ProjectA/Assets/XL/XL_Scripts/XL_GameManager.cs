using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_GameManager : MonoBehaviour
{
    public static XL_GameManager instance;
    //public GameObject[] players = new GameObject[3];
    public List<GameObject> players = new List<GameObject>();
    public KLD_ZombieList zombieList;

    private void Awake()
    {
        instance = this;
    }

    public void AddPlayer(GameObject player) 
    {
        players.Add(player);
    }

    public void removePlayer(GameObject player) 
    {
        players.Remove(player);
    }

    public void AddEnemy(KLD_ZombieAttributes zombieAttributes) 
    {
        zombieList.AddZombie(zombieAttributes);
    }

    public void RemoveEnemy(KLD_ZombieAttributes zombieAttributes) 
    {
        zombieList.RemoveZombie(zombieAttributes);
    }

}
