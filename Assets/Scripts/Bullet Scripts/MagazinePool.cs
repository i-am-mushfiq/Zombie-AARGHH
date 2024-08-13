using UnityEngine;
using System.Collections.Generic;

public class MagazinePool : MonoBehaviour
{
    public GameObject magazinePrefab;
    public int poolSize = 10;

    private List<GameObject> magazinePool;

    void Start()
    {
        InitializePool();
    }

    void InitializePool()
    {
        magazinePool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject magazine = Instantiate(magazinePrefab);
            magazine.SetActive(false);
            magazinePool.Add(magazine);
        }
    }

    public GameObject GetPooledMagazine()
    {
        foreach (GameObject magazine in magazinePool)
        {
            if (!magazine.activeInHierarchy)
            {
                return magazine;
            }
        }
        return null;
    }

    public void ReturnMagazineToPool(GameObject magazine)
    {
        magazine.SetActive(false);
    }
}
