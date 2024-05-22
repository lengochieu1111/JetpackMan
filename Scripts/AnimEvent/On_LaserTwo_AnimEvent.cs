using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class On_LaserTwo_AnimEvent : StateMachineBehaviour
{
    private LaserTwo _laserTwo;
    private Laser_UFO laser_UFO;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this._laserTwo = animator.GetComponentInParent<LaserTwo>();
        this._laserTwo?.SetActiveStatus_CapsuleCollider(true);

        if (this._laserTwo != null) return;
        this.laser_UFO = animator.GetComponentInParent<Laser_UFO>();
        this.laser_UFO?.SetActiveStatus_CapsuleCollider(true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this._laserTwo?.SetActiveStatus_CapsuleCollider(false);
        this.laser_UFO?.SetActiveStatus_CapsuleCollider(true);
    }

}
