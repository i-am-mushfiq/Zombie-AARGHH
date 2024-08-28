using UnityEngine;

public class Grenade : MonoBehaviour
{
    public static Grenade Instance { get; private set; }

    public float throwForce = 10f;
    public float explosionDelay = 3f;
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public int damage = 1;

    public float groundExplosionYOffset = 2f;
    public float airExplosionYOffset = 2f;

    private GrenadeAudioManager audioManager;

    private bool hasExploded = false;
    private bool isGrounded = false;
    private float countdown;

    void Start()
    {
        // Initialize properties
        ResetGrenade();

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

        if (isGrounded)
        {
            Vector3 explosionPosition = new Vector3(transform.position.x, transform.position.y + groundExplosionYOffset, transform.position.z);
            ParticleSystem explosionInstance = GroundExplosionPool.Instance.GetGroundExplosion();
            explosionInstance.transform.position = explosionPosition;
            explosionInstance.transform.rotation = transform.rotation;
            explosionInstance.Play();

            GroundExplosionPool.Instance.ReleaseGroundExplosion(explosionInstance);
        }
        else
        {
            Vector3 explosionPosition = new Vector3(transform.position.x, transform.position.y + airExplosionYOffset, transform.position.z);
            ParticleSystem explosionInstance = AirExplosionPool.Instance.GetAirExplosion();
            explosionInstance.transform.position = explosionPosition;
            explosionInstance.transform.rotation = transform.rotation;
            explosionInstance.Play();

            AirExplosionPool.Instance.ReleaseAirExplosion(explosionInstance);
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

        ResetGrenade();
        GrenadeController.Instance.ReleaseGrenade(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.isTrigger)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.collider.isTrigger)
        {
            isGrounded = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    private void ResetGrenade()
    {
        hasExploded = false;
        isGrounded = false;
        countdown = explosionDelay;
    }
}
