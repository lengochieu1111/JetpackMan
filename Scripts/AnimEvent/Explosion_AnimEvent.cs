using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_AnimEvent : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Explosion explosion = animator?.GetComponent<Explosion>();
        explosion?.DestroyObject();
    }
}
