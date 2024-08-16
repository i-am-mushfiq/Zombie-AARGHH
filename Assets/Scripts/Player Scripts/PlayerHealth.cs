using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerHealth : MonoBehaviour
{
    public Transform healthBar;
    [SerializeField]
    private int totalHealth = 5;
    private int health = 5;

    public GameObject blood_effect_UI;

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

        // Activate the object, wait for 1 second, then deactivate
        ActivateAndReactivateObject().Forget();
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

    private async UniTaskVoid ActivateAndReactivateObject()
    {
        // Replace this with the actual game object you want to activate and deactivate


        blood_effect_UI.SetActive(true); // Activate the object
        await UniTask.Delay(1000); // Wait for 1 second
        blood_effect_UI.SetActive(false); // Deactivate the object
    }
}
