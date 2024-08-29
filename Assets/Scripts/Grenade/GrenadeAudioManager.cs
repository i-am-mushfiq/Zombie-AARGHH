using UnityEngine;

public class GrenadeAudioManager : MonoBehaviour
{
    public static GrenadeAudioManager Instance { get; private set; }

    public AudioSource throwSound;
    public AudioSource tickSound;
    public AudioSource explosionSound;

    private void Awake()
    {
        // Implement the singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayThrowSound()
    {
        if (throwSound != null)
        {
            throwSound.Play();
        }
        else
        {
            Debug.LogWarning("Throw sound AudioSource is not assigned.");
        }
    }

    public void PlayExplosionSound()
    {
        if (explosionSound != null)
        {
            explosionSound.Play();
        }
        else
        {
            Debug.LogWarning("Explosion sound AudioSource is not assigned.");
        }
    }
}
