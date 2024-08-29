using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryManager : MonoBehaviour
{
    public static TrajectoryManager Instance { get; private set; }

    public int maxPoints = 50;
    public float increment = 0.025f;
    public float rayOverlap = 1.1f;

    [SerializeField] private LayerMask groundLayer;

    private LineRenderer trajectoryLine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        trajectoryLine = GetComponent<LineRenderer>();
        trajectoryLine.positionCount = maxPoints;
    }

    public void PredictTrajectory(Vector2 initialPosition, Vector2 initialVelocity)
    {
        Vector2 velocity = initialVelocity;
        Vector2 position = initialPosition;

        for (int i = 0; i < maxPoints; i++)
        {
            Vector2 nextPosition = position + velocity * increment;

            // Raycast only against the "Ground" layer
            RaycastHit2D hit = Physics2D.Raycast(position, velocity.normalized, velocity.magnitude * increment, groundLayer);

            if (hit.collider != null)
            {
                Debug.Log($"Hit detected with {hit.collider.name} at position {hit.point}");
                if (hit.collider.CompareTag("Ground"))
                {
                    Debug.Log("Hit Ground");
                    trajectoryLine.positionCount = i + 1;
                    trajectoryLine.SetPosition(i, hit.point);

                    Debug.Log("Stopping the trajectory here.");
                    break;
                }
            }

            Debug.Log($"No hit detected, setting trajectory point at {position}");

            trajectoryLine.SetPosition(i, new Vector3(position.x, position.y, 0f)); // Set the next position in the trajectory

            // Update position and velocity for the next iteration
            position = nextPosition;
            velocity = CalculateNewVelocity(velocity, increment);
        }
    }

    private Vector2 CalculateNewVelocity(Vector2 velocity, float increment)
    {
        velocity += Physics2D.gravity * increment;
        return velocity;
    }

    public void ShowTrajectory()
    {
        trajectoryLine.enabled = true;
    }

    public void HideTrajectory()
    {
        trajectoryLine.enabled = false;
    }
}
