using UnityEngine;
using UnityEngine.Pool;

public class GrenadeController : MonoBehaviour
{
    public static GrenadeController Instance { get; private set; }  // Singleton instance

    public GameObject grenadePrefab;    // Reference to the grenade prefab
    public Transform throwPoint;        // Point from which the grenade will be thrown
    public float throwForce = 10f;      // Force applied to throw the grenade
    public float throwAngle = 45f;      // Angle at which the grenade will be thrown

    private ObjectPool<GameObject> grenadePool;  // Pool for grenades

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);  // Optionally persist this instance across scenes
    }

    void Start()
    {
        // Initialize the grenade pool with initial size 10, maximum size 20
        grenadePool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(grenadePrefab),
            actionOnGet: grenade => grenade.SetActive(true),
            actionOnRelease: grenade => grenade.SetActive(false),
            actionOnDestroy: Destroy,
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 20
        );
    }

    public void Throw(bool throwRight)
    {
        if (grenadePrefab == null || throwPoint == null)
        {
            Debug.Log("GrenadePrefab or ThrowPoint is not assigned.");
            return;
        }

        // Get a grenade from the pool
        GameObject grenade = grenadePool.Get();
        grenade.transform.position = throwPoint.position;
        grenade.transform.rotation = throwPoint.rotation;

        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float radians = throwAngle * Mathf.Deg2Rad;
            Vector2 throwDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

            if (!throwRight)
            {
                throwDirection.x *= -1;
            }

            rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody2D component not found on the grenade prefab.");
        }
    }

    public void ReleaseGrenade(GameObject grenade)
    {
        grenadePool.Release(grenade);
    }
}
