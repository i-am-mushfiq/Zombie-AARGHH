using UnityEngine;
using Cysharp.Threading.Tasks;
public class ShieldManager : MonoBehaviour
{
    public GameObject shieldObject;
    public float shieldDuration = 30f;
    private bool isShieldActive = false;
    public int streakToActivate = 30;

    void Start()
    {
        if (shieldObject != null)
        {
            shieldObject.SetActive(false);
        }
    }

    void Update()
    {
        if (ScoreManager.Instance.GetKillStreak() == streakToActivate && !isShieldActive)
        {
            ActivateShield().Forget();
        }
    }

    private async UniTaskVoid ActivateShield()
    {
        ScoreManager.Instance.ResetKillStreak();
        isShieldActive = true;

        if (shieldObject != null)
        {
            shieldObject.SetActive(true);
        }
        await UniTask.Delay((int)(shieldDuration * 1000));

        if (shieldObject != null)
        {
            shieldObject.SetActive(false);
        }

        isShieldActive = false;
    }
}
