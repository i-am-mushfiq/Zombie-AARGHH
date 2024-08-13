using UnityEngine;

public class Magazine : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AmmoManager ammoManager = other.GetComponent<AmmoManager>();
            if (ammoManager != null)
            {
                ammoManager.AddMagazine(1);
            }
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Destroyer"))
        {
            gameObject.SetActive(false);
        }
    }
}
