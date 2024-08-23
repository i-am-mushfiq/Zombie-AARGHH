using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
    public bool invertDirection = false;
    private Rigidbody2D rb;
    private float moveSpeed;
    private Transform playerTransform;
    private bool isFacingRight = true;

    public HealthBar healthBarScript;

    public LayerMask groundLayer;
    public float groundCheckDistance = 1f;

    [SerializeField]
    private int hitPoints = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        moveSpeed = Random.Range(minSpeed, maxSpeed);

        if (invertDirection)
        {
            moveSpeed *= -1f;
        }

        StayGrounded();

        if (healthBarScript != null)
        {
            healthBarScript.OnHealthDepleted += Death;
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;

            Vector2 movement = new Vector2(direction.x, 0) * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);

            if (movement.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (movement.x < 0 && isFacingRight)
            {
                Flip();
            }
        }

        StayGrounded();
    }

    private void StayGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        if (hit.collider != null)
        {
            transform.position = new Vector2(transform.position.x, hit.point.y + (transform.localScale.y / 2));
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void TakeDamage(int damage)
    {
        if (healthBarScript != null)
        {
            ZombieSoundManager.Instance.PlayZombieHurtSound();
            healthBarScript.TakeDamage();
        }
    }

    private void Death()
    {
        gameObject.SetActive(false);
        if (gameObject.tag != "Player")
        {
            ZombieSoundManager.Instance.PlayZombieHurtSound();
            ScoreManager.Instance.AddPoints(hitPoints);
        }
    }
}