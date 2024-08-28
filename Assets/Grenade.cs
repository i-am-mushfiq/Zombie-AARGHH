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

    public ParticleSystem groundExplosionEffect;
    public ParticleSystem airExplosionEffect;
    private GrenadeAudioManager audioManager;

    private bool hasExploded = false;
    private bool isGrounded = false;
    private float countdown;

    void Awake()
    {

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

        PlayExplosionSound();
        TriggerExplosionEffect();
        ApplyExplosionForceAndDamage();

        ResetGrenade();
        GrenadeController.Instance.ReleaseGrenade(gameObject);
    }

    private void PlayExplosionSound()
    {
        if (audioManager != null)
        {
            audioManager.PlayExplosionSound();
        }
    }

    private void TriggerExplosionEffect()
    {
        if (isGrounded && groundExplosionEffect != null)
        {
            Vector3 explosionPosition = new Vector3(transform.position.x, transform.position.y + groundExplosionYOffset, transform.position.z);
            ParticleSystem explosionInstance = Instantiate(groundExplosionEffect, explosionPosition, transform.rotation);
            Destroy(explosionInstance.gameObject, explosionInstance.main.duration);
        }
        else if (!isGrounded && airExplosionEffect != null)
        {
            Vector3 explosionPosition = new Vector3(transform.position.x, transform.position.y + airExplosionYOffset, transform.position.z);
            ParticleSystem explosionInstance = Instantiate(airExplosionEffect, explosionPosition, transform.rotation);
            Destroy(explosionInstance.gameObject, explosionInstance.main.duration);
        }
    }

    private void ApplyExplosionForceAndDamage()
    {
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
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.isTrigger == false)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
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
    private void ResetGrenade()
    {
        hasExploded = false;
        isGrounded = false;
        countdown = explosionDelay;
    }
}
