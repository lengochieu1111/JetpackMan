using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGunBullet_AnimEvent : StateMachineBehaviour
{
    private PlayerGunBullet_Laser _gunBullet;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this._gunBullet = animator.GetComponent<PlayerGunBullet_Laser>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this._gunBullet?.BulletTrace();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this._gunBullet?.DestroyObject();
    }

}
