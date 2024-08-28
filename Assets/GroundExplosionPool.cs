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
        groundExplosionPool = new ObjectPool<ParticleSystem>(
            CreateGroundExplosion,
            OnGetGroundExplosion,
            OnReleaseGroundExplosion,
            OnDestroyGroundExplosion);

        Debug.Log("GroundExplosionPool initialized.");
    }

    private ParticleSystem CreateGroundExplosion()
    {
        Debug.Log("Creating new ground explosion instance.");
        return Instantiate(groundExplosionPrefab);
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
}
