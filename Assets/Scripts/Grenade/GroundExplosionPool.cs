using UnityEngine;
using UnityEngine.Pool;

public class GroundExplosionPool : MonoBehaviour
{
    public static GroundExplosionPool Instance { get; private set; }

    public ParticleSystem groundExplosionPrefab;

    private ObjectPool<ParticleSystem> groundExplosionPool;

    void Awake()
    {
        Instance = this;

        if (groundExplosionPrefab == null)
        {
            Debug.LogError("GroundExplosionPrefab is not assigned in the inspector!");
            return;
        }

        groundExplosionPool = new ObjectPool<ParticleSystem>(
            CreateGroundExplosion,
            OnGetGroundExplosion,
            OnReleaseGroundExplosion,
            OnDestroyGroundExplosion,
            false, // Collection check
            5, // Default capacity
            10 // Max size of pool
        );

        Debug.Log("GroundExplosionPool initialized.");

        // Pre-populate the pool with 5 instances
        PrepopulatePool(5);
    }

    private ParticleSystem CreateGroundExplosion()
    {
        Debug.Log("Creating new ground explosion instance.");
        var explosion = Instantiate(groundExplosionPrefab);
        explosion.gameObject.SetActive(false);
        return explosion;
    }

    private void OnGetGroundExplosion(ParticleSystem explosion)
    {
        Debug.Log("Getting ground explosion from pool.");
        explosion.gameObject.SetActive(true);
    }

    private void OnReleaseGroundExplosion(ParticleSystem explosion)
    {
        Debug.Log("Releasing ground explosion back to pool.");
        explosion.gameObject.SetActive(false);
    }

    private void OnDestroyGroundExplosion(ParticleSystem explosion)
    {
        Debug.Log("Destroying ground explosion instance.");
        Destroy(explosion.gameObject);
    }

    public ParticleSystem GetGroundExplosion()
    {
        Debug.Log("Ground explosion requested.");
        return groundExplosionPool.Get();
    }

    public void ReleaseGroundExplosion(ParticleSystem explosion)
    {
        Debug.Log("Ground explosion released.");
        groundExplosionPool.Release(explosion);
    }

    private void PrepopulatePool(int initialSize)
    {
        for (int i = 0; i < initialSize; i++)
        {
            ParticleSystem explosion = groundExplosionPool.Get();
            groundExplosionPool.Release(explosion);
        }

        Debug.Log($"{initialSize} ground explosions pre-populated in the pool.");
    }
}
