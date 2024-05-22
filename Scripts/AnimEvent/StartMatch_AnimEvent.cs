using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMatch_AnimEvent : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<MainGameWidget>()?.StartGame();
    }
}