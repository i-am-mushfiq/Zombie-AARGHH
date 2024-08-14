using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    public GameObject enemyPrefab;
    public int poolSize = 10;
    public float minSpawnTime = 2f;
    public float maxSpawnTime = 5f;

    public List<Transform> spawnPoints; // List of spawn points

    private IObjectPool<GameObject> enemyPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        enemyPool = new ObjectPool<GameObject>(
            createFunc: CreateEnemy,
            actionOnGet: OnEnemyGet,
            actionOnRelease: OnEnemyRelease,
            actionOnDestroy: OnEnemyDestroy,
            defaultCapacity: poolSize,
            maxSize: poolSize
        );

        SpawnEnemyRoutine().Forget();
    }

    private GameObject CreateEnemy()
    {
        return Instantiate(enemyPrefab);
    }

    private void OnEnemyGet(GameObject enemy)
    {
        enemy.SetActive(true);
        HealthBar healthBar = enemy.GetComponent<HealthBar>();
        if (healthBar != null)
        {
            healthBar.ResetHealth();
        }
    }

    private void OnEnemyRelease(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    private void OnEnemyDestroy(GameObject enemy)
    {
        Destroy(enemy);
    }

    private async UniTaskVoid SpawnEnemyRoutine()
    {
        while (true)
        {
            float spawnDelay = Random.Range(minSpawnTime, maxSpawnTime);
            await UniTask.Delay(System.TimeSpan.FromSeconds(spawnDelay));

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }

        GameObject enemy = enemyPool.Get();

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        enemy.transform.position = spawnPoint.position;

        // Get pool statistics
        int totalCount = ((ObjectPool<GameObject>)enemyPool).CountAll;
        int activeCount = ((ObjectPool<GameObject>)enemyPool).CountActive;
        int inactiveCount = ((ObjectPool<GameObject>)enemyPool).CountInactive;

        Debug.Log($"Pool Stats - Total: {totalCount}, Active: {activeCount}, Inactive: {inactiveCount}");
    }

    public void ReleaseEnemy(GameObject enemy)
    {
        enemyPool.Release(enemy);
    }
}
