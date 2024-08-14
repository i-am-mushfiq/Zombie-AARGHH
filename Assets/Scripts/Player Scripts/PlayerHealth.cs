using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Transform healthBar;
    [SerializeField]
    private int totalHealth = 5;
    private int health = 5;

    // Reference to the GameOver UI parent
    [SerializeField]
    private GameObject gameOverUI;

    public void Start()
    {
        health = totalHealth;
    }

    public void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            Death();
        }

        float healthRatio = (float)health / (float)totalHealth;
        healthBar.localScale = new Vector3(healthRatio, healthBar.localScale.y, healthBar.localScale.z);
    }

    public void Death()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            Debug.Log("GameOver UI activated.");
            //Invoke("DeactivatePlayer", 1.5f); // Delay deactivating the player by 0.1 seconds
        }
        else
        {
            Debug.Log("GameOver UI is not assigned in the Inspector.");
        }
        DeactivatePlayer();
    }

    void DeactivatePlayer()
    {
        gameObject.SetActive(false);
    }

    public void ResetHealth()
    {
        health = totalHealth;
        healthBar.localScale = new Vector3(1, healthBar.localScale.y, healthBar.localScale.z);
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // Deactivate the GameOver UI when health is reset
        }
        else
        {
            Debug.LogError("GameOver UI is not assigned in the Inspector.");
        }
    }

    public void addHealth(int additionalHealth)
    {
        health += additionalHealth;

        if (health > totalHealth)
        {
            health = totalHealth;
        }

        float healthRatio = (float)health / (float)totalHealth;
        healthBar.localScale = new Vector3(healthRatio, healthBar.localScale.y, healthBar.localScale.z);
    }
}
