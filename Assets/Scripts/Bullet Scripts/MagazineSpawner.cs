using UnityEngine;

public class MagazineSpawner : MonoBehaviour
{
    public MagazineHandler magazinePool;

    public void HandleMagazineSpawning(Vector3 enemyPosition)
    {
        
        GameObject magazine = magazinePool.GetPooledMagazine();
        if (magazine != null)
        {
            //Debug.Log("Magazine not null");
            magazine.transform.position = enemyPosition;
            magazine.SetActive(true);
        }
        else
        {
            Debug.Log("Magazine null");
        }
    }
}
