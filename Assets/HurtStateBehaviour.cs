using UnityEngine;

public class HurtStateBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AnimatedEnemyController enemyController = animator.GetComponent<AnimatedEnemyController>();
        if (enemyController != null)
        {
            //enemyController.ResetTakeDamage();
        }
    }
}
