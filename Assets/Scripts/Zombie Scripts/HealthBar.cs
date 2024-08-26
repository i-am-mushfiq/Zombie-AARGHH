using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform healthBar;
    [SerializeField]
    private int totalHealth = 5;
    private int health;

    public delegate void HealthDepleted();
    public event HealthDepleted OnHealthDepleted;

    public delegate void HealthDamaged(int damage);
    public event HealthDamaged OnHealthDamaged;

    void Start()
    {
        ResetHealth(); 
    }

    public void TakeDamage()
    {
        TakeDamage(1);
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            Debug.LogWarning("Damage cannot be negative.");
            return;
        }

        health -= damage;

        if (health < 0)
        {
            health = 0;
        }

        //OnHealthDamaged?.Invoke(damage);

        if (health <= 0)
        {
            OnHealthDepleted?.Invoke();
        }

        UpdateHealthBar();
    }

    public void ResetHealth()
    {
        health = totalHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float healthRatio = (float)health / totalHealth;
        healthBar.localScale = new Vector3(healthRatio, healthBar.localScale.y, healthBar.localScale.z);
    }
}
