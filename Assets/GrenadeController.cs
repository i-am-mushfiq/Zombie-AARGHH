using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(LineRenderer))]
public class GrenadeController : MonoBehaviour
{
    public static GrenadeController Instance { get; private set; }

    public GameObject grenadePrefab;
    public Transform throwPoint;
    public float throwForce = 10f;
    public float throwAngle = 45f;

    private ObjectPool<GameObject> grenadePool;
    private LineRenderer trajectoryLine;
    public int maxPoints = 50;
    public float increment = 0.025f;
    public float rayOverlap = 1.1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        grenadePool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(grenadePrefab),
            actionOnGet: grenade => grenade.SetActive(true),
            actionOnRelease: grenade => grenade.SetActive(false),
            actionOnDestroy: Destroy,
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 20
        );

        trajectoryLine = GetComponent<LineRenderer>();
        trajectoryLine.positionCount = maxPoints;

        ShowTrajectory();
    }

    void Update()
    {
        PredictTrajectory();
    }

    public void Throw(bool throwRight)
    {
        if (grenadePrefab == null || throwPoint == null)
        {
            Debug.Log("GrenadePrefab or ThrowPoint is not assigned.");
            return;
        }

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

    private void PredictTrajectory()
    {
        Vector2 velocity = CalculateInitialVelocity();
        Vector2 position = throwPoint.position;

        for (int i = 0; i < maxPoints; i++)
        {
            // Calculate the next position in the trajectory
            Vector2 nextPosition = position + velocity * increment;

            // Check if the next position hits an object with the "Ground" tag
            RaycastHit2D hit = Physics2D.Raycast(position, velocity.normalized, velocity.magnitude * increment);
            if (hit.collider != null && hit.collider.CompareTag("Ground"))
            {
                // If "Ground" is hit, stop the trajectory prediction
                trajectoryLine.positionCount = i + 1;
                trajectoryLine.SetPosition(i, hit.point);
                break;
            }

            // Set the position in the LineRenderer
            trajectoryLine.SetPosition(i, new Vector3(position.x, position.y, 0f));

            // Update position and velocity for the next iteration
            position = nextPosition;
            velocity = CalculateNewVelocity(velocity, increment);
        }
    }

    private Vector2 CalculateInitialVelocity()
    {
        float radians = throwAngle * Mathf.Deg2Rad;
        Vector2 throwDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * throwForce;
        return throwDirection;
    }

    private Vector2 CalculateNewVelocity(Vector2 velocity, float increment)
    {
        velocity += Physics2D.gravity * increment;
        return velocity;
    }

    private void ShowTrajectory()
    {
        trajectoryLine.enabled = true;
    }

    private void HideTrajectory()
    {
        trajectoryLine.enabled = false;
    }
}
