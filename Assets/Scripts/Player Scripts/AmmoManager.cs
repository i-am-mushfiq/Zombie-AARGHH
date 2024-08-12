using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public int maxAmmo = 30;
    public int currentAmmo;
    public TextMeshProUGUI ammoText;
    public float reloadTime = 1.5f;
    public Slider reloadSlider;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        if (reloadSlider != null)
        {
            reloadSlider.gameObject.SetActive(false);
            reloadSlider.value = 0;
        }
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

    public void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + currentAmmo;
        }
    }

    public async UniTaskVoid Reload()
    {
        if (!isReloading && currentAmmo < maxAmmo)
        {
            isReloading = true;
            Debug.Log("Reloading...");

            if (reloadSlider != null)
            {
                reloadSlider.gameObject.SetActive(true);
                reloadSlider.value = 0;
            }

            float elapsedTime = 0f;

            while (elapsedTime < reloadTime)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / reloadTime);

                if (reloadSlider != null)
                {
                    reloadSlider.value = progress;
                }

                await UniTask.Yield();
            }

            currentAmmo = maxAmmo;
            UpdateAmmoUI();
            isReloading = false;
            Debug.Log("Reloaded.");

            if (reloadSlider != null)
            {
                reloadSlider.gameObject.SetActive(false);
            }
        }
    }
}
