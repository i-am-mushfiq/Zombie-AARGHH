using UnityEngine;
using UnityEngine.Pool;

public class GrenadeController : MonoBehaviour
{
    public static GrenadeController Instance { get; private set; }

    public GameObject grenadePrefab;
    public Transform throwPoint;
    public float throwForce = 10f;
    public float throwAngle = 45f;

    private ObjectPool<GameObject> grenadePool;
    private bool throwRight = true;
    private bool showTrajectory = false; // Start with trajectory hidden

    private PlayerController playerController; // Reference to PlayerController

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        PlayerController.OnFlip += UpdateThrowDirection;
    }

    private void OnDisable()
    {
        PlayerController.OnFlip -= UpdateThrowDirection;
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

        playerController = FindObjectOfType<PlayerController>(); // Get the PlayerController instance

        // Initially hide the trajectory
        ToggleTrajectory(false);
    }

    void Update()
    {
        if (showTrajectory)
        {
            PredictTrajectory();
        }
    }

    public void Throw(bool throwRight)
    {
        this.throwRight = throwRight;

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
            Vector2 initialVelocity = CalculateInitialVelocity();
            rb.AddForce(initialVelocity, ForceMode2D.Impulse);

            // Hide the trajectory after throwing the grenade
            ToggleTrajectory(false);
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
        Vector2 initialVelocity = CalculateInitialVelocity();
        Vector2 initialPosition = throwPoint.position;

        TrajectoryManager.Instance.PredictTrajectory(initialPosition, initialVelocity);
    }

    private Vector2 CalculateInitialVelocity()
    {
        float radians = throwAngle * Mathf.Deg2Rad;
        Vector2 throwDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * throwForce;

        if (playerController != null && playerController.IsMoving) // Check if the player is moving
        {
            float moveSpeed = playerController.MoveSpeed; // Get the move speed
            throwDirection += throwDirection.normalized * moveSpeed; // Adjust the throw direction based on move speed
        }

        if (!throwRight)
        {
            throwDirection.x *= -1;
        }

        return throwDirection;
    }

    private void UpdateThrowDirection(bool facingRight)
    {
        throwRight = facingRight;
    }

    // Function to toggle trajectory visibility
    public void ToggleTrajectory(bool shouldShow)
    {
        showTrajectory = shouldShow;

        if (shouldShow)
        {
            TrajectoryManager.Instance.ShowTrajectory();
        }
        else
        {
            TrajectoryManager.Instance.HideTrajectory();
        }
    }
}
