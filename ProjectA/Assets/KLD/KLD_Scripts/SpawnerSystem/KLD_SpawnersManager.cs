using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_SpawnersManager : MonoBehaviour
{
    public static KLD_SpawnersManager Instance;

    public float levelTime { get; private set; } = 0f;

    public float enemiesCount
    {
        get { return KLD_ZombieList.Instance.GetZombies().Count; }
        private set { }
    }

    [SerializeField] int maxEnemiesAtOnce = 30;

    Vector3 spawnerToPlayer = Vector3.zero;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        levelTime += Time.deltaTime;
    }

    public bool CanSpawn(Vector3 spawnerPosition, float minDist)
    {
        spawnerToPlayer = XL_GameManager.instance.players[0].transform.position - spawnerPosition;

        return enemiesCount < maxEnemiesAtOnce
        && spawnerToPlayer.sqrMagnitude > minDist * minDist;
    }

    public void ChangeMaxEnemies(int number)
    {
        maxEnemiesAtOnce = number;
    }
}
