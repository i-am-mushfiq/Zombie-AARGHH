using UnityEngine;
using TMPro;

public class AmmoManager : MonoBehaviour
{
    public int maxAmmo = 30;
    public int currentAmmo;
    public TextMeshProUGUI ammoText;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    public bool UseAmmo()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            UpdateAmmoUI();
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
}
