using UnityEngine;

public class HealthKit : MonoBehaviour
{
    public int healthAmount = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.addHealth(healthAmount);
            }

            HealthKitHandler.Instance.ReleaseHealthKit(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            HealthKitHandler.Instance.ReleaseHealthKit(gameObject);
        }
    }
}
