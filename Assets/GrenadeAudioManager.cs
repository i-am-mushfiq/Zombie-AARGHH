using UnityEngine;

public class GrenadeAudioManager : MonoBehaviour
{
    public AudioSource throwSound;      // AudioSource for throw sound
    public AudioSource explosionSound;  // AudioSource for explosion sound

    void Awake()
    {
        // Ensure that the AudioSources are assigned
        if (throwSound == null || explosionSound == null)
        {
            Debug.LogError("AudioSources for throwSound or explosionSound are not assigned.");
        }
    }

    public void PlayThrowSound()
    {
        if (throwSound != null)
        {
            throwSound.Play();  // Play the throw sound
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
            explosionSound.Play();  // Play the explosion sound
        }
        else
        {
            Debug.LogWarning("Explosion sound AudioSource is not assigned.");
        }
    }
}
