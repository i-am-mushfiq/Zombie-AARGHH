using UnityEngine;

public class ShootAudioManager : MonoBehaviour
{
    public static ShootAudioManager Instance { get; private set; }

    [SerializeField] private AudioSource fireSound;
    [SerializeField] private AudioSource reloadSound;

    private void Awake()
    {
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

    public void PlayFireSound()
    {
        Debug.Log("in");
        if (fireSound != null)
        {
            fireSound.Play();
        }
        else
        {
            Debug.LogWarning("Fire sound not assigned!");
        }
    }

    public void PlayReloadSound()
    {
        if (reloadSound != null)
        {
            reloadSound.Play();
        }
        else
        {
            Debug.LogWarning("Reload sound not assigned!");
        }
    }
}
