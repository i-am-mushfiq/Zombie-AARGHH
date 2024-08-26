using UnityEngine;
using System.Collections;

public class AnimatedEnemyController : MonoBehaviour
{
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
    public bool invertDirection = false;
    private Rigidbody2D rb;
    private float moveSpeed;
    private Transform playerTransform;
    private bool isFacingRight = true;

    public ParticleSystem impactParticleSystem;

    public HealthBar healthBarScript;
    public LayerMask groundLayer;
    public float groundCheckDistance = 1f;
    public Animator animator;

    public float fadeOutTime = 3f;

    [SerializeField]
    private int hitPoints = 1;

    private bool isDead = false;
    private bool isMovementStopped = false;

    public GameObject healthBarFull;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        moveSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);

        if (invertDirection)
        {
            moveSpeed *= -1f;
        }

        StayGrounded();

        if (healthBarScript != null)
        {
            healthBarScript.OnHealthDepleted += Death;
            healthBarScript.OnHealthDamaged += (damage) => TakeDamage(damage);
        }
    }

    private void FixedUpdate()
    {
        if (isDead || isMovementStopped || GameManager.Instance.isPaused) return;

        bool wasMoving = animator.GetBool("isWalking");

        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            Vector2 movement = new Vector2(direction.x, 0) * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);

            bool isWalking = movement.x != 0;
            animator.SetBool("isWalking", isWalking);

            if (movement.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (movement.x < 0 && isFacingRight)
            {
                Flip();
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
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

    public void OnZombieHurtCompleted()
    {
        animator.SetBool("takeDamage", false);
        RestartMovement();
        animator.SetBool("isWalking", true);
    }

    public void TakeDamage(int damage)
    {
        healthBarScript.TakeDamage(damage);
        if (isDead) return;

        if (impactParticleSystem != null)
        {
            impactParticleSystem.Play();
        }

        StopMovement();
        ZombieSoundManager.Instance.PlayZombieHurtSound();
        animator.SetBool("takeDamage", true);
    }

    private void StopMovement()
    {
        isMovementStopped = true;
        rb.velocity = Vector2.zero;
        animator.SetBool("isWalking", false);
    }

    private void RestartMovement()
    {
        isMovementStopped = false;
    }

    private void Death()
    {
        isDead = true;
        ScoreManager.Instance.AddPoints(hitPoints);
        ZombieSoundManager.Instance.PlayZombieDeathSound();
        gameObject.tag = "DeadEnemy";
        animator.SetTrigger("death");

        if (impactParticleSystem != null)
        {
            impactParticleSystem.Play();
        }

        if (healthBarFull != null)
        {
            healthBarFull.SetActive(false);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            Collider2D enemyCollider = GetComponent<Collider2D>();
            if (playerCollider != null && enemyCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);
            }
        }

        MagazineSpawner.Instance.HandleMagazineSpawning(transform.position);
        HealthKitHandler.Instance.SpawnHealthKit(transform.position);

        StartCoroutine(FadeOutSprite(fadeOutTime));
    }

    private IEnumerator FadeOutSprite(float duration)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color startColor = spriteRenderer.color;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            spriteRenderer.color = endColor;

            if (EnemySpawner.Instance != null)
            {
                EnemySpawner.Instance.ReleaseEnemy(gameObject);
            }
            else
            {
                Debug.LogWarning("EnemySpawner instance not found.");
            }
        }
    }

    public void OnEnable()
    {
        // Set the tag to "Enemy"
        gameObject.tag = "Enemy";

        isDead = false;
        isMovementStopped = false;
        hitPoints = 1;

        animator.SetBool("isWalking", true);
        animator.SetBool("takeDamage", false);

        if (impactParticleSystem != null)
        {
            impactParticleSystem.Stop();
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }

        gameObject.SetActive(true);

        if (healthBarFull != null)
        {
            healthBarFull.SetActive(true);
        }
    }
}
