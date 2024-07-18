using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            float spawnDelay = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnDelay);

            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                // Reset enemy's health (assuming HealthBar is a component of the enemyPrefab)
                HealthBar healthBar = enemy.GetComponent<HealthBar>();
                if (healthBar != null)
                {
                    healthBar.ResetHealth(); // Call the ResetHealth() method of HealthBar
                }

                enemy.transform.position = transform.position;
                enemy.SetActive(true);
                break;
            }
        }
    }
}
