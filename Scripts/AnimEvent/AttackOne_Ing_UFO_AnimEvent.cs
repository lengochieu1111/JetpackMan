using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOne_Ing_UFO_AnimEvent : StateMachineBehaviour
{
    private UFO _ufo;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this._ufo = animator.GetComponent<UFO>();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (this._ufo == null) return;

        if (this._ufo.IsAttacking_One)
        {
            this._ufo.RequestAttackOne();
        }
        else if (this._ufo.IsAttacking_Two)
        {
            this._ufo.EndAttackTwo_Missile();
        }
    }

    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)

}
