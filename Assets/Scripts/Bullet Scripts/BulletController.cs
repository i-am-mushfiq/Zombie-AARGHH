using UnityEngine;
using System.Collections.Generic;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public GameObject player;
    public GameObject bulletPrefab;
    public int bulletPoolSize = 30;
    public Transform spawnPoint;
    public PlayerController playerController;
    public AmmoManager ammoManager;
    public MuzzleHandler muzzleHandler;

    private List<GameObject> bulletPool;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        ammoManager = player.GetComponent<AmmoManager>();

        bulletPool = new List<GameObject>();
        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    public void SpawnBullet()
    {
        
        if (ammoManager.UseAmmo())
        {
            foreach (GameObject bullet in bulletPool)
            {
                if (!bullet.activeInHierarchy)
                {
                    bullet.transform.position = spawnPoint.position;
                    bullet.SetActive(true);

                    bullet.GetComponent<Bullet>().SetDirection(playerController.isFacingRight ? Vector2.right : Vector2.left);
                    if (muzzleHandler != null)
                    {
                        muzzleHandler.gameObject.SetActive(true);
                        muzzleHandler.PlayAnimationAndDeactivate();
                    }
                    break;
                }
            }
        }
    }
}
