using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform healthBar;
    [SerializeField]
    private int totalHealth = 5;
    private int health = 5;
    [SerializeField]
    private int hitPoints = 1;

    public ParticleSystemPool particleSystemPool;

    public void Start()
    {
        health = totalHealth;
    }

    public void TakeDamage()
    {
        health--;
        //Debug.Log("Health: " + health);

        if (health <= 0)
        {
            Death();
        }

        float healthRatio = (float)health / (float)totalHealth;
        //Debug.Log("Health Ratio: " + healthRatio);
        healthBar.localScale = new Vector3(healthRatio, healthBar.localScale.y, healthBar.localScale.z);
    }

    void Death()
    {
        gameObject.SetActive(false);
        if (gameObject.tag != "player")
        {
            ScoreManager.Instance.AddPoints(hitPoints);
        }
        particleSystemPool.ActivateParticleSystem(Vector3.zero);
    }

    public void ResetHealth()
    {
        health = totalHealth;
        healthBar.localScale = new Vector3(1, healthBar.localScale.y, healthBar.localScale.z);
    }
}
