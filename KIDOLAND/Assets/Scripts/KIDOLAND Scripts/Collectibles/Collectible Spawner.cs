using UnityEngine;
using System.Collections.Generic;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject collectiblePrefab;

    [Header("Pool")]
    [SerializeField] private int poolSize = 10;
    [SerializeField] private int maxActive = 5;

    [Header("Spawn Area")]
    [SerializeField] private Vector2 areaSize = new Vector2(10f, 10f);

    [Header("Spacing")]
    [SerializeField] private float minDistanceBetweenCollectibles = 1.5f;
    [SerializeField] private int maxSpawnAttempts = 20;

    private List<GameObject> pool = new List<GameObject>();
    private List<GameObject> activeCollectibles = new List<GameObject>();

    private void Awake()
    {
        InitializePool();
    }

    private void OnEnable()
    {
        GameEvents.OnCollectiblePicked += HandleCollectiblePicked;
    }

    private void OnDisable()
    {
        GameEvents.OnCollectiblePicked -= HandleCollectiblePicked;
    }

    private void Start()
    {
        for (int i = 0; i < maxActive; i++)
        {
            SpawnFromPool();
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(collectiblePrefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    private void HandleCollectiblePicked(int value)
    {
        CleanupInactive();
        while (activeCollectibles.Count < maxActive) // safety check if the max count is low and it doesn't spawn
            SpawnFromPool();
    }

    private void SpawnFromPool()
    {
        if (activeCollectibles.Count >= maxActive)
            return;

        GameObject obj = GetInactiveFromPool();
        if (obj == null)
            return;

        Vector2 spawnPos = GetValidSpawnPosition();

        obj.transform.position = spawnPos;
        obj.SetActive(true);

        activeCollectibles.Add(obj);
    }

    private GameObject GetInactiveFromPool()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }
        return null;
    }

    private void CleanupInactive()
    {
        activeCollectibles.RemoveAll(item => item == null || !item.activeInHierarchy);
    }

    private Vector2 GetValidSpawnPosition()
    {
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Vector2 candidate = GetRandomPosition();

            if (IsFarEnough(candidate))
                return candidate;
        }

        // fallback (rare)
        return GetRandomPosition();
    }

    private Vector2 GetRandomPosition()
    {
        Vector2 center = transform.position;

        float x = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float y = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);

        return center + new Vector2(x, y);
    }

    private bool IsFarEnough(Vector2 pos)
    {
        foreach (var obj in activeCollectibles)
        {
            if (obj == null || !obj.activeInHierarchy)
                continue;

            float dist = Vector2.Distance(pos, obj.transform.position);
            if (dist < minDistanceBetweenCollectibles)
                return false;
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}