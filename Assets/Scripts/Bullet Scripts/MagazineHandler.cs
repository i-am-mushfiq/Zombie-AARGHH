using UnityEngine;
using UnityEngine.Pool;

public class MagazineHandler : MonoBehaviour
{
    public GameObject magazinePrefab;
    public int initialPoolSize = 10;
    public float spawnProbability = 0.125f;

    public AmmoManager ammoManager;

    private ObjectPool<GameObject> magazinePool;

    private void Start()
    {
        if (magazinePrefab == null)
        {
            Debug.LogError("Magazine Prefab is not assigned in the Inspector.");
            return;
        }

        // Initialize the pool with a method for creating new magazines and returning magazines to the pool
        magazinePool = new ObjectPool<GameObject>(
            createFunc: CreateMagazine,
            actionOnGet: OnMagazineGet,
            actionOnRelease: OnMagazineRelease,
            actionOnDestroy: OnMagazineDestroy,
            collectionCheck: false,
            defaultCapacity: initialPoolSize,
            maxSize: initialPoolSize * 2
        );
    }

    private GameObject CreateMagazine()
    {
        return Instantiate(magazinePrefab);
    }

    private void OnMagazineGet(GameObject magazine)
    {
        magazine.SetActive(true);
    }

    private void OnMagazineRelease(GameObject magazine)
    {
        magazine.SetActive(false);
    }

    private void OnMagazineDestroy(GameObject magazine)
    {
        Destroy(magazine);
    }

    public GameObject GetPooledMagazine()
    {
        if (ammoManager == null)
        {
            Debug.Log("Ammo Manager is null");
            return null;
        }

        Debug.Log("Current Magazines: " + ammoManager.currentMagazines);
        if (ammoManager.currentMagazines <= 0 || Random.value <= spawnProbability)
        {
            return magazinePool.Get();
        }

        return null;
    }

    public void ReleaseMagazine(GameObject magazine)
    {
        if (magazine != null)
        {
            magazinePool.Release(magazine);
        }
        else
        {
            Debug.LogWarning("Attempted to release a null magazine.");
        }
    }
}
