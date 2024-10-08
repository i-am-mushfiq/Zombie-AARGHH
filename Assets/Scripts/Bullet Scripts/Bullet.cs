using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;
    public int damage = 1;

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //HealthBar healthBar = other.GetComponent<HealthBar>();
            AnimatedEnemyController animatedEnemyController = other.GetComponent<AnimatedEnemyController>();
            if (animatedEnemyController != null)
            {
                animatedEnemyController.TakeDamage(damage);
            }
            gameObject.SetActive(false);
        }
        if (other.CompareTag("Destroyer"))
        {
            gameObject.SetActive(false);
        }

        //Debug.Log("Tried to hit xD");
    }
}
