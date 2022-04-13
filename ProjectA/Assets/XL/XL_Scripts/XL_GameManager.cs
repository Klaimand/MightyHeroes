using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XL_GameManager : MonoBehaviour
{
    public static XL_GameManager instance;
    public List<GameObject> players = new List<GameObject>();
    public List<XL_Enemy> enemies = new List<XL_Enemy>();
    public KLD_ZombieList zombieList;
    private XL_GameMode gameMode;

    [SerializeField] private GameObject EndingScreen;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        instance = this;
    }

    public void WinGame() 
    {
        DeactivateAllEnemyScript();
        text.text = "Mission accomplished !";
        EndingScreen.SetActive(true);
        Debug.Log("Game won !");
        
    }

    public void LoseGame() 
    {
        DeactivateAllEnemyScript();
        text.text = "Mission failed !";
        EndingScreen.SetActive(true);
        Debug.Log("Game Lost !");
    }

    public void AddPlayer(GameObject player) 
    {
        players.Add(player);
    }

    public void RemovePlayer(GameObject player) 
    {
        players.Remove(player);
    }

    public void AddEnemy(XL_Enemy enemy) 
    {
        enemies.Add(enemy);
        AddEnemyAttributes(enemy.GetZombieAttributes());
    }

    public void RemoveEnemy(XL_Enemy enemy) 
    {
        enemies.Remove(enemy);
        RemoveEnemyAttributes(enemy.GetZombieAttributes());
    }

    private void DeactivateAllEnemyScript() 
    {
        foreach (XL_Enemy enemy in enemies) 
        {
            enemy.enabled = false;
        }
    }

    public void AddEnemyAttributes(KLD_ZombieAttributes zombieAttributes) 
    {
        zombieList.AddZombie(zombieAttributes);
    }

    public void RemoveEnemyAttributes(KLD_ZombieAttributes zombieAttributes) 
    {
        zombieList.RemoveZombie(zombieAttributes);
    }

}
