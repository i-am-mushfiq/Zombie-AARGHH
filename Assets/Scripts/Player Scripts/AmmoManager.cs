using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Collections.Generic;

public class AmmoManager : MonoBehaviour
{
    public int maxAmmo = 30;
    public int currentAmmo;
    public int maxMagazines = 3;
    public int currentMagazines;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI magazineText;
    public float reloadTime = 1.5f;
    public Slider reloadSlider;
    public List<GameObject> magazineUIElements; // List of UI elements for magazines

    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        currentMagazines = maxMagazines;
        UpdateAmmoUI();
        UpdateMagazineUI();

        // Ensure magazine UI elements are inactive by default
        for (int i = 0; i < magazineUIElements.Count; i++)
        {
            if (magazineUIElements[i] != null)
            {
                magazineUIElements[i].SetActive(false);
            }
        }

        if (reloadSlider != null)
        {
            reloadSlider.gameObject.SetActive(false);
            reloadSlider.value = 0;
        }

        UpdateMagazineUI();
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

            if (currentAmmo == 0 && currentMagazines > 0)
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

    public void AddMagazine(int amount)
    {
        currentMagazines = Mathf.Clamp(currentMagazines + amount, 0, maxMagazines);
        UpdateMagazineUI();
    }

    public void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + currentAmmo;
        }
    }

    public void UpdateMagazineUI()
    {
        if (magazineText != null)
        {
            magazineText.text = "Magazines: " + currentMagazines;
        }

        // Update magazine UI elements based on the current number of magazines
        for (int i = 0; i < magazineUIElements.Count; i++)
        {
            if (magazineUIElements[i] != null)
            {
                magazineUIElements[i].SetActive(i < currentMagazines);
            }
        }
    }

    public async UniTaskVoid Reload()
    {
        if (!isReloading && currentAmmo < maxAmmo && currentMagazines > 0)
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

            currentMagazines--;
            currentAmmo = maxAmmo;
            UpdateAmmoUI();
            UpdateMagazineUI();
            isReloading = false;
            Debug.Log("Reloaded.");

            if (reloadSlider != null)
            {
                reloadSlider.gameObject.SetActive(false);
            }
        }
        else if (currentMagazines == 0)
        {
            Debug.Log("No more magazines left!");
        }
    }
}
