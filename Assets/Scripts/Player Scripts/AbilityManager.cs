using UnityEngine;
using UnityEngine.SceneManagement;

public class AbilityManager : MonoBehaviour
{
    private bool isShieldAvailable = false;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "level2")
        {
            isShieldAvailable = true;
        }
        else
        {
            isShieldAvailable = false;
        }
    }

    public bool getIsShieldAvailable()
    {
        return isShieldAvailable;
    }

    public void setIsShieldAvailable(bool value)
    {
        isShieldAvailable = value;
    }
}
