using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_Pooler : MonoBehaviour
{
    public static XL_Pooler instance;
    Dictionary<string, Pool> pools = new Dictionary<string, Pool>();
    private GameObject objectInstance;
    [SerializeField] private List<PoolKey> poolKeys = new List<PoolKey>();

    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public Queue<GameObject> queue = new Queue<GameObject>();

        public int baseCount;
        public float baseRefreshSpeed = 5;
        public float refreshSpeed = 5;
    }

    [System.Serializable]
    public class PoolKey
    {
        public string key;
        public Pool pool;
    }

    private void Awake()
    {
        instance = this;
        InitPools();
        PopulatePools();
    }

    void PopulatePools()
    {
        foreach (var pool in pools)
        {
            PopulatePool(pool.Value);
        }
    }

    void PopulatePool(Pool pool)
    {
        for (int j = 0; j < pool.baseCount; j++)
        {
            AddInstance(pool);
        }
    }

    void AddInstance(Pool pool)
    {
        objectInstance = Instantiate(pool.prefab, transform);
        objectInstance.SetActive(false);
        pool.queue.Enqueue(objectInstance);
    }

    private int i;

    void InitPools()
    {
        for (int i = 0; i < poolKeys.Count; i++)
        {
            pools.Add(poolKeys[i].key, poolKeys[i].pool);
        }
    }

    void InitRefreshCount()
    {
        foreach (KeyValuePair<string, Pool> pool in pools)
        {
            StartCoroutine(RefreshPool(pool.Value, pool.Value.baseRefreshSpeed));
        }
    }

    IEnumerator RefreshPool(Pool pool, float t)
    {
        yield return new WaitForSeconds(t);

        if (pool.queue.Count < pool.baseCount)
        {
            AddInstance(pool);
            pool.refreshSpeed = pool.baseRefreshSpeed * pool.queue.Count / pool.baseCount;
        }

        StartCoroutine(RefreshPool(pool, pool.refreshSpeed));
    }


    private void Start()
    {
        InitRefreshCount();
    }

    public GameObject Pop(string key)
    {
        if (pools[key].queue.Count != 0)
        {
            objectInstance = pools[key].queue.Dequeue();
            objectInstance.SetActive(true);
        }
        else
        {
            Debug.LogWarning("pool of" + key + "is empty");
            AddInstance(pools[key]);
            objectInstance = pools[key].queue.Dequeue();
            objectInstance.SetActive(true);
        }

        return objectInstance;
    }

    public void DePop(string key, GameObject go)
    {
        pools[key].queue.Enqueue(go);

        go.transform.parent = transform;
        go.SetActive(false);
    }

    public void DePopAll()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void DelayedDePop(float t, string key, GameObject go)
    {
        StartCoroutine((DelayedDePopCoroutine(t, key, go)));
    }

    IEnumerator DelayedDePopCoroutine(float t, string key, GameObject go)
    {
        yield return new WaitForSeconds(t);
        DePop(key, go);
    }
}
