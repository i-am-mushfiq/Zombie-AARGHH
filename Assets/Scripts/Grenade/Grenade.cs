using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float explosionDelay = 3f;
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public int damage = 1;

    public float groundExplosionYOffset = 2f;
    public float airExplosionYOffset = 2f;

    public ParticleSystem groundExplosionEffect;
    public ParticleSystem airExplosionEffect;

    private bool hasExploded = false;
    private bool isGrounded = false;
    private float countdown;

    void Start()
    {
        countdown = explosionDelay;

        // Access the singleton instance of GrenadeAudioManager
        if (GrenadeAudioManager.Instance != null)
        {
            GrenadeAudioManager.Instance.PlayThrowSound();  // Play throw sound
        }
        else
        {
            Debug.LogWarning("GrenadeAudioManager singleton instance is not available.");
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
        if (GrenadeAudioManager.Instance != null)
        {
            GrenadeAudioManager.Instance.PlayExplosionSound();
        }
        else
        {
            Debug.LogWarning("GrenadeAudioManager singleton instance is not available.");
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
