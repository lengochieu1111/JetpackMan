using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class On_BaseEffect_AnimEvent : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BaseEffect explosion = animator?.GetComponent<BaseEffect>();
        explosion?.DestroyObject();
    }
}
