using UnityEngine;

public class MagazineHandler : MonoBehaviour
{
    public GameObject magazinePrefab;
    public int initialPoolSize = 10;
    public float spawnProbability = 0.125f;

    public AmmoManager ammoManager;

    private void Start()
    {
        if (magazinePrefab == null)
        {
            Debug.LogError("Magazine Prefab is not assigned in the Inspector.");
        }
    }

    public GameObject GetPooledMagazine()
    {
        if (ammoManager == null)
        {
            Debug.Log("Ammo Manager null");
        }
        Debug.Log("Current Magazines: " + ammoManager.currentMagazines);
        if (ammoManager.currentMagazines <= 0)
        {
            return SpawnMagazine();
        }
        else
        {
            if (Random.value <= spawnProbability)
            {
                return SpawnMagazine();
            }
            return null;
        }
        return null;
    }

    public GameObject SpawnMagazine()
    {
        if (magazinePrefab == null)
        {
            Debug.LogError("Magazine Prefab is not assigned.");
            return null;
        }

        Vector3 spawnPosition = new Vector3(100, 100, 0);
        GameObject magazine = Instantiate(magazinePrefab, spawnPosition, Quaternion.identity);
        return magazine;
    }

    public void DestroyMagazine(GameObject magazine)
    {
        if (magazine != null)
        {
            Destroy(magazine);
        }
        else
        {
            Debug.LogWarning("Attempted to destroy a null magazine.");
        }
    }
}
