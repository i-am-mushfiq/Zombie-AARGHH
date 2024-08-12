using UnityEngine;

public class MuzzleHandler : MonoBehaviour
{
    public Animator animator;
    public void PlayAnimationAndDeactivate()
    {
        animator.Play("Muzzle Flash");
    }

    public void OnAnimationComplete()
    {
        gameObject.SetActive(false);
    }
}
