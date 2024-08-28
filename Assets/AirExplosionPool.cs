using UnityEngine;
using UnityEngine.Pool;

public class AirExplosionPool : MonoBehaviour
{
    public static AirExplosionPool Instance { get; private set; }

    public ParticleSystem airExplosionPrefab;

    private ObjectPool<ParticleSystem> airExplosionPool;

    void Awake()
    {
        Instance = this;
        airExplosionPool = new ObjectPool<ParticleSystem>(
            CreateAirExplosion,
            OnGetAirExplosion,
            OnReleaseAirExplosion,
            OnDestroyAirExplosion);

        Debug.Log("AirExplosionPool initialized.");
    }

    private ParticleSystem CreateAirExplosion()
    {
        Debug.Log("Creating new air explosion instance.");
        return Instantiate(airExplosionPrefab);
    }

    private void OnGetAirExplosion(ParticleSystem explosion)
    {
        Debug.Log("Getting air explosion from pool.");
        explosion.gameObject.SetActive(true);
    }

    private void OnReleaseAirExplosion(ParticleSystem explosion)
    {
        Debug.Log("Releasing air explosion back to pool.");
        explosion.gameObject.SetActive(false);
    }

    private void OnDestroyAirExplosion(ParticleSystem explosion)
    {
        Debug.Log("Destroying air explosion instance.");
        Destroy(explosion.gameObject);
    }

    public ParticleSystem GetAirExplosion()
    {
        Debug.Log("Air explosion requested.");
        return airExplosionPool.Get();
    }

    public void ReleaseAirExplosion(ParticleSystem explosion)
    {
        Debug.Log("Air explosion released.");
        airExplosionPool.Release(explosion);
    }
}
