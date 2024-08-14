using UnityEngine;

public class MagazineSpawner : MonoBehaviour
{
    public static MagazineSpawner Instance { get; private set; }

    public MagazineHandler magazinePool;

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

    public void HandleMagazineSpawning(Vector3 enemyPosition)
    {
        GameObject magazine = magazinePool.GetPooledMagazine();
        if (magazine != null)
        {
            magazine.transform.position = enemyPosition;
            magazine.SetActive(true);
        }
        else
        {
            Debug.Log("Magazine null");
        }
    }
}
