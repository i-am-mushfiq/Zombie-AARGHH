using UnityEngine;

public class ZombieSoundManager : MonoBehaviour
{
    // Singleton instance
    public static ZombieSoundManager Instance { get; private set; }

    // Audio sources for different zombie sounds
    [SerializeField] private AudioSource zombieHurtSound;
    [SerializeField] private AudioSource zombieDeathSound;

    private void Awake()
    {
        // Ensure there's only one instance of ZombieSoundManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Function to play zombie hurt sound
    public void PlayZombieHurtSound()
    {
        if (zombieHurtSound != null)
        {
            zombieHurtSound.Play();
        }
        else
        {
            Debug.LogWarning("Zombie hurt sound not assigned!");
        }
    }

    // Function to play zombie death sound
    public void PlayZombieDeathSound()
    {
        if (zombieDeathSound != null)
        {
            zombieDeathSound.Play();
        }
        else
        {
            Debug.LogWarning("Zombie death sound not assigned!");
        }
    }
}
