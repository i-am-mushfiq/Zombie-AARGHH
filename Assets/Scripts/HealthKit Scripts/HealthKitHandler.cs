using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class HealthKitHandler : MonoBehaviour
{
    public static HealthKitHandler Instance { get; private set; }

    public GameObject healthKitPrefab;
    public int poolSize = 10;

    private IObjectPool<GameObject> healthKitPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        healthKitPool = new ObjectPool<GameObject>(
            createFunc: CreateHealthKit,
            actionOnGet: OnHealthKitGet,
            actionOnRelease: OnHealthKitRelease,
            actionOnDestroy: OnHealthKitDestroy,
            defaultCapacity: poolSize,
            maxSize: poolSize
        );
    }

    private GameObject CreateHealthKit()
    {
        return Instantiate(healthKitPrefab);
    }

    private void OnHealthKitGet(GameObject healthKit)
    {
        healthKit.SetActive(true);
    }

    private void OnHealthKitRelease(GameObject healthKit)
    {
        healthKit.SetActive(false);
    }

    private void OnHealthKitDestroy(GameObject healthKit)
    {
        Destroy(healthKit);
    }

    public void SpawnHealthKit(Vector3 spawnPosition)
    {
        GameObject healthKit = healthKitPool.Get();

        healthKit.transform.position = spawnPosition;
    }

    public void ReleaseHealthKit(GameObject healthKit)
    {
        healthKitPool.Release(healthKit);
    }
}