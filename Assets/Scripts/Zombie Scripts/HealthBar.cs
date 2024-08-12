using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform healthBar;
    [SerializeField]
    private int totalHealth = 5;
    private int health = 5;

    public delegate void HealthDepleted();
    public event HealthDepleted OnHealthDepleted;

    public void Start()
    {
        health = totalHealth;
    }

    public void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            OnHealthDepleted?.Invoke();
        }

        float healthRatio = (float)health / (float)totalHealth;
        healthBar.localScale = new Vector3(healthRatio, healthBar.localScale.y, healthBar.localScale.z);
    }

    public void ResetHealth()
    {
        health = totalHealth;
        healthBar.localScale = new Vector3(1, healthBar.localScale.y, healthBar.localScale.z);
    }
}
