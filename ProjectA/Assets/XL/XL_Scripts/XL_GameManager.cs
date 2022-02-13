using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_GameManager : MonoBehaviour
{
    public static XL_GameManager instance;
    public GameObject[] players = new GameObject[3];

    private void Awake()
    {
        instance = this;
    }

    public GameObject[] GetPlayers() 
    {
        return players;
    }

}
