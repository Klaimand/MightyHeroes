using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_SpawnersManager : MonoBehaviour
{
    public static KLD_SpawnersManager Instance;

    public float levelTime { get; private set; } = 0f;

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
}
