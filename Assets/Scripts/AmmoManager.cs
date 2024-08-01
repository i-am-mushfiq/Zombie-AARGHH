using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

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
                Reload().Forget();
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

    public async UniTaskVoid Reload()
    {
        if (!isReloading)
        {
            isReloading = true;
            Debug.Log("Reloading...");
            await UniTask.Delay((int)(reloadTime * 1000)); 
            currentAmmo = maxAmmo;
            UpdateAmmoUI();
            isReloading = false;
            Debug.Log("Reloaded.");
        }
    }
}
