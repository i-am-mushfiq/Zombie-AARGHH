using UnityEngine;

public class Grenade : MonoBehaviour
{
    public static Grenade Instance { get; private set; }

    public float throwForce = 10f;
    public float explosionDelay = 3f;
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public int damage = 1;

    public float groundExplosionYOffset = 2f; // Offset for the explosion when on the ground
    public float airExplosionYOffset = 2f;   // Offset for the explosion when in the air

    public ParticleSystem groundExplosionEffect;  // Particle system for ground explosion
    public ParticleSystem airExplosionEffect;     // Particle system for air explosion
    private GrenadeAudioManager audioManager;

    private bool hasExploded = false;
    private bool isGrounded = false;  // Tracks whether the grenade is on the ground
    private float countdown;

    void Awake()
    {
        // Singleton pattern setup
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        countdown = explosionDelay;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * throwForce, ForceMode2D.Impulse);

        audioManager = GetComponent<GrenadeAudioManager>();
        if (audioManager != null)
        {
            audioManager.PlayThrowSound();  // Play throw sound
        }
    }

    void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown <= 0f && !hasExploded)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded)
            return;

        hasExploded = true;

        if (audioManager != null)
        {
            audioManager.PlayExplosionSound();
        }

        // Choose explosion effect based on grounded state
        if (isGrounded && groundExplosionEffect != null)
        {
            // Instantiate ground explosion effect with ground Y offset
            Vector3 explosionPosition = new Vector3(transform.position.x, transform.position.y + groundExplosionYOffset, transform.position.z);
            ParticleSystem explosionInstance = Instantiate(groundExplosionEffect, explosionPosition, transform.rotation);
            Destroy(explosionInstance.gameObject, explosionInstance.main.duration);  // Destroy after the effect finishes
        }
        else if (!isGrounded && airExplosionEffect != null)
        {
            // Instantiate air explosion effect with air Y offset
            Vector3 explosionPosition = new Vector3(transform.position.x, transform.position.y + airExplosionYOffset, transform.position.z);
            ParticleSystem explosionInstance = Instantiate(airExplosionEffect, explosionPosition, transform.rotation);
            Destroy(explosionInstance.gameObject, explosionInstance.main.duration);  // Destroy after the effect finishes
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D nearbyObject in colliders)
        {
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = rb.transform.position - transform.position;
                rb.AddForce(direction.normalized * explosionForce);
            }

            AnimatedEnemyController animatedEnemyController = nearbyObject.GetComponent<AnimatedEnemyController>();
            if (animatedEnemyController != null)
            {
                animatedEnemyController.TakeDamage(damage);
            }
        }

        // Destroy the grenade itself after the explosion
        Destroy(gameObject);
    }

    // Detect collision with the ground to determine if the grenade is grounded
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Assuming that any collision with a non-trigger collider means the grenade is grounded
        if (collision.collider.isTrigger == false)
        {
            isGrounded = true;
        }
    }

    // Detect when the grenade is no longer in contact with the ground
    void OnCollisionExit2D(Collision2D collision)
    {
        // If the grenade is no longer in contact with any non-trigger collider, it's no longer grounded
        if (collision.collider.isTrigger == false)
        {
            isGrounded = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
