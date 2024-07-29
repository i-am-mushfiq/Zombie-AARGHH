using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public int poolSize = 20;

    private List<GameObject> enemyPool;

    public float minSpawnTime = 2f;
    public float maxSpawnTime = 5f;

    void Start()
    {
        enemyPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
        SpawnEnemyRoutine().Forget();
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

    void SpawnEnemy()
    {
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                HealthBar healthBar = enemy.GetComponent<HealthBar>();
                if (healthBar != null)
                {
                    healthBar.ResetHealth();
                }

                enemy.transform.position = transform.position;
                enemy.SetActive(true);
                break;
            }
        }
    }
}
