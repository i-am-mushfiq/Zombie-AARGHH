using UnityEngine;

public class MagazinePool : MonoBehaviour
{
    public GameObject magazinePrefab;
    public int initialPoolSize = 10;

    private void Start()
    {
        if (magazinePrefab == null)
        {
            Debug.LogError("Magazine Prefab is not assigned in the Inspector.");
        }
    }
    public GameObject GetPooledMagazine()
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
