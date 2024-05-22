using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_Dr_Bones_AnimEvent : StateMachineBehaviour
{
    private Dr_Bones _dr_Bones;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this._dr_Bones = animator.GetComponent<Dr_Bones>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (this._dr_Bones == null) return;

        if (this._dr_Bones.CanDestroyWhenDead())
        {
            this._dr_Bones.DestroyObject();
        }

    }

}
