using UnityEngine;
using TMPro;
using System.Collections;

public class AmmoManager : MonoBehaviour
{
    public int maxAmmo = 30;
    public int currentAmmo;
    public TextMeshProUGUI ammoText;
    public float reloadTime = 1.5f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    public bool UseAmmo()
    {
        if (isReloading)
        {
            Debug.Log("Reloading...");
            return false;
        }

        if (currentAmmo > 0)
        {
            currentAmmo--;
            UpdateAmmoUI();

            if (currentAmmo == 0)
            {
                StartCoroutine(ReloadCoroutine()); // Corrected usage
            }

            return true;
        }
        else
        {
            Debug.Log("Out of ammo!");
            return false;
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + currentAmmo;
        }
    }

    public void Reload()
    {
        if (!isReloading)
        {
            StartCoroutine(ReloadCoroutine()); // Corrected usage
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        isReloading = false;
        Debug.Log("Reloaded.");
    }
}
