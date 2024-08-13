using UnityEngine;

public class MagazineSpawner : MonoBehaviour
{
    public MagazinePool magazinePool;

    private void Start()
    {

    }

    private void HandleEnemyDeath(Transform enemyTransform)
    {
        Debug.Log("Inside HandleEnemyDeath");
        GameObject magazine = magazinePool.GetPooledMagazine();
        if (magazine != null)
        {
            magazine.transform.position = enemyTransform.position;
            magazine.SetActive(true);
        }
    }

    public void handleMagazineSpawning()
    {
        Debug.Log("in handleMagazineSpawning");
    }
}
